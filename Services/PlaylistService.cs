using System.Collections.Generic;
using KanaMelody.Models;

namespace KanaMelody.Services;

public class PlaylistService
{
    // TODO: Support multiple playlists
    private List<Song> _playlist = new();
    public int CurrentIndex { get; set; } = -1;
    
    public void AddSong(Song song)
    {
        _playlist.Add(song);
    }
    
    public void SetPlaylist(Song[] playlist)
    {
        _playlist = new List<Song>(playlist);
    }
    
    public Song[] GetPlaylist()
    {
        return _playlist.ToArray();
    }
    
    public void ClearPlaylist()
    {
        _playlist.Clear();
    }
}