using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using YoutubeApi.Application.Bases;
using YoutubeApi.Application.Features.Auth.Rules;
using YoutubeApi.Application.Interfaces.AutoMapper;
using YoutubeApi.Application.Interfaces.Tokens;
using YoutubeApi.Application.Interfaces.UnitOfWorks;
using YoutubeApi.Domain.Entities;

namespace YoutubeApi.Application.Features.Auth.Command.Login
{
    // Giriş işlemini (login) yöneten sınıf
    // IRequestHandler: MediatR'den gelen komut isteğini işler ve bir cevap döner
    public class LoginCommandHandler : BasesHandler, IRequestHandler<LoginCommandRequest, LoginCommandResponse>
    {
        private readonly UserManager<User> userManager; // Kullanıcı işlemleri için (bulma, şifre kontrolü vs.)
        private readonly IConfiguration configuration; // JWT ayarlarını okumak için
        private readonly ITokenService tokenService; // JWT token oluşturmak için
        private readonly AuthRules authRules; // Giriş kurallarını kontrol eden sınıf

        // Constructor: Bu sınıf çağrıldığında gerekli servisleri alır
        public LoginCommandHandler(UserManager<User> userManager,IConfiguration configuration,ITokenService tokenService,AuthRules authRules,IMapper mapper,IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor) : base(mapper, unitOfWork, httpContextAccessor)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.tokenService = tokenService;
            this.authRules = authRules;
        }

        // Giriş işlemini gerçekleştiren fonksiyon
        public async Task<LoginCommandResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            User user = await userManager.FindByEmailAsync(request.Email);

            bool checkPassword = await userManager.CheckPasswordAsync(user, request.Password);

            // Eğer kullanıcı yoksa veya şifre yanlışsa hata fırlat
            await authRules.EmailOrPasswordShouldNotBeInvalid(user, checkPassword);

            // Kullanıcının rollerini (admin, user vb.) getir
            IList<string> roles = await userManager.GetRolesAsync(user);

            // Kullanıcı için JWT token oluştur
            JwtSecurityToken token = await tokenService.CreateToken(user, roles);

            // Yeni bir refresh token (yenileme anahtarı) oluştur
            string refreshToken = tokenService.GenerateRefreshToken();

            // Refresh token kaç gün geçerli olacak onu ayarlardan al
            _ = int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            // Kullanıcıya refresh token ve son kullanma tarihini ata
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            // Kullanıcı bilgilerini güncelle
            await userManager.UpdateAsync(user);

            // Güvenlik damgasını yenile (kullanıcı bilgileri değiştiyse güvenliği sağlamak için)
            await userManager.UpdateSecurityStampAsync(user);

            // Token'ı string'e çevir (gönderebilmek için)
            string _token = new JwtSecurityTokenHandler().WriteToken(token);

            // Kullanıcının giriş token'ını kaydet (çıkış yaptırma gibi işlemlerde kullanılabilir)
            await userManager.SetAuthenticationTokenAsync(user, "Default", "AccessToken", _token);

            // Giriş başarılıysa token, refresh token ve süresini döndür
            return new()
            {
                Token = _token,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo // Token'ın sona ereceği zaman
            };
        }
    }
}
