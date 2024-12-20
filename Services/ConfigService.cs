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
        if (!Directory.Exists(StorageService.StarterStorageFolder))
        {
            Log.Information("Creating storage folder at {StarterStorageFolder}", StorageService.StarterStorageFolder);
            Directory.CreateDirectory(StorageService.StarterStorageFolder);
        }
        if (!Directory.Exists(StorageService.SettingsFullPath))
        {
            Log.Information("Creating settings folder at {SettingsFullPath}", StorageService.SettingsFullPath);
            Directory.CreateDirectory(StorageService.SettingsFullPath);
        }
    }
    
    public static StorageSettings LoadStorageSettings()
    {
        try
        {
            string json = File.ReadAllText(StorageService.StorageSettingsFullPath);
            StorageSettings settings = JsonSerializer.Deserialize<StorageSettings>(json) ?? throw new Exception("Failed to deserialize storage settings");
            Log.Information("Loaded storage setting from {StorageSettingsFile}", StorageService.StorageSettingsFile);
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
            File.WriteAllText(StorageService.StorageSettingsFullPath, json);
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