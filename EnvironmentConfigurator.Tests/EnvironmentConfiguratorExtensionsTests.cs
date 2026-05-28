using EnvironmentConfigurator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace EnvironmentConfigurator.Tests;

public sealed class EnvironmentConfiguratorExtensionsTests : IDisposable
{
    private readonly string _dir;

    public EnvironmentConfiguratorExtensionsTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), "envcfg-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
    }

    [Fact]
    public void Loads_base_and_environment_specific_files()
    {
        WriteFile("appsettings.json", """{ "Key": "base", "OnlyBase": "x" }""");
        WriteFile("appsettings.Beta.json", """{ "Key": "beta" }""");

        var config = BuildConfig("Beta");

        Assert.Equal("beta", config["Key"]);   // environment override wins
        Assert.Equal("x", config["OnlyBase"]); // base value preserved
    }

    [Fact]
    public void Missing_environment_file_falls_back_to_base()
    {
        WriteFile("appsettings.json", """{ "Key": "base" }""");

        var config = BuildConfig("Production"); // no appsettings.Production.json present

        Assert.Equal("base", config["Key"]);
    }

    [Fact]
    public void EnvironmentVariables_can_be_excluded()
    {
        WriteFile("appsettings.json", """{ "Key": "base" }""");
        var varName = "ENVCFG_TEST_" + Guid.NewGuid().ToString("N");
        System.Environment.SetEnvironmentVariable(varName, "fromEnv");
        try
        {
            var config = BuildConfig("Beta", o => o.IncludeEnvironmentVariables = false);
            Assert.Null(config[varName]);
        }
        finally
        {
            System.Environment.SetEnvironmentVariable(varName, null);
        }
    }

    private IConfigurationRoot BuildConfig(string environmentName, Action<EnvironmentConfiguratorOptions>? configure = null)
    {
        var env = new FakeHostEnvironment(environmentName);
        return new ConfigurationBuilder()
            .AddEnvironmentJsonFiles(env, o =>
            {
                o.BasePath = _dir;
                o.ReloadOnChange = false;
                configure?.Invoke(o);
            })
            .Build();
    }

    private void WriteFile(string name, string content) =>
        File.WriteAllText(Path.Combine(_dir, name), content);

    public void Dispose()
    {
        try { Directory.Delete(_dir, recursive: true); } catch { /* best effort */ }
    }

    private sealed class FakeHostEnvironment : IHostEnvironment
    {
        public FakeHostEnvironment(string environmentName) => EnvironmentName = environmentName;
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; } = "Tests";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
