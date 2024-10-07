using System.IO;
using KanaMelody.Services;

namespace KanaMelody.Models.Configs;

public class StorageSettings
{
    public string storageFolder { get; set; }
    
    public static StorageSettings Default => new StorageSettings
    {
        storageFolder = Path.Combine(StorageService.STARTER_STORAGE_FOLDER)
    };
}