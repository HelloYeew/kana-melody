using System;
using System.Collections.Generic;
using System.Linq;
using KanaMelody.Database;
using KanaMelody.Models;
using KanaMelody.Services;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace KanaMelody.ViewModels;

public class TrackListViewModel : ReactiveObject
{
    private readonly SongDatabaseContext _songDatabaseContext;
    private readonly PlaylistService _playlistService;
    private readonly NowPlayingService _nowPlayingService;
    private readonly PlaylistViewModel _playlistViewModel;
    private Song? _selectedSong;
    
    private Song[] trackList = Array.Empty<Song>();
    
    public TrackListViewModel(SongDatabaseContext songDatabaseContext, PlaylistService playlistService, NowPlayingService nowPlayingService, PlaylistViewModel playlistViewModel)
    {
        _songDatabaseContext = songDatabaseContext;
        _playlistService = playlistService;
        _nowPlayingService = nowPlayingService;
        _playlistViewModel = playlistViewModel;
        UpdateTrackList();
    }
    
    // Get all song from the database
    public Song[] TrackList
    {
        get => trackList;
        set => this.RaiseAndSetIfChanged(ref trackList, value);
    }
    
    public Song? SelectedSong
    {
        get => _selectedSong;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedSong, value);
            if (_selectedSong != null)
            {
                _playlistService.ClearPlaylist();
                _playlistService.SetPlaylist(GetTrackList());
                _playlistService.CurrentIndex = Array.IndexOf(_playlistService.GetPlaylist(), _selectedSong);
                _nowPlayingService.PlayMusic(_selectedSong);
                _playlistViewModel.SelectedSong = _selectedSong;
                _playlistViewModel.RaisePropertyChanged(nameof(PlaylistViewModel.Playlist));
            }
        }
    }
    
    private Song[] GetTrackList()
    {
        List<Song> list = _songDatabaseContext.Songs.Include(song => song.Metadata).ToList();
        list.Sort((a, b) =>
        {
            if (a.Metadata == null || b.Metadata == null) return 0;
            if (a.Metadata.Album != b.Metadata.Album) return string.Compare(a.Metadata.Album, b.Metadata.Album, StringComparison.Ordinal);
            if (a.Metadata.DiscNumber != b.Metadata.DiscNumber) return a.Metadata.DiscNumber.GetValueOrDefault().CompareTo(b.Metadata.DiscNumber.GetValueOrDefault());
            return a.Metadata.TrackNumber != b.Metadata.TrackNumber ? a.Metadata.TrackNumber.GetValueOrDefault().CompareTo(b.Metadata.TrackNumber.GetValueOrDefault()) : string.Compare(a.Metadata.Title, b.Metadata.Title, StringComparison.Ordinal);
        });
        return list.ToArray();
    }
    
    public void UpdateTrackList()
    {
        TrackList = GetTrackList();
    }
}