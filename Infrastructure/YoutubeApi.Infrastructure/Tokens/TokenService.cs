using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YoutubeApi.Application.Interfaces.Tokens;
using YoutubeApi.Domain.Entities;

namespace YoutubeApi.Infrastructure.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager; 
        private readonly TokenSettings _tokenSettings;   // Token ayarlarını (secret, süre, issuer vs.) tutan yapı
       
        public TokenService(UserManager<User> userManager, IOptions<TokenSettings> options)
        {
            _userManager = userManager;
            _tokenSettings = options.Value;
        }

        // Kullanıcı için JWT (token) oluşturma
        public async Task<JwtSecurityToken> CreateToken(User user, IList<string> roles)
        {
            // Kullanıcıya ait bilgiler (claim) ekleniyor
            var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),          
            new Claim(JwtRegisteredClaimNames.Email, user.Email)                
        };

            // Kullanıcının rollerini  ekle
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Tokeni imzalamak için güvenlik anahtarı oluştur
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));

            // Token ayarlarıyla birlikte oluştur
            var token = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,             // Token'i kim oluşturdu
                audience: _tokenSettings.Audience,         // Token'i kim kullanabilir
                expires: DateTime.Now.AddMinutes(_tokenSettings.TokenValidityInMunitues), // Token geçerlilik süresi
                claims: claims,                            // Kullanıcı bilgileri
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256) // Şifreleme
            );

            // Kullanıcının claimlerini veritabanına ekle
            await _userManager.AddClaimsAsync(user, claims);

            return token; // Oluşturulan token döndür
        }

        // Rastgele bir refresh token (yenileme anahtarı) üretir
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64]; // 64 byte'lık rastgele sayı üret
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);      // Rastgele doldur
            return Convert.ToBase64String(randomNumber); // Base64 string olarak döndür
        }

        // Süresi bitmiş bir token’den kullanıcı bilgilerini (Claims) alır
        public ClaimsPrincipal GetPrincipalFromExpriredToken(string? token)
        {
            // Token doğrulama kuralları
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = false, // Issuer kontrol etme
                ValidateAudience = false, // Audience kontrol etme
                ValidateIssuerSigningKey = true, // İmza anahtarını kontrol et
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret)), // Anahtar
                ValidateLifetime = false, // Süre dolmuş olsa bile kontrol etme
            };

            JwtSecurityTokenHandler tokenHandler = new();

            // Token doğrula
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            // Doğrulanan token HMACSHA256 ile mi imzalanmış kontrol et
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token bulunamadı."); // Hatalıysa hata fırlat
            }

            return principal; // Token içindeki kullanıcı bilgilerini döndür
        }
    }

}
