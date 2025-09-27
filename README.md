## Youtube API - Onion Architecture & CQRS ile API Geliştirme

Bu proje,  Onion Architecture (Soğan Mimarisi) ve CQRS  tasarım desenlerini temel alarak geliştirildi. .NET 9.0 kullanılarak  RESTful API mimarisi içermektedir

### Kullanılan Teknolojiler ve Katmanlar

| Katman                                | Açıklama                                                                 |
|---------------------------------------|--------------------------------------------------------------------------|
| **ASP.NET Core 9 Web API**           | Ana API projesi ve sunum katmanı                                        |
| **Onion Architecture**               | Temiz ve sürdürülebilir mimari yaklaşımı                                |
| **CQRS & MediatR**                   | Komut ve sorgu ayrımı; işleme yönlendirmesi                             |
| **JWT Authentication**              | Erişim Belirteci (Token) ve Yenileme Belirteci (Refresh Token) ile kimlik doğrulama |
| **ASP.NET Identity**                | Kullanıcı ve rol yönetimi                                               |
| **Entity Framework Core (SQL Server)** | Veritabanı işlemleri                                                  |
| **FluentValidation**                | İşlem hattı (Pipeline) tabanlı request doğrulama                        |
| **Redis Cache**                     | Okuma işlemlerini hızlandırmak için önbellekleme mekanizması            |
| **AutoMapper**                      | DTO ↔ Entity dönüşümlerinin kolaylaştırılması                           |
| **Swagger**                         | API dökümantasyonu ve test arayüzü       

### Kurulum
"ConnectionStrings": {
  "DefaultConnection": "Server=Veritabanı adresinizi yazınız;Database=YoutubeApiDb;Trusted_Connection=True;TrustServerCertificate=true;"
},
packege manage console içerisine
update-database işlemi yazın veritabanı işlemleri oluşturulacaktır.

### Resimler 
<img width="468" height="818" alt="image" src="https://github.com/user-attachments/assets/53270e46-fbad-4f5c-bc10-bf0b5ca2df1e" />
<img width="1920" height="902" alt="image" src="https://github.com/user-attachments/assets/c6bbfa1d-19ed-44f6-96a6-b7c0e55c8a05" />


