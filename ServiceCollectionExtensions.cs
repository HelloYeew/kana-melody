using KanaMelody.Services;
using KanaMelody.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace KanaMelody;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<NowPlayingService>();
        collection.AddSingleton<PlaylistService>();
        collection.AddSingleton<ConfigService>();
        
        collection.AddSingleton<PlaybackControllerViewModel>();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<PlaylistViewModel>();
    }
}