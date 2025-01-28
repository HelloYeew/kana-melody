using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace KanaMelody.Development;

public static class DebugUtils
{
    public static bool IsDebugBuild => is_debug_build.Value;

    private static readonly Lazy<bool> is_debug_build = new Lazy<bool>(() =>
        IsDebugAssembly(typeof(DebugUtils).Assembly)
    );
    
    // https://stackoverflow.com/a/2186634
    private static bool IsDebugAssembly(Assembly? assembly) => assembly?.GetCustomAttributes(false).OfType<DebuggableAttribute>().Any(da => da.IsJITTrackingEnabled) ?? false;
    
    public static string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0.0.0.0";
}