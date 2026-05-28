# EnvironmentConfigurator

[![NuGet](https://img.shields.io/nuget/v/EnvironmentConfigurator.svg)](https://www.nuget.org/packages/EnvironmentConfigurator)
[![Downloads](https://img.shields.io/nuget/dt/EnvironmentConfigurator.svg)](https://www.nuget.org/packages/EnvironmentConfigurator)

Environment-aware configuration for ASP.NET Core (.NET 8+).

- Loads `appsettings.json` → `appsettings.{Environment}.json` → environment variables with a single call.
- On first build, auto-scaffolds publish profiles, `web.config` and per-environment `appsettings` into your project (existing files are never overwritten).

## Install

```bash
dotnet add package EnvironmentConfigurator
```

## Use

```csharp
using EnvironmentConfigurator;

var builder = WebApplication.CreateBuilder(args);
builder.AddEnvironmentConfiguration();   // one line
```

Pick the active environment via `ASPNETCORE_ENVIRONMENT` (launchSettings profile, command line, or `web.config`).

Disable auto-scaffolding with `<EnvironmentConfiguratorScaffold>false</EnvironmentConfiguratorScaffold>`.

See the project repository for full documentation and a working sample API.
