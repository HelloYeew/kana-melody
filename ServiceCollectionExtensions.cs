using KanaMelody.Services;
using KanaMelody.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KanaMelody;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.LoadSingleton<NowPlayingService>();
        collection.LoadSingleton<PlaylistService>();
        collection.LoadSingleton<ConfigService>();
        
        collection.LoadSingleton<PlaybackControllerViewModel>();
        collection.LoadSingleton<MainWindowViewModel>();
        collection.LoadSingleton<PlaylistViewModel>();
        
        Log.Information("✅ All services added");
    }
    
    public static void LoadSingleton<TService>(this IServiceCollection services) where TService : class
    {
        Log.Information("Adding singleton service: {ServiceType}", typeof(TService).Name);
        services.AddSingleton<TService>();
    }
}