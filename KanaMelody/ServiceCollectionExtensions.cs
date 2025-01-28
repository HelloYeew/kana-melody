using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace KanaMelody;

public static class ServiceCollectionExtensions
{
    public static void LoadSingleton<TService>(this IServiceCollection services) where TService : class
    {
        Log.Information("Adding singleton service: {ServiceType}", typeof(TService).Name);
        services.AddSingleton<TService>();
    }
}