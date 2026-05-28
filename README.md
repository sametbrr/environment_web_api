# EnvironmentConfigurator

[![NuGet](https://img.shields.io/nuget/v/EnvironmentConfigurator.svg)](https://www.nuget.org/packages/EnvironmentConfigurator)
[![Downloads](https://img.shields.io/nuget/dt/EnvironmentConfigurator.svg)](https://www.nuget.org/packages/EnvironmentConfigurator)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE.txt)

🌍 English · **[Türkçe](README.tr.md)**

> Environment-aware configuration for ASP.NET Core (.NET 8+).
> Loads `appsettings.{Environment}.json` with a single call **and**, on first build, auto-scaffolds publish profiles, `web.config` and per-environment `appsettings` files into your project.

---

## 📦 What does this package do?

- ✅ Loads `appsettings.json` → `appsettings.{Environment}.json` → environment variables, in one line.
- ✅ On the first `dotnet build` after install, **auto-copies** missing configuration files into your project (never overwrites existing ones).
- ✅ Integrates into existing projects in seconds via `dotnet add package`.
- ✅ Targets **.NET 8 and later** (incl. net9 / net10).

---

## 🚀 Install

```bash
dotnet add package EnvironmentConfigurator
```

Available on [nuget.org](https://www.nuget.org/packages/EnvironmentConfigurator).

---

## ⚡ Usage

### One line (recommended)

```csharp
using EnvironmentConfigurator;

var builder = WebApplication.CreateBuilder(args);

builder.AddEnvironmentConfiguration();   // appsettings.{Environment}.json + env variables

var app = builder.Build();
app.Run();
```

`AddEnvironmentConfiguration` runs on the .NET 8 `IHostApplicationBuilder`; use it directly with `WebApplicationBuilder`.

### Flexible variant

If you don't have access to the builder, or work at the `IConfigurationBuilder` level:

```csharp
builder.Configuration.AddEnvironmentJsonFiles(builder.Environment);
```

### Options (`EnvironmentConfiguratorOptions`)

```csharp
builder.AddEnvironmentConfiguration(options =>
{
    options.BaseSettingsOptional        = false;  // throw if appsettings.json is missing (default: false)
    options.ReloadOnChange              = true;   // reload on file change (default: true)
    options.IncludeEnvironmentVariables = true;   // add the env variables source (default: true)
    options.BasePath                    = "config"; // if JSON files live in another folder (default: working dir)
});
```

---

## 🛠️ Auto-scaffold (file generation)

After installing the package and building **for the first time**, the following files that are **not already present** in your project are generated automatically:

```
appsettings.Beta.json
appsettings.Production.json
web.config
Properties/PublishProfiles/Beta-FolderProfile.pubxml
Properties/PublishProfiles/Development-FolderProfile.pubxml
Properties/PublishProfiles/Production-FolderProfile.pubxml
```

- **Existing files are never overwritten** — only missing ones are added, your edits are preserved.
- Generated files are logged in the build output: `EnvironmentConfigurator: scaffolded ...`
- The `.pubxml` templates contain no project-specific `ProjectGuid` (not needed for folder publish, avoids conflicts) — `EnvironmentName` is set in each file.

### Disabling scaffold

If you don't want it, add to your `.csproj`:

```xml
<PropertyGroup>
  <EnvironmentConfiguratorScaffold>false</EnvironmentConfiguratorScaffold>
</PropertyGroup>
```

---

## 🌍 Environment selection

The `ASPNETCORE_ENVIRONMENT` variable decides which `appsettings.{X}.json` is loaded.

| Scenario | Where to set it |
|---|---|
| Debug / local | `Properties/launchSettings.json` → profile `environmentVariables` |
| Command line | `ASPNETCORE_ENVIRONMENT=Beta dotnet run` |
| IIS / Web Deploy | `web.config` → `<environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />` |

**Override logic:** `appsettings.json` holds shared settings; `appsettings.{Environment}.json` overrides only the keys that change for that environment.

`launchSettings.json` example:

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

In Visual Studio, use the publish profiles (`.pubxml`). Each profile sets its environment via `<EnvironmentName>`; the correct configuration is selected at publish time.

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

## 🧪 Sample application

[samples/EnvironmentConfiguratorApi](samples/EnvironmentConfiguratorApi) is a **fully working live example** of the package. It consumes the package via `ProjectReference` (a single line in [Program.cs](samples/EnvironmentConfiguratorApi/Program.cs)) and ships with per-environment `appsettings` files, publish profiles and `launchSettings.json`.

The `/test/environment-name` endpoint returns the active environment name ([TestController.cs](samples/EnvironmentConfiguratorApi/Controllers/TestController.cs)).

Run it:

```bash
cd samples/EnvironmentConfiguratorApi
ASPNETCORE_ENVIRONMENT=Beta dotnet run
# in another terminal:
curl http://localhost:5000/test/environment-name   # -> "Beta"
```

---

## 🏗️ Project structure

```
EnvironmentConfigurator.sln
├── src/EnvironmentConfigurator/           → NuGet package (code + scaffold target + templates)
├── samples/EnvironmentConfiguratorApi/    → sample API consuming the package
└── tests/EnvironmentConfigurator.Tests/   → xUnit tests
```

---

## 📦 Building the package locally

```bash
dotnet pack src/EnvironmentConfigurator -c Release -o ./artifacts
# -> artifacts/EnvironmentConfigurator.1.0.3.nupkg
```

---

## 📁 License

MIT — see [LICENSE.txt](LICENSE.txt).
