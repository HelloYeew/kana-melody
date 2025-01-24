using ATL;

namespace KanaMelody.Models;

public class SongEntry
{
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public int? TrackNumber { get; set; }
    public int? DiscNumber { get; set; }
    
    public string Path { get; set; }

    public SongEntry(string path)
    {
        Path = path;
        var trackFile = new Track(path);
        Title = trackFile.Title;
        Artist = trackFile.Artist;
        Album = trackFile.Album;
        TrackNumber = trackFile.TrackNumber ?? 0;
        DiscNumber = trackFile.DiscNumber ?? 0;
    }
}