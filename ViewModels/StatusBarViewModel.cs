using System;
using ReactiveUI;
using Serilog;

namespace KanaMelody.ViewModels;

public class StatusBarViewModel : ReactiveObject
{
    private string _statusText = string.Empty;
    
    public string StatusText
    {
        get => _statusText;
        set
        {
            _statusText = value;
            this.RaisePropertyChanged();
        }
    }
}