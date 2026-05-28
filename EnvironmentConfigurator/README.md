# EnvironmentConfigurator

[![NuGet](https://img.shields.io/nuget/v/EnvironmentConfigurator.svg)](https://www.nuget.org/packages/EnvironmentConfigurator)
[![Downloads](https://img.shields.io/nuget/dt/EnvironmentConfigurator.svg)](https://www.nuget.org/packages/EnvironmentConfigurator)

**EN:** Environment-aware configuration for ASP.NET Core (.NET 8+).

**TR:** ASP.NET Core (.NET 8+) için ortam duyarlı konfigürasyon.

**EN:**
- Loads `appsettings.json` → `appsettings.{Environment}.json` → environment variables with a single call.
- On first build, auto-scaffolds publish profiles, `web.config` and per-environment `appsettings` into your project (existing files are never overwritten).

**TR:**
- Tek bir çağrı ile `appsettings.json` → `appsettings.{Environment}.json` → ortam değişkenlerini yükler.
- İlk derlemede publish profilleri, `web.config` ve ortam bazlı `appsettings` dosyalarını projenize otomatik oluşturur (var olan dosyalar asla üzerine yazılmaz).

## Install | Kurulum

```bash
dotnet add package EnvironmentConfigurator
```

## Use | Kullanım

```csharp
using EnvironmentConfigurator;

var builder = WebApplication.CreateBuilder(args);
builder.AddEnvironmentConfiguration();   // one line | tek satır
```

**EN:** Pick the active environment via `ASPNETCORE_ENVIRONMENT` (launchSettings profile, command line, or `web.config`). Disable auto-scaffolding with `<EnvironmentConfiguratorScaffold>false</EnvironmentConfiguratorScaffold>`.

**TR:** Aktif ortamı `ASPNETCORE_ENVIRONMENT` ile seçin (launchSettings profili, komut satırı veya `web.config`). Otomatik oluşturmayı `<EnvironmentConfiguratorScaffold>false</EnvironmentConfiguratorScaffold>` ile kapatın.

**EN:** See the project repository for full documentation and a working sample API.

**TR:** Tam dokümantasyon ve çalışan örnek API için proje deposuna bakın.
