using System.ComponentModel;
using System.IO;
using KanaMelody.Services;

namespace KanaMelody.Models.Configs;

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