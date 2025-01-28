using KanaMelody.Models;
using KanaMelody.Services;
using ReactiveUI;

namespace KanaMelody.ViewModels;

public class PlaylistViewModel : ReactiveObject
{
    private readonly NowPlayingService _nowPlayingService;
    private readonly PlaylistService _playlistService;
    private readonly ConfigService _configService;
    private Song? _selectedSong;
    
    public PlaylistViewModel(NowPlayingService nowPlayingService, PlaylistService playlistService, ConfigService configService)
    {
        _nowPlayingService = nowPlayingService;
        _playlistService = playlistService;
        _configService = configService;
    }
    
    public Song[] Playlist => _playlistService.GetPlaylist();
    
    public Song? SelectedSong
    {
        get => _selectedSong;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedSong, value);
            if (_selectedSong != null) _nowPlayingService.PlayMusic(_selectedSong);
        }
    }
}