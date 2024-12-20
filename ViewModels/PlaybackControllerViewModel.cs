using System;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
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
    public string PlayButtonText => IsPlaying ? "\u23f8" : "\u23f5";
    public string Title => _nowPlayingService.Title;
    public string Artist => _nowPlayingService.Artist;
    public string Album => _nowPlayingService.Album;
    public double TotalLength => _nowPlayingService.TotalLength;
    public Bitmap AlbumArt => _nowPlayingService.AlbumArt;
    public string FileInfo => _nowPlayingService.FileInfo;
    public double Volume
    {
        get => _nowPlayingService.Volume;
        set => _nowPlayingService.Volume = value;
    }
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
        this.RaisePropertyChanged(nameof(Title));
        this.RaisePropertyChanged(nameof(Artist));
        this.RaisePropertyChanged(nameof(Album));
        this.RaisePropertyChanged(nameof(PlayButtonText));
        this.RaisePropertyChanged(nameof(CurrentPosition));
        this.RaisePropertyChanged(nameof(TotalLength));
        this.RaisePropertyChanged(nameof(CurrentPositionString));
        this.RaisePropertyChanged(nameof(TotalLengthString));
        this.RaisePropertyChanged(nameof(Volume));
        this.RaisePropertyChanged(nameof(AlbumArt));
        this.RaisePropertyChanged(nameof(FileInfo));
    }
    
    public void PlayCommand()
    {
        if (IsPlaying)
            _nowPlayingService.Pause();
        else
            _nowPlayingService.Play();
        this.RaisePropertyChanged(nameof(IsPlaying));
        this.RaisePropertyChanged(nameof(Title));
        this.RaisePropertyChanged(nameof(Artist));
        this.RaisePropertyChanged(nameof(Album));
        this.RaisePropertyChanged(nameof(PlayButtonText));
    }
    
    public void MuteCommand()
    {
        Volume = 0;
        this.RaisePropertyChanged(nameof(Volume));
    }
    
    public void UnmuteCommand()
    {
        Volume = 100;
        this.RaisePropertyChanged(nameof(Volume));
    }
}