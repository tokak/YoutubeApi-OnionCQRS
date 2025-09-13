using YoutubeApi.Persistence;
using YoutubeApi.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
var env = builder.Environment;
// Uygulamanýn çalýþtýðý ortam bilgilerini (Development, Production vb.) alýr.

builder.Configuration
    .SetBasePath(env.ContentRootPath)
    // Yapýlandýrma dosyalarýnýn kök yolunu (uygulamanýn ana klasörünü) ayarlar.

    .AddJsonFile("appsettings.json", optional: false)
    // appsettings.json dosyasýný zorunlu olarak yükler (bulunmazsa hata verir).

    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
    // Ortama özel (örn: appsettings.Development.json) yapýlandýrmayý ekler (bulunmazsa hata vermez).

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
