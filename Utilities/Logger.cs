using System;
using System.IO;
using System.Runtime.InteropServices;
using KanaMelody.Development;
using KanaMelody.Services;
using KanaMelody.ViewModels;
using Serilog;

namespace KanaMelody.Utilities;

/// <summary>
/// Helper class to manage the logger.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Initialize the logger with the basic configuration.
    /// </summary>
    public static void InitializeLogger()
    {
        Log.Logger = CreateLoggerConfiguration().CreateLogger();
        LogHeader();
    }
    
    /// <summary>
    /// Create basic logger configuration.
    /// </summary>
    public static LoggerConfiguration CreateLoggerConfiguration()
    {
        var logFileName = $"log-{(int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds}-{Guid.NewGuid()}.log";
        var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}]: {Message:lj}{NewLine}{Exception}";
        
        // Log to console and file with rotate log file every session, also delete old log files after exceed 20 session
        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.File(
                Path.Combine(StorageService.LogFullPath, logFileName),
                outputTemplate: outputTemplate,
                retainedFileCountLimit: 20
            );
        
#if DEBUG
        loggerConfiguration.MinimumLevel.Debug();
#else
        loggerConfiguration.MinimumLevel.Information();
#endif
        
        return loggerConfiguration;
    }

    /// <summary>
    /// Add status bar sink configuration to the old logger.
    /// </summary>
    /// <param name="statusBarViewModel">The status bar view model that will be used to display the log.</param>
    public static void AddStatusBarSinkConfiguration(StatusBarViewModel statusBarViewModel)
    {
        LoggerConfiguration basicConfiguration = CreateLoggerConfiguration();
        Log.Logger = basicConfiguration.WriteTo.Sink(new StatusBarLogSink(statusBarViewModel)).CreateLogger();
    }

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
    
    /// <summary>
    /// Clean old log files.
    /// </summary>
    public static void CleanOldLogFiles()
    {
        StorageService.CleanOldLogFiles();
    }
}