using System.Reflection;

namespace Ezvpn.App;

/// <summary>
/// App-level metadata surfaced in the UI. The version comes from the assembly's
/// informational version (set by &lt;Version&gt; in Ezvpn.App.csproj, overridable
/// from CI); any build-metadata suffix (e.g. "+&lt;commit&gt;" added by SourceLink)
/// is trimmed off.
/// </summary>
public static class AppInfo
{
    /// <summary>The app version formatted for display, e.g. "v0.1.0".</summary>
    public static string Version { get; } = ComputeVersion();

    private static string ComputeVersion()
    {
        var informational = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        var version = informational is { Length: > 0 }
            ? informational.Split('+', 2)[0]
            : Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0";

        return "v" + version;
    }
}
