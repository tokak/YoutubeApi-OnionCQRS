using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeApi.Application.Bases;

namespace YoutubeApi.Application.Features.Auth.Exceptions
{
    public class RefreshTokenShouldBeExpiredException : BaseException
    {
        public RefreshTokenShouldBeExpiredException() : base("Oturum süresi sona ermiştir.Lütfen tekrar giriş yapınız") { }
    }
}
