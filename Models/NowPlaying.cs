using Avalonia.Media.Imaging;

namespace KanaMelody.Models;

public class NowPlaying
{
    public bool IsPlaying { get; set; }
    public int SongStream { get; set; }
    
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
}