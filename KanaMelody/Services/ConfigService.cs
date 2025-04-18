using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using KanaMelody.Models.Configs;
using Serilog;

namespace KanaMelody.Services;

public class ConfigService : INotifyPropertyChanged
{
    private StorageSettings _storageSettings;
    public StorageSettings StorageSettings
    {
        get => _storageSettings;
        set
        {
            if (_storageSettings != null)
            {
                _storageSettings.PropertyChanged -= OnStorageSettingsChanged;
            }
            _storageSettings = value;
            _storageSettings.PropertyChanged += OnStorageSettingsChanged;
            OnPropertyChanged(nameof(StorageSettings));
            SaveStorageSettings(_storageSettings);
        }
    }

    private PlayerSettings _playerSettings;
    public PlayerSettings PlayerSettings
    {
        get => _playerSettings;
        set
        {
            if (_playerSettings != null)
            {
                _playerSettings.PropertyChanged -= OnPlayerSettingsChanged;
            }
            _playerSettings = value;
            _playerSettings.PropertyChanged += OnPlayerSettingsChanged;
            OnPropertyChanged(nameof(PlayerSettings));
            SavePlayerSettings(_playerSettings);
        }
    }
    
    private FolderSettings _folderSettings;
    
    public FolderSettings FolderSettings
    {
        get => _folderSettings;
        set
        {
            if (_folderSettings != null)
            {
                _folderSettings.PropertyChanged -= OnFolderSettingsChanged;
            }
            _folderSettings = value;
            _folderSettings.PropertyChanged += OnFolderSettingsChanged;
            OnPropertyChanged(nameof(FolderSettings));
            SaveFolderSettings(_folderSettings);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnStorageSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        SaveStorageSettings(StorageSettings);
    }

    private void OnPlayerSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        SavePlayerSettings(PlayerSettings);
    }
    
    private void OnFolderSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        SaveFolderSettings(FolderSettings);
    }

    public void LoadConfig()
    {
        EnsureStorageSettingsDirectory();
        StorageSettings = LoadStorageSettings();
        PlayerSettings = LoadPlayerSettings();
        FolderSettings = LoadFolderSettings();
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
            Log.Information("Storage folder: {StorageFolder}", settings.StorageFolder);
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
    
    public static PlayerSettings LoadPlayerSettings()
    {
        try
        {
            string json = File.ReadAllText(StorageService.PlayerSettingsFullPath);
            PlayerSettings settings = JsonSerializer.Deserialize<PlayerSettings>(json) ?? throw new Exception("Failed to deserialize player settings");
            Log.Information("Loaded player setting from {PlayerSettingsFile}", StorageService.PlayerSettingsFile);
            return settings;
        }
        catch (Exception e)
        {
            Log.Warning("Failed to load player settings: {E}, creating new settings", e);
            PlayerSettings newSettings = PlayerSettings.Default;
            SavePlayerSettings(newSettings);
            return newSettings;
        }
    }
    
    public static FolderSettings LoadFolderSettings()
    {
        try
        {
            string json = File.ReadAllText(StorageService.FolderSettingsFullPath);
            FolderSettings settings = JsonSerializer.Deserialize<FolderSettings>(json) ?? throw new Exception("Failed to deserialize folder settings");
            Log.Information("Loaded folder setting from {FolderSettingsFile}", StorageService.FolderSettingsFile);
            return settings;
        }
        catch (Exception e)
        {
            Log.Warning("Failed to load folder settings: {E}, creating new settings", e);
            FolderSettings newSettings = FolderSettings.Default;
            SaveFolderSettings(newSettings);
            return newSettings;
        }
    }
    
    public static void SaveStorageSettings(StorageSettings settings)
    {
        try
        {
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(StorageService.StorageSettingsFullPath, json);
            Log.Debug("Saved storage settings to {StorageSettingsFile}", StorageService.StorageSettingsFile);
        }
        catch (Exception e)
        {
            Log.Error("Failed to save storage settings: {E}", e);
        }
    }
    
    public static void SavePlayerSettings(PlayerSettings settings)
    {
        try
        {
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(StorageService.PlayerSettingsFullPath, json);
            Log.Debug("Saved player settings to {PlayerSettingsFile}", StorageService.PlayerSettingsFile);
        }
        catch (Exception e)
        {
            Log.Error("Failed to save storage settings: {E}", e);
        }
    }
    
    public static void SaveFolderSettings(FolderSettings settings)
    {
        try
        {
            string json = JsonSerializer.Serialize(settings);
            File.WriteAllText(StorageService.FolderSettingsFullPath, json);
            Log.Debug("Saved folder settings to {FolderSettingsFile}", StorageService.FolderSettingsFile);
        }
        catch (Exception e)
        {
            Log.Error("Failed to save folder settings: {E}", e);
        }
    }
    
    /// <summary>
    /// Save all settings
    /// </summary>
    public void SaveConfig()
    {
        SaveStorageSettings(StorageSettings);
        SavePlayerSettings(PlayerSettings);
        SaveFolderSettings(FolderSettings);
    }

    public ConfigService()
    {
        LoadConfig();
    }
}