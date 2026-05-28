# EnvironmentConfigurator

[![NuGet](https://img.shields.io/nuget/v/EnvironmentConfigurator.svg)](https://www.nuget.org/packages/EnvironmentConfigurator)
[![Downloads](https://img.shields.io/nuget/dt/EnvironmentConfigurator.svg)](https://www.nuget.org/packages/EnvironmentConfigurator)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)

**EN:** Environment-aware configuration for ASP.NET Core (.NET 8+). Loads `appsettings.{Environment}.json` with a single call **and**, on first build, auto-scaffolds publish profiles, `web.config` and per-environment `appsettings` files into your project.

**TR:** ASP.NET Core (.NET 8+) için ortam duyarlı konfigürasyon. Tek bir çağrı ile `appsettings.{Environment}.json` dosyasını yükler **ve** ilk derlemede publish profilleri, `web.config` ve ortam bazlı `appsettings` dosyalarını projenize otomatik olarak oluşturur.

---

## 📦 What does this package do? | Bu paket ne yapar?

**EN:**
- ✅ Loads `appsettings.json` → `appsettings.{Environment}.json` → environment variables, in one line.
- ✅ On the first `dotnet build` after install, **auto-copies** missing configuration files into your project (never overwrites existing ones).
- ✅ Integrates into existing projects in seconds via `dotnet add package`.
- ✅ Targets **.NET 8 and later** (incl. net9 / net10).

**TR:**
- ✅ Tek satırda `appsettings.json` → `appsettings.{Environment}.json` → ortam değişkenlerini yükler.
- ✅ Kurulumdan sonraki ilk `dotnet build` işleminde eksik konfigürasyon dosyalarını projenize **otomatik kopyalar** (var olanları asla üzerine yazmaz).
- ✅ `dotnet add package` ile mevcut projelere saniyeler içinde entegre olur.
- ✅ **.NET 8 ve üzerini** hedefler (net9 / net10 dahil).

---

## 🚀 Install | Kurulum

```bash
dotnet add package EnvironmentConfigurator
```

**EN:** Available on [nuget.org](https://www.nuget.org/packages/EnvironmentConfigurator).

**TR:** [nuget.org](https://www.nuget.org/packages/EnvironmentConfigurator) üzerinde mevcuttur.

---

## ⚡ Usage | Kullanım

### One line (recommended) | Tek satır (önerilen)

```csharp
using EnvironmentConfigurator;

var builder = WebApplication.CreateBuilder(args);

builder.AddEnvironmentConfiguration();   // appsettings.{Environment}.json + env variables

var app = builder.Build();
app.Run();
```

**EN:** `AddEnvironmentConfiguration` runs on the .NET 8 `IHostApplicationBuilder`; use it directly with `WebApplicationBuilder`.

**TR:** `AddEnvironmentConfiguration`, .NET 8 `IHostApplicationBuilder` üzerinde çalışır; doğrudan `WebApplicationBuilder` ile kullanın.

### Flexible variant | Esnek kullanım

**EN:** If you don't have access to the builder, or work at the `IConfigurationBuilder` level:

**TR:** Builder'a erişiminiz yoksa veya `IConfigurationBuilder` seviyesinde çalışıyorsanız:

```csharp
builder.Configuration.AddEnvironmentJsonFiles(builder.Environment);
```

### Options (`EnvironmentConfiguratorOptions`) | Seçenekler

```csharp
builder.AddEnvironmentConfiguration(options =>
{
    options.BaseSettingsOptional        = false;  // throw if appsettings.json is missing (default: false) | appsettings.json yoksa hata fırlat (varsayılan: false)
    options.ReloadOnChange              = true;   // reload on file change (default: true) | dosya değişiminde yeniden yükle (varsayılan: true)
    options.IncludeEnvironmentVariables = true;   // add the env variables source (default: true) | ortam değişkenleri kaynağını ekle (varsayılan: true)
    options.BasePath                    = "config"; // if JSON files live in another folder (default: working dir) | JSON dosyaları başka klasördeyse (varsayılan: çalışma dizini)
});
```

---

## 🛠️ Auto-scaffold (file generation) | Otomatik oluşturma (dosya üretimi)

**EN:** After installing the package and building **for the first time**, the following files that are **not already present** in your project are generated automatically:

**TR:** Paketi kurduktan ve **ilk kez** derledikten sonra, projenizde **henüz bulunmayan** aşağıdaki dosyalar otomatik olarak oluşturulur:

```
appsettings.Beta.json
appsettings.Production.json
web.config
Properties/PublishProfiles/Beta-FolderProfile.pubxml
Properties/PublishProfiles/Development-FolderProfile.pubxml
Properties/PublishProfiles/Production-FolderProfile.pubxml
```

**EN:**
- **Existing files are never overwritten** — only missing ones are added, your edits are preserved.
- Generated files are logged in the build output: `EnvironmentConfigurator: scaffolded ...`
- The `.pubxml` templates contain no project-specific `ProjectGuid` (not needed for folder publish, avoids conflicts) — `EnvironmentName` is set in each file.

**TR:**
- **Var olan dosyalar asla üzerine yazılmaz** — yalnızca eksik olanlar eklenir, düzenlemeleriniz korunur.
- Oluşturulan dosyalar derleme çıktısında loglanır: `EnvironmentConfigurator: scaffolded ...`
- `.pubxml` şablonları projeye özel `ProjectGuid` içermez (folder publish için gerekli değildir, çakışmaları önler) — her dosyada `EnvironmentName` ayarlanır.

### Disabling scaffold | Otomatik oluşturmayı kapatma

**EN:** If you don't want it, add to your `.csproj`:

**TR:** İstemiyorsanız `.csproj` dosyanıza ekleyin:

```xml
<PropertyGroup>
  <EnvironmentConfiguratorScaffold>false</EnvironmentConfiguratorScaffold>
</PropertyGroup>
```

---

## 🌍 Environment selection | Ortam seçimi

**EN:** The `ASPNETCORE_ENVIRONMENT` variable decides which `appsettings.{X}.json` is loaded.

**TR:** Hangi `appsettings.{X}.json` dosyasının yükleneceğine `ASPNETCORE_ENVIRONMENT` değişkeni karar verir.

| Scenario | Senaryo | Where to set it | Nerede ayarlanır |
|---|---|---|---|
| Debug / local | Debug / yerel | `Properties/launchSettings.json` → profile `environmentVariables` | `Properties/launchSettings.json` → profil `environmentVariables` |
| Command line | Komut satırı | `ASPNETCORE_ENVIRONMENT=Beta dotnet run` | `ASPNETCORE_ENVIRONMENT=Beta dotnet run` |
| IIS / Web Deploy | IIS / Web Deploy | `web.config` → `<environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />` | `web.config` → `<environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />` |

**EN:** **Override logic:** `appsettings.json` holds shared settings; `appsettings.{Environment}.json` overrides only the keys that change for that environment.

**TR:** **Override mantığı:** `appsettings.json` ortak ayarları tutar; `appsettings.{Environment}.json` yalnızca o ortam için değişen anahtarları geçersiz kılar.

**EN:** `launchSettings.json` example: | **TR:** `launchSettings.json` örneği:

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

**EN:** In Visual Studio, use the publish profiles (`.pubxml`). Each profile sets its environment via `<EnvironmentName>`; the correct configuration is selected at publish time.

**TR:** Visual Studio'da publish profillerini (`.pubxml`) kullanın. Her profil ortamını `<EnvironmentName>` ile ayarlar; doğru konfigürasyon publish anında seçilir.

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

## 🧪 Sample application | Örnek uygulama

**EN:** [samples/EnvironmentConfiguratorApi](samples/EnvironmentConfiguratorApi) is a **fully working live example** of the package. It consumes the package via `ProjectReference` (a single line in [Program.cs](samples/EnvironmentConfiguratorApi/Program.cs)) and ships with per-environment `appsettings` files, publish profiles and `launchSettings.json`. The `/test/environment-name` endpoint returns the active environment name ([TestController.cs](samples/EnvironmentConfiguratorApi/Controllers/TestController.cs)).

**TR:** [samples/EnvironmentConfiguratorApi](samples/EnvironmentConfiguratorApi), paketin **tam çalışan canlı bir örneğidir**. Paketi `ProjectReference` ile kullanır ([Program.cs](samples/EnvironmentConfiguratorApi/Program.cs) içinde tek satır) ve ortam bazlı `appsettings` dosyaları, publish profilleri ve `launchSettings.json` ile gelir. `/test/environment-name` endpoint'i aktif ortam adını döner ([TestController.cs](samples/EnvironmentConfiguratorApi/Controllers/TestController.cs)).

**EN:** Run it: | **TR:** Çalıştırın:

```bash
cd samples/EnvironmentConfiguratorApi
ASPNETCORE_ENVIRONMENT=Beta dotnet run
# in another terminal | başka bir terminalde:
curl http://localhost:5000/test/environment-name   # -> "Beta"
```

---

## 🏗️ Project structure | Proje yapısı

```
EnvironmentConfigurator.sln
├── src/EnvironmentConfigurator/           → NuGet package (code + scaffold target + templates)
├── samples/EnvironmentConfiguratorApi/    → sample API consuming the package
└── tests/EnvironmentConfigurator.Tests/   → xUnit tests
```

---

## 📦 Building the package locally | Paketi yerelde derleme

```bash
dotnet pack src/EnvironmentConfigurator -c Release -o ./artifacts
# -> artifacts/EnvironmentConfigurator.1.0.3.nupkg
```

---

## 📁 License | Lisans

**EN:** MIT — see [LICENSE.txt](LICENSE.txt).

**TR:** MIT — bkz. [LICENSE.txt](LICENSE.txt).
