namespace KanaMelody.Models;

public class Song
{
    public int Id { get; set; }
    public string Path { get; set; }
    
    public SongMetadata? Metadata { get; set; }
}