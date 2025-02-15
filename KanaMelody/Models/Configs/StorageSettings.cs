using System.ComponentModel;
using System.IO;
using KanaMelody.Services;

namespace KanaMelody.Models.Configs;

/// <summary>
/// Main storage settings for the application. This setting should not be moved with other settings for startup logic.
/// </summary>
public class StorageSettings : INotifyPropertyChanged
{
    private string? _storageFolder;
    public string? StorageFolder
    {
        get => _storageFolder;
        set
        {
            if (_storageFolder != value)
            {
                _storageFolder = value;
                OnPropertyChanged(nameof(StorageFolder));
            }
        }
    }

    public string GetLogFolder() => Path.Combine(_storageFolder, "logs");
    public string GetDatabaseLocation() => Path.Combine(_storageFolder, "song.db");

    public static StorageSettings Default => new StorageSettings
    {
        StorageFolder = Path.Combine(StorageService.StarterStorageFolder)
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}