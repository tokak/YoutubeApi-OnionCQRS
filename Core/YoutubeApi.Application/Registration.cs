using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YoutubeApi.Application.Exceptions;
using System.Globalization;
using MediatR;
using YoutubeApi.Application.Beheviors;

namespace YoutubeApi.Application
{
    //  uygulamadaki servis kayıtlarını topluca yapmak için kullanılan yardımcı sınıf
    public static class Registration
    {
        // Extension method: IServiceCollection üzerine eklenebilen bir metot
        // Program.cs veya Startup.cs içinde kolayca çağrılabilir
        public static void AddApplication(this IServiceCollection services)
        {
            // Şu an çalışmakta olan assembly'i (derlenmiş projeyi) alıyoruz
            var assembly = Assembly.GetExecutingAssembly();

            // MediatR servislerini dependency injection container'a ekliyoruz
            // Ve bu assembly içindeki tüm IRequestHandler, INotificationHandler vb. sınıfları otomatik kaydediyoruz
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            services.AddTransient<ExceptionMiddleware>();

            services.AddValidatorsFromAssembly(assembly);
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("tr");
            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(FluentValidationBehevior<,>));

        }
    }

}
