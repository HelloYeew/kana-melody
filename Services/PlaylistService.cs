using System.Collections.Generic;
using KanaMelody.Models;

namespace KanaMelody.Services;

public class PlaylistService
{
    // TODO: Support multiple playlists
    private List<SongEntry> _playlist = new();
    
    public void AddSong(SongEntry song)
    {
        _playlist.Add(song);
    }
    
    public SongEntry[] GetPlaylist()
    {
        return _playlist.ToArray();
    }
}