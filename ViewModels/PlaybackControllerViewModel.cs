using System;
using System.Reactive.Linq;
using KanaMelody.Services;
using ReactiveUI;

namespace KanaMelody.ViewModels;

public class PlaybackControllerViewModel : ReactiveObject
{
    private readonly NowPlayingService _nowPlayingService;
    
    public PlaybackControllerViewModel(NowPlayingService nowPlayingService)
    {
        _nowPlayingService = nowPlayingService;
        Observable.Interval(TimeSpan.FromMilliseconds(100))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => UpdateComponents());
    }
    
    public bool IsPlaying => _nowPlayingService.IsPlaying;
    public string PlayButtonText => IsPlaying ? "Pause" : "Play";
    public string PlayingTitle => $"{_nowPlayingService.Title} - {_nowPlayingService.Artist} ({_nowPlayingService.Album})";
    public double TotalLength => _nowPlayingService.TotalLength;
    public string CurrentPositionString => TimeSpan.FromSeconds(CurrentPosition).ToString(@"mm\:ss");
    public string TotalLengthString => TimeSpan.FromSeconds(TotalLength).ToString(@"mm\:ss");

    private double _currentPosition;
    public double CurrentPosition
    {
        get => _currentPosition;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentPosition, value);
            _nowPlayingService.SetPosition(value);
        }
    }
    
    public void UpdateComponents()
    {
        _currentPosition = _nowPlayingService.CurrentPosition;
        this.RaisePropertyChanged(nameof(IsPlaying));
        this.RaisePropertyChanged(nameof(PlayingTitle));
        this.RaisePropertyChanged(nameof(PlayButtonText));
        this.RaisePropertyChanged(nameof(CurrentPosition));
        this.RaisePropertyChanged(nameof(TotalLength));
        this.RaisePropertyChanged(nameof(CurrentPositionString));
        this.RaisePropertyChanged(nameof(TotalLengthString));
    }
    
    public void PlayCommand()
    {
        if (IsPlaying)
            _nowPlayingService.Pause();
        else
            _nowPlayingService.Play();
        this.RaisePropertyChanged(nameof(IsPlaying));
        this.RaisePropertyChanged(nameof(PlayingTitle));
        this.RaisePropertyChanged(nameof(PlayButtonText));
    }
}