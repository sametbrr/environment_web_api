using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EnvironmentConfigurator;

/// <summary>
/// Extension methods that load environment-aware JSON configuration:
/// <c>appsettings.json</c> followed by <c>appsettings.{EnvironmentName}.json</c>
/// and (optionally) environment variables.
/// </summary>
public static class EnvironmentConfiguratorExtensions
{
    /// <summary>
    /// Adds <c>appsettings.json</c>, the environment-specific
    /// <c>appsettings.{environment.EnvironmentName}.json</c> override and
    /// (optionally) environment variables to the configuration builder.
    /// </summary>
    public static IConfigurationBuilder AddEnvironmentJsonFiles(
        this IConfigurationBuilder builder,
        IHostEnvironment environment,
        Action<EnvironmentConfiguratorOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(environment);

        var options = new EnvironmentConfiguratorOptions();
        configure?.Invoke(options);

        if (!string.IsNullOrWhiteSpace(options.BasePath))
            builder.SetBasePath(options.BasePath);

        builder
            .AddJsonFile("appsettings.json", optional: options.BaseSettingsOptional, reloadOnChange: options.ReloadOnChange)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: options.ReloadOnChange);

        if (options.IncludeEnvironmentVariables)
            builder.AddEnvironmentVariables();

        return builder;
    }

    /// <summary>
    /// Convenience one-liner for hosts (e.g. <c>WebApplicationBuilder</c>) that wires
    /// environment-aware configuration using the builder's own environment.
    /// </summary>
    public static TBuilder AddEnvironmentConfiguration<TBuilder>(
        this TBuilder builder,
        Action<EnvironmentConfiguratorOptions>? configure = null)
        where TBuilder : IHostApplicationBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Configuration.AddEnvironmentJsonFiles(builder.Environment, configure);
        return builder;
    }
}
