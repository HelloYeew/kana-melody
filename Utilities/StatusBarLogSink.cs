using KanaMelody.ViewModels;
using Serilog.Core;
using Serilog.Events;

namespace KanaMelody.Utilities;

/// <summary>
/// Serilog sink that receive log messages and display them in the status bar.
/// </summary>
public class StatusBarLogSink : ILogEventSink
{
    private readonly StatusBarViewModel _statusBarViewModel;

    public StatusBarLogSink(StatusBarViewModel statusBarViewModel)
    {
        _statusBarViewModel = statusBarViewModel;
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level < LogEventLevel.Information)
        {
            return;
        }
        _statusBarViewModel.StatusText = logEvent.RenderMessage();
    }
}