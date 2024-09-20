using KanaMelody.Services;
using ReactiveUI;

namespace KanaMelody.ViewModels;

public class PlaybackControllerViewModel : ReactiveObject
{
    private readonly NowPlayingService _nowPlayingService;
    
    public PlaybackControllerViewModel(NowPlayingService nowPlayingService)
    {
        _nowPlayingService = nowPlayingService;
    }
    
    public bool IsPlaying => _nowPlayingService.IsPlaying;
    
    public string PlayingTitle => $"{_nowPlayingService.Title} - {_nowPlayingService.Artist} ({_nowPlayingService.Album})";
    
    public void PlayCommand()
    {
        _nowPlayingService.Play();
        this.RaisePropertyChanged(nameof(IsPlaying));
        this.RaisePropertyChanged(nameof(PlayingTitle));
    }
    
    public void PauseCommand()
    {
        _nowPlayingService.Pause();
    }
}