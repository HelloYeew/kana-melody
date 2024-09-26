using KanaMelody.Models;
using KanaMelody.Services;
using ReactiveUI;

namespace KanaMelody.ViewModels;

public class PlaylistViewModel : ReactiveObject
{
    private readonly NowPlayingService _nowPlayingService;
    private readonly PlaylistService _playlistService;
    private SongEntry? _selectedSong;
    
    public PlaylistViewModel(NowPlayingService nowPlayingService, PlaylistService playlistService)
    {
        _nowPlayingService = nowPlayingService;
        _playlistService = playlistService;
        foreach (var path in SongEntryServices.GetAllSongs(SongEntryServices.Path))
        {
            _playlistService.AddSong(new SongEntry(path));
        }
        this.RaisePropertyChanged(nameof(Playlist));
    }
    
    public SongEntry[] Playlist => _playlistService.GetPlaylist();
    
    public SongEntry SelectedSong
    {
        get => _selectedSong;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedSong, value);
            _nowPlayingService.PlayMusic(_selectedSong.Path);
        }
    }
}