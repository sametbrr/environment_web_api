# EnvironmentConfigurator

> ASP.NET Core (.NET 8+) için environment-aware (ortama duyarlı) konfigürasyon paketi.
> Tek satır kodla `appsettings.{Environment}.json` yüklemesi yapar **ve** ilk build'de publish profillerini, `web.config` ve ortam `appsettings` dosyalarını projeye otomatik oluşturur.

---

## 📦 Bu paket ne yapar?

- ✅ `appsettings.json` → `appsettings.{Environment}.json` → environment variables sırasıyla yüklenir, tek satır.
- ✅ Pakete eklenince ilk `dotnet build`'de eksik konfigürasyon dosyalarını projeye **otomatik kopyalar** (var olanı asla ezmez).
- ✅ Mevcut projelere `dotnet add package` ile saniyeler içinde entegre olur.
- ✅ Hedef: **.NET 8 ve sonrası** (net9 / net10 dahil).

---

## 🚀 Kurulum

```bash
dotnet add package EnvironmentConfigurator
```

> Şu an paket yalnızca lokal `.nupkg` olarak üretiliyor. Lokal paketi kullanmak için (yayınlanmadıysa) `.nupkg`'in bulunduğu klasörü kaynak olarak ekle:
> ```bash
> dotnet add package EnvironmentConfigurator --source ./artifacts
> ```

---

## ⚡ Kullanım

### Tek satır (önerilen)

```csharp
using EnvironmentConfigurator;

var builder = WebApplication.CreateBuilder(args);

builder.AddEnvironmentConfiguration();   // appsettings.{Environment}.json + env variables

var app = builder.Build();
app.Run();
```

`AddEnvironmentConfiguration`, .NET 8'deki `IHostApplicationBuilder` üzerinde çalışır; `WebApplicationBuilder` ile doğrudan kullanılır.

### Esnek varyant

Builder'a erişimin yoksa veya `IConfigurationBuilder` seviyesinde çalışıyorsan:

```csharp
builder.Configuration.AddEnvironmentJsonFiles(builder.Environment);
```

### Seçenekler (`EnvironmentConfiguratorOptions`)

```csharp
builder.AddEnvironmentConfiguration(options =>
{
    options.BaseSettingsOptional        = false;  // appsettings.json yoksa hata fırlatma (varsayılan: false)
    options.ReloadOnChange              = true;   // dosya değişince yeniden yükle (varsayılan: true)
    options.IncludeEnvironmentVariables = true;   // env variables kaynağını ekle (varsayılan: true)
    options.BasePath                    = "config"; // JSON dosyaları farklı klasördeyse (varsayılan: çalışma dizini)
});
```

---

## 🛠️ Otomatik scaffold (dosya oluşturma)

Paketi kurup **ilk kez build** ettiğinde, projede **bulunmayan** şu dosyalar otomatik oluşturulur:

```
appsettings.Beta.json
appsettings.Production.json
web.config
Properties/PublishProfiles/Beta-FolderProfile.pubxml
Properties/PublishProfiles/Development-FolderProfile.pubxml
Properties/PublishProfiles/Production-FolderProfile.pubxml
```

- **Var olan dosyalar asla ezilmez** — sadece eksik olanlar eklenir, düzenlemelerin korunur.
- Oluşturulan dosyalar build çıktısında loglanır: `EnvironmentConfigurator: scaffolded ...`
- `.pubxml` şablonlarında projeye-özel `ProjectGuid` **yoktur** (folder publish için gerekmez, çakışmayı önler) — `EnvironmentName` her dosyada hazırdır.

### Scaffold'u kapatma

İstemiyorsan `.csproj` içine ekle:

```xml
<PropertyGroup>
  <EnvironmentConfiguratorScaffold>false</EnvironmentConfiguratorScaffold>
</PropertyGroup>
```

---

## 🌍 Ortam (Environment) seçimi

Hangi `appsettings.{X}.json`'ın yükleneceğini `ASPNETCORE_ENVIRONMENT` değişkeni belirler.

| Senaryo | Nerede ayarlanır |
|---|---|
| Debug / lokal | `Properties/launchSettings.json` → profil `environmentVariables` |
| Komut satırı | `ASPNETCORE_ENVIRONMENT=Beta dotnet run` |
| IIS / Web Deploy | `web.config` → `<environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />` |

**Override mantığı:** `appsettings.json` ortak ayarları taşır; `appsettings.{Environment}.json` yalnızca o ortamda değişen anahtarları ezer.

`launchSettings.json` örneği:

```json
{
  "profiles": {
    "Beta": {
      "commandName": "Project",
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": { "ASPNETCORE_ENVIRONMENT": "Beta" }
    }
  }
}
```

---

## 📤 Publish / Web Deploy

Visual Studio'da publish profillerini (`.pubxml`) kullan. Her profil `<EnvironmentName>` ile ortamını belirtir; yayın sırasında doğru konfigürasyon seçilir.

```xml
<Project>
  <PropertyGroup>
    <EnvironmentName>Beta</EnvironmentName>
    <PublishProvider>FileSystem</PublishProvider>
    <PublishUrl>bin\Release\net8.0\publish\</PublishUrl>
    <WebPublishMethod>FileSystem</WebPublishMethod>
    <TargetFramework>net8.0</TargetFramework>
    <SelfContained>false</SelfContained>
  </PropertyGroup>
</Project>
```

---

## 🧪 Örnek uygulama

[samples/EnvironmentWebApi](samples/EnvironmentWebApi) paketin **tam çalışan canlı kullanım örneğidir**. Paketi `ProjectReference` ile tüketir ([Program.cs](samples/EnvironmentWebApi/Program.cs) tek satır), ortam `appsettings` dosyaları, publish profilleri ve `launchSettings.json` ile birlikte gelir.

`/test/environment-name` endpoint'i aktif ortam adını döndürür ([TestController.cs](samples/EnvironmentWebApi/Controllers/TestController.cs)).

Çalıştır:

```bash
cd samples/EnvironmentWebApi
ASPNETCORE_ENVIRONMENT=Beta dotnet run
# başka terminalde:
curl http://localhost:5000/test/environment-name   # -> "Beta"
```

---

## 🏗️ Proje yapısı

```
Environment-WebApi.sln
├── src/EnvironmentConfigurator/           → NuGet paketi (kod + scaffold target + şablonlar)
├── samples/EnvironmentWebApi/             → paketi kullanan örnek API
└── tests/EnvironmentConfigurator.Tests/   → xUnit testleri
```

---

## 📦 Paketi lokal üretme

```bash
dotnet pack src/EnvironmentConfigurator -c Release -o ./artifacts
# -> artifacts/EnvironmentConfigurator.1.0.0.nupkg
```

---

## 📁 Lisans

MIT — bkz. [LICENSE.txt](LICENSE.txt).
