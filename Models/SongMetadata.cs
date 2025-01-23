namespace KanaMelody.Models;

public class SongMetadata
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    
    public int SongId { get; set; }
    public Song Song { get; set; } = null!;
}