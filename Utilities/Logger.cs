using System;
using System.Runtime.InteropServices;
using KanaMelody.Development;
using Serilog;

namespace KanaMelody.Utilities;

/// <summary>
/// Helper class to manage the logger.
/// </summary>
public static class Logger
{

    /// <summary>
    /// Write the initial log.
    /// </summary>
    public static void LogHeader()
    {
        Log.Information("------------------------------------");
        Log.Information("Log for KanaMelody {Version} (Debug: {IsDebug})", DebugUtils.GetVersion(), DebugUtils.IsDebugBuild);
        Log.Information("Framework : {Framework}", RuntimeInformation.FrameworkDescription);
        Log.Information("Environment: {RuntimeInfo} ({OSVersion}), {ProcessorCount} cores {Architecture}", RuntimeInfo.OS, Environment.OSVersion, Environment.ProcessorCount, RuntimeInformation.OSArchitecture);
        Log.Information("------------------------------------");
    }
}