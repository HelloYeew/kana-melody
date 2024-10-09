using System;
using System.IO;

namespace KanaMelody.Services;

public class StorageService
{
    // TODO: This should be configurable, save starter in settings
    public static string STARTER_STORAGE_FOLDER => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KanaMelody");
    
    // Storage settings cannot be changed 
    public static string SETTINGS_FOLDER => "settings";
    public static string SETTINGS_FULL_PATH => Path.Combine(STARTER_STORAGE_FOLDER, SETTINGS_FOLDER);
    public static string STORAGE_SETTINGS_FILE => "settings/storage.json";
    public static string STORAGE_SETTINGS_FULL_PATH => Path.Combine(STARTER_STORAGE_FOLDER, STORAGE_SETTINGS_FILE);
    
    // Log settings cannot be changed
    public static string LOG_FOLDER => "logs";
    public static string LOG_FULL_PATH => Path.Combine(STARTER_STORAGE_FOLDER, LOG_FOLDER);
}