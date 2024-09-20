using KanaMelody.Models;
using KanaMelody.Services;
using KanaMelody.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace KanaMelody;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<NowPlaying>();
        collection.AddSingleton<NowPlayingService>();
        
        collection.AddSingleton<PlaybackControllerViewModel>();
        collection.AddSingleton<MainWindowViewModel>();
    }
}