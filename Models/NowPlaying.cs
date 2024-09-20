namespace KanaMelody.Models;

public class NowPlaying
{
    public bool IsPlaying { get; set; }
    public int SongStream { get; set; } = 0;
    
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
}