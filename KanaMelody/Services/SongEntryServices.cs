using System.Collections.Generic;

namespace KanaMelody.Services;

public class SongEntryServices
{
    private static string[] _extensions = { ".mp3", ".wav", ".flac", ".ogg" , ".m4a" };

    public static string[] GetAllSongs(string path)
    {
        List<string> songs = new();
        
        foreach (string extension in _extensions)
        {
            songs.AddRange(System.IO.Directory.GetFiles(path, "*" + extension, System.IO.SearchOption.AllDirectories));
        }
        
        return songs.ToArray();
    }
}