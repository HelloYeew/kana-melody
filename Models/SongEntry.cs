using System.IO;
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
        var tfile = TagLib.File.Create(path);
        Title = tfile.Tag.Title ?? "";
        Artist = tfile.Tag.FirstPerformer ?? "";
        Album = tfile.Tag.Album ?? "";
        if (tfile.Tag.Pictures.Length > 0)
        {
            var picture = tfile.Tag.Pictures[0];
            using (var stream = new MemoryStream(picture.Data.Data))
            {
                AlbumCover = new Bitmap(stream);
            }
        }
        else
        {
            AlbumCover = null;
        }
    }
}