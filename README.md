# GRAPE 🍇

Grape, ASP.NET Core MVC mimarisi kullanılarak geliştirilmiş, modern bir müzik ve albüm inceleme/yönetim platformudur. Kullanıcıların albümleri incelemesine, favorilerine eklemesine ve sanatçıları takip etmesine olanak tanır.

## 🚀 Özellikler

- **Müzik Veritabanı:** Albümler ve Sanatçılar için detaylı veri yönetimi.
- **Kullanıcı Etkileşimi:** Albüm puanlama, yorum yapma (Review) ve favorilere ekleme.
- **Gelişmiş Arama:** Albüm ve sanatçılar için arama özelliği.
- **Admin Paneli:** İçerik yönetimi için özel admin arayüzü.
- **Kullanıcı Profilleri:** Kişisel profil sayfaları ve aktiviteler.
- **Güvenlik:** ASP.NET Core Identity ile güvenli üyelik ve giriş sistemi.

## 🛠 Teknolojiler

- **Backend:** .NET 8.0 (ASP.NET Core MVC)
- **Veritabanı:** MySQL (Entity Framework Core & Pomelo)
- **Frontend:** Razor Views, HTML5, CSS3, JavaScript
- **ORM:** Entity Framework Core

## ⚙️ Kurulum ve Çalıştırma

Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları izleyin:

### Ön Gereksinimler
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/installer/)

### Adımlar

1. **Repoyu Klonlayın:**
   ```bash
   git clone <repo-url>
   cd VolumeWeb
   ```

2. **Veritabanı Ayarları:**
   `appsettings.json` dosyasındaki veritabanı bağlantı dizesini kendi MySQL yapılandırmanıza göre düzenleyin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "server=localhost;database=grape;user=grape_app;password=guclu_sifre"
   }
   ```

3. **Veritabanını Oluşturun:**
   Uygulama ilk çalıştırıldığında veritabanını otomatik olarak oluşturur (`EnsureCreated` kullanılmıştır). Ancak migration kullanmak isterseniz:
   ```bash
   dotnet ef database update
   ```

4. **Projeyi Çalıştırın:**
   ```bash
   dotnet run
   ```

5. **Tarayıcıda Açın:**
   Adres çubuğuna `http://localhost:5180` yazarak uygulamaya erişebilirsiniz.

## 🔑 Varsayılan Admin Hesabı

Uygulama ilk kez çalıştırıldığında otomatik olarak bir admin kullanıcısı oluşturur:

- **Email:** `admin@grape.com`
- **Şifre:** `Admin123!`

---
YT: https://youtu.be/gCIqq3zo-rE
