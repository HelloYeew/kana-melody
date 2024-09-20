using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KanaMelody.ViewModels;
using KanaMelody.Views;
using ManagedBass;
using ManagedBass.Fx;
using ManagedBass.Mix;
using Microsoft.Extensions.DependencyInjection;

namespace KanaMelody;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void InitializeAudioManager()
    {
        // TODO: This need to be in seperate class, not only depend on BASS
        
        Bass.DeviceNonStop = true;

        if (!Bass.Init())
        {
            Console.WriteLine("Cannot initialize BASS");
        }
        else
        {
            Console.WriteLine("BASS Initialized.");
            Console.WriteLine($"BASS Version: {Bass.Version}");
            Console.WriteLine($"BASS FX Version: {BassFx.Version}");
            Console.WriteLine($"BASS Mix Version: {BassMix.Version}");
        }
    }
    

    public override void OnFrameworkInitializationCompleted()
    {
        InitializeAudioManager();
        
        var collection = new ServiceCollection();
        collection.AddCommonServices();
        
        var services = collection.BuildServiceProvider();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = services.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}