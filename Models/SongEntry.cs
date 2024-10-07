using System.IO;
using ATL;
using Avalonia.Media.Imaging;

namespace KanaMelody.Models;

public class SongEntry
{
    public Bitmap? AlbumCover { get; set; }
    
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
        AlbumCover = trackFile.EmbeddedPictures.Count > 0 ? new Bitmap(new MemoryStream(trackFile.EmbeddedPictures[0].PictureData)) : null;
    }
}