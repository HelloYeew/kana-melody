using System;
using System.ComponentModel;
using Serilog;

namespace KanaMelody.Models.Configs;

/// <summary>
/// Settings for the folder path for track search.
/// </summary>
public class FolderSettings : INotifyPropertyChanged
{
    public static readonly string[] DefaultFolderPath = Array.Empty<string>();

    private string[] _folderPath = DefaultFolderPath;
    
    public string[] FolderPath
    {
        get => _folderPath;
        set
        {
            _folderPath = value;
            Log.Information("📁 Folder path changed to {FolderPath}", _folderPath);
            OnPropertyChanged(nameof(FolderPath));
        }
    }

    public static FolderSettings Default => new FolderSettings
    {
        FolderPath = DefaultFolderPath
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}