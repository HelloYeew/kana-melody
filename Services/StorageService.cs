using System;
using System.IO;

namespace KanaMelody.Services;

public class StorageService
{
    // TODO: This should be configurable, save starter in settings
    public static string StarterStorageFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KanaMelody");
    
    // Storage settings cannot be changed 
    public static string SettingsFolder => "settings";
    public static string SettingsFullPath => Path.Combine(StarterStorageFolder, SettingsFolder);
    public static string StorageSettingsFile => "settings/storage.json";
    public static string PlayerSettingsFile => "settings/player.json";
    public static string StorageSettingsFullPath => Path.Combine(StarterStorageFolder, StorageSettingsFile);
    public static string PlayerSettingsFullPath => Path.Combine(StarterStorageFolder, PlayerSettingsFile);
    
    // Log settings cannot be changed
    public static string LogFolder => "logs";
    public static string LogFullPath => Path.Combine(StarterStorageFolder, LogFolder);
    
    private const int MaxLogFiles = 20;
    
    /// <summary>
    /// Clean up old log files if exceed the limit
    /// </summary>
    public static void CleanOldLogFiles()
    {
        if (!Directory.Exists(LogFullPath))
        {
            Directory.CreateDirectory(LogFullPath);
        }
        
        string[] logFiles = Directory.GetFiles(LogFullPath);
        
        if (logFiles.Length > MaxLogFiles)
        {
            Array.Sort(logFiles);
            for (int i = 0; i < logFiles.Length - MaxLogFiles; i++)
            {
                File.Delete(logFiles[i]);
            }
        }
    }
}