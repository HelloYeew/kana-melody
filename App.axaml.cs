using System;
using System.IO;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KanaMelody.Database;
using KanaMelody.Development;
using KanaMelody.Services;
using KanaMelody.ViewModels;
using KanaMelody.Views;
using ManagedBass;
using ManagedBass.Fx;
using ManagedBass.Mix;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KanaMelody;

public class App : Application
{
    private IServiceProvider _services = null!;
    
    private ConfigService _configService = null!;
    private PlaylistViewModel _playlistViewModel = null!;
    private MainWindowViewModel _mainWindowViewModel = null!;
    
    public override void Initialize()
    {
        var logFileName = $"log-{(int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds}-{Guid.NewGuid()}.log";
        var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}]: {Message:lj}{NewLine}{Exception}";
        
        StorageService.CleanOldLogFiles();
        
        // Log to console and file with rotate log file every session, also delete old log files after exceed 20 session
        var loggerConfig = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .WriteTo.File(
                Path.Combine(StorageService.LogFullPath, logFileName),
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

        Log.Information("------------------------------------");
        Log.Information("Log for KanaMelody {Version} (Debug: {IsDebug})", DebugUtils.GetVersion(), DebugUtils.IsDebugBuild);
        Log.Information("Framework : {Framework}", RuntimeInformation.FrameworkDescription);
        Log.Information("Environment: {RuntimeInfo} ({OSVersion}), {ProcessorCount} cores {Architecture}", RuntimeInfo.OS, Environment.OSVersion, Environment.ProcessorCount, RuntimeInformation.OSArchitecture);
        Log.Information("------------------------------------");
        
        var collection = new ServiceCollection();
        
        // Config service
        collection.LoadSingleton<NowPlayingService>();
        collection.LoadSingleton<PlaylistService>();
        collection.LoadSingleton<ConfigService>();
        
        // Component view model
        collection.LoadSingleton<PlaylistViewModel>();
        collection.LoadSingleton<PlaybackControllerViewModel>();
        
        _services = collection.BuildServiceProvider();
        _configService = _services.GetRequiredService<ConfigService>();
        // TODO: Remove this after Kana completely rely on database
        _playlistViewModel = _services.GetRequiredService<PlaylistViewModel>();
        
        // Database
        var databaseContext = new SongDatabaseContext(_configService.StorageSettings);
        collection.AddSingleton(databaseContext);
        collection.LoadSingleton<DatabaseService>();
        
        // App
        collection.AddSingleton(this);
        
        // Windows view model
        collection.LoadSingleton<MainWindowViewModel>();
        
        _services = collection.BuildServiceProvider();
        _mainWindowViewModel = _services.GetRequiredService<MainWindowViewModel>();
        
        Log.Information("✅ All services added");
        
        AvaloniaXamlLoader.Load(this);
    }
    
    public async void ShowFolderSelectionDialog()
    {
        // TODO: Change this API to new WIndow API
        var dialog = new OpenFolderDialog
        {
            Title = "Select Start Folder"
        };

        var mainWindow = (Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        var result = await dialog.ShowAsync(mainWindow);

        if (!string.IsNullOrEmpty(result))
        {
            _configService.FolderSettings.FolderPath = new[]
            {
                result
            };
            _playlistViewModel.ScanAllFolder();
            _configService.SaveConfig();
        }
    }
    
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
            desktop.Exit += OnExit;
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainWindowViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
        
        Log.Information("✅ Application initialized");
        
        if (_configService.FolderSettings.FolderPath.Length == 0)
        {
            ShowFolderSelectionDialog();
        }
    }

    private void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        Log.Information("🚪 Application close signal received");
        _configService.SaveConfig();
        Bass.Free();
        Log.Information("👋 Application exited with code {ExitCode}", e.ApplicationExitCode);
    } 
}