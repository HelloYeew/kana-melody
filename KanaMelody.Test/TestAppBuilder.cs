using Avalonia;
using Avalonia.Headless;

namespace KanaMelody.Test;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<KanaMelodyApp>()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}