using System;
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
    
    public void SortPlaylist()
    {
        // Sort order : Album -> DiscNumber -> TrackNumber -> Title
        _playlist.Sort((a, b) =>
        {
            if (a.Album != b.Album) return string.Compare(a.Album, b.Album, StringComparison.Ordinal);
            if (a.DiscNumber != b.DiscNumber) return a.DiscNumber.GetValueOrDefault().CompareTo(b.DiscNumber.GetValueOrDefault());
            if (a.TrackNumber != b.TrackNumber) return a.TrackNumber.GetValueOrDefault().CompareTo(b.TrackNumber.GetValueOrDefault());
            return string.Compare(a.Title, b.Title, StringComparison.Ordinal);
        });
    }
    
    public void ClearPlaylist()
    {
        _playlist.Clear();
    }
}