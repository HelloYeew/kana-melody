using Avalonia;
using Avalonia.ReactiveUI;
using System;
using KanaMelody.Utilities;
using Serilog;
using Velopack;

namespace KanaMelody;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // To receive unhandled exceptions from all threads in the application to the logger
        // the logger must be initialized before everything else
        Logger.CleanOldLogFiles();
        Logger.InitializeLogger();
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
        
        VelopackApp.Build().Run();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();

    private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        Log.Error(e.ExceptionObject as Exception, "Unhandled exception");
    }
}