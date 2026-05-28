namespace EnvironmentConfigurator;

/// <summary>
/// Controls how environment-aware JSON configuration files are loaded.
/// </summary>
public sealed class EnvironmentConfiguratorOptions
{
    /// <summary>When true, a missing base <c>appsettings.json</c> does not throw. Default: false.</summary>
    public bool BaseSettingsOptional { get; set; } = false;

    /// <summary>Reload configuration when a file changes on disk. Default: true.</summary>
    public bool ReloadOnChange { get; set; } = true;

    /// <summary>Also add environment variables as a configuration source. Default: true.</summary>
    public bool IncludeEnvironmentVariables { get; set; } = true;

    /// <summary>Optional base path for the JSON files. When null, the current directory is used.</summary>
    public string? BasePath { get; set; }
}
