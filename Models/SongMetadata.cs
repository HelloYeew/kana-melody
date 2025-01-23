using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KanaMelody.Models;

public class SongMetadata
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? Album { get; set; }
    
    /// <summary>
    /// Order of the song in the album
    /// </summary>
    /// <returns></returns>
    public int? TrackNumber { get; set; } = 0;
    
    public int SongId { get; set; }
    public Song Song { get; set; } = null!;
}