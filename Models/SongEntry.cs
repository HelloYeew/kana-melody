using System.IO;
using ATL;
using Avalonia.Media.Imaging;

namespace KanaMelody.Models;

public class SongEntry
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    
    public string Path { get; set; }

    public SongEntry(string path)
    {
        Path = path;
        var trackFile = new Track(path);
        Title = trackFile.Title;
        Artist = trackFile.Artist;
        Album = trackFile.Album;
    }
}