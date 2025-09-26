using YoutubeApi.Persistence;
using YoutubeApi.Application;
using YoutubeApi.Mapper;
using YoutubeApi.Application.Exceptions;
using YoutubeApi.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


var env = builder.Environment;
// Uygulamanýn çalýþtýðý ortam bilgilerini (Development, Production vb.) alýr.

builder.Configuration
    .SetBasePath(env.ContentRootPath)
    // Yapýlandýrma dosyalarýnýn kök yolunu (uygulamanýn ana klasörünü) ayarlar.

    .AddJsonFile("appsettings.json", optional: false)
    // appsettings.json dosyasýný zorunlu olarak yükler (bulunmazsa hata verir).

    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
// Ortama özel (örn: appsettings.Development.json) yapýlandýrmayý ekler (bulunmazsa hata vermez).

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddCustomMapper();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Youtube API", Version = "v1", Description = "Youtube API swagger client." });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "'Bearer' yazipp bosluk biraktiktan sonra Token'i Girebilirsiniz \r\n\r\n Örnegin: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandlingMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
