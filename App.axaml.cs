using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KanaMelody.Services;
using KanaMelody.ViewModels;
using KanaMelody.Views;
using ManagedBass;
using ManagedBass.Fx;
using ManagedBass.Mix;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KanaMelody;

public partial class App : Application
{
    private IServiceProvider _services = null!;
    
    public override void Initialize()
    {
        var logFileName = $"log-{(int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds}-{Guid.NewGuid()}.txt";
        var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}]: {Message:lj}{NewLine}{Exception}";
        
        // Log to console and file with rotate log file every session, also delete old log files after exceed 20 session
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.File(
                Path.Combine(StorageService.LOG_FULL_PATH, logFileName),
                outputTemplate: outputTemplate,
                retainedFileCountLimit: 20
            );
        
#if DEBUG
        loggerConfig.MinimumLevel.Debug();
#else
        loggerConfig.MinimumLevel.Information();
#endif
        
        Log.Logger = loggerConfig.CreateLogger();
        Log.Information("📝 Logger initialized with file: {LogFileName}", logFileName);
        
        AvaloniaXamlLoader.Load(this);
        
        var collection = new ServiceCollection();
        collection.AddCommonServices();
        
        _services = collection.BuildServiceProvider();
        
        // Invoke LoadConfig
        ConfigService configService = _services.GetRequiredService<ConfigService>();
    }
    
    // TODO: Kanna Logger maybe serilog
    
    private void InitializeAudioManager()
    {
        // TODO: This need to be in seperate class, not only depend on BASS
        
        Bass.DeviceNonStop = true;

        if (!Bass.Init())
        {
            Log.Error("Cannot initialize BASS");
        }
        else
        {
            Log.Information("BASS Initialized");
            Log.Information("BASS Version: {Version}", Bass.Version);
            Log.Information("BASS FX Version: {Version}", BassFx.Version);
            Log.Information("BASS Mix Version: {Version}", BassMix.Version);
        }
    }
    

    public override void OnFrameworkInitializationCompleted()
    {
        InitializeAudioManager();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _services.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
        
        Log.Information("✅ Application initialized");
    }
}