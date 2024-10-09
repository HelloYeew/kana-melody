using System;
using System.IO;
using System.Text.Json;
using KanaMelody.Models.Configs;
using Serilog;
using Serilog.Core;

namespace KanaMelody.Services;

public class ConfigService
{
    public StorageSettings StorageSettings { get; set; }

    public void LoadConfig()
    {
        EnsureStorageSettingsDirectory();
        StorageSettings = LoadStorageSettings();
        // TODO: Use logging
        Log.Information("⚙ All settings loaded");
    }
    
    private static void EnsureStorageSettingsDirectory()
    {
        if (!Directory.Exists(StorageService.STARTER_STORAGE_FOLDER))
        {
            Log.Information("Creating storage folder at {StarterStorageFolder}", StorageService.STARTER_STORAGE_FOLDER);
            Directory.CreateDirectory(StorageService.STARTER_STORAGE_FOLDER);
        }
        if (!Directory.Exists(StorageService.SETTINGS_FULL_PATH))
        {
            Log.Information("Creating settings folder at {SettingsFullPath}", StorageService.SETTINGS_FULL_PATH);
            Directory.CreateDirectory(StorageService.SETTINGS_FULL_PATH);
        }
    }
    
    public static StorageSettings LoadStorageSettings()
    {
        try
        {
            string json = File.ReadAllText(StorageService.STORAGE_SETTINGS_FULL_PATH);
            StorageSettings settings = JsonSerializer.Deserialize<StorageSettings>(json) ?? throw new Exception("Failed to deserialize storage settings");
            Log.Information("Loaded storage setting from {StorageSettingsFile}", StorageService.STORAGE_SETTINGS_FILE);
            Log.Information("Storage folder: {StorageFolder}", settings.storageFolder);
            return settings;
        }
        catch (Exception e)
        {
            Log.Warning("Failed to load storage settings: {E}, creating new settings", e);
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
            Log.Error("Failed to save storage settings: {E}", e);
        }
    }

    public ConfigService()
    {
        LoadConfig();
    }
}