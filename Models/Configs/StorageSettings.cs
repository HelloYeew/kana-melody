using System.IO;
using KanaMelody.Services;
using Serilog;

namespace KanaMelody.Models.Configs;

public class StorageSettings
{
    public string storageFolder { get; set; }
    
    public string GetLogFolder() => Path.Combine(storageFolder, "logs");
    
    public static StorageSettings Default => new StorageSettings
    {
        storageFolder = Path.Combine(StorageService.StarterStorageFolder)
    };
}