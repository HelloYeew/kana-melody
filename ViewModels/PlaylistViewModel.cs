using KanaMelody.Models;
using KanaMelody.Services;
using ReactiveUI;

namespace KanaMelody.ViewModels;

public class PlaylistViewModel : ReactiveObject
{
    private readonly NowPlayingService _nowPlayingService;
    private readonly PlaylistService _playlistService;
    private readonly ConfigService _configService;
    private SongEntry? _selectedSong;
    
    public PlaylistViewModel(NowPlayingService nowPlayingService, PlaylistService playlistService, ConfigService configService)
    {
        _nowPlayingService = nowPlayingService;
        _playlistService = playlistService;
        _configService = configService;
        ScanAllFolder();
    }
    
    public SongEntry[] Playlist => _playlistService.GetPlaylist();
    
    public SongEntry? SelectedSong
    {
        get => _selectedSong;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedSong, value);
            if (_selectedSong != null) _nowPlayingService.PlayMusic(_selectedSong);
        }
    }

    /// <summary>
    /// Scan all folders in the folder settings and add all songs to the playlist.
    /// </summary>
    public void ScanAllFolder()
    {
        _playlistService.ClearPlaylist();
        foreach (var folderPath in _configService.FolderSettings.FolderPath)
        {
            foreach (var path in SongEntryServices.GetAllSongs(folderPath))
            {
                _playlistService.AddSong(new SongEntry(path));
            }
        }
        this.RaisePropertyChanged(nameof(Playlist));
    }
}