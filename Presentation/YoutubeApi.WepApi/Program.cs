using YoutubeApi.Persistence;
using YoutubeApi.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
var env = builder.Environment;
// Uygulaman�n �al��t��� ortam bilgilerini (Development, Production vb.) al�r.

builder.Configuration
    .SetBasePath(env.ContentRootPath)
    // Yap�land�rma dosyalar�n�n k�k yolunu (uygulaman�n ana klas�r�n�) ayarlar.

    .AddJsonFile("appsettings.json", optional: false)
    // appsettings.json dosyas�n� zorunlu olarak y�kler (bulunmazsa hata verir).

    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
    // Ortama �zel (�rn: appsettings.Development.json) yap�land�rmay� ekler (bulunmazsa hata vermez).

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
