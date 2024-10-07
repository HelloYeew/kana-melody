using System;
using System.IO;
using System.Text.Json;
using KanaMelody.Models.Configs;

namespace KanaMelody.Services;

public class ConfigService
{
    public StorageSettings StorageSettings { get; set; }

    public void LoadConfig()
    {
        EnsureStorageSettingsDirectory();
        StorageSettings = LoadStorageSettings();
        // TODO: Use logging
        Console.WriteLine("⚙ All settings loaded");
    }
    
    private static void EnsureStorageSettingsDirectory()
    {
        if (!Directory.Exists(StorageService.STARTER_STORAGE_FOLDER))
        {
            Console.WriteLine($"Creating storage folder at {StorageService.STARTER_STORAGE_FOLDER}");
            Directory.CreateDirectory(StorageService.STARTER_STORAGE_FOLDER);
        }
        if (!Directory.Exists(StorageService.SETTINGS_FULL_PATH))
        {
            Console.WriteLine($"Creating settings folder at {StorageService.SETTINGS_FULL_PATH}");
            Directory.CreateDirectory(StorageService.SETTINGS_FULL_PATH);
        }
    }
    
    public static StorageSettings LoadStorageSettings()
    {
        try
        {
            string json = File.ReadAllText(StorageService.STORAGE_SETTINGS_FULL_PATH);
            StorageSettings settings = JsonSerializer.Deserialize<StorageSettings>(json) ?? throw new Exception("Failed to deserialize storage settings");
            Console.WriteLine($"Loaded storage setting from {StorageService.STORAGE_SETTINGS_FILE}");
            return settings;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to load storage settings: {e}, creating new settings");
            StorageSettings newSettings = StorageSettings.Default;
            SaveStorageSettings(newSettings);
            return newSettings;
        }
    }
    
    public static void SaveStorageSettings(StorageSettings settings)
    {
        try
        {
            string json = JsonSerializer.Serialize(settings);
            // Create a new file, or overwrite an existing file.
            File.WriteAllText(StorageService.STORAGE_SETTINGS_FULL_PATH, json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public ConfigService()
    {
        LoadConfig();
    }
}