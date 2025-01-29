using System;
using KanaMelody.Services;

namespace KanaMelody.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly KanaMelodyApp _mainApp;
    private readonly DatabaseService _databaseService;
    
    public MainWindowViewModel(PlaybackControllerViewModel playbackControllerViewModel, PlaylistViewModel playlistViewModel, StatusBarViewModel statusBarViewModel, TrackListViewModel trackListViewModel, KanaMelodyApp app, DatabaseService databaseService)
    {
        PlaybackControllerViewModel = playbackControllerViewModel;
        PlaylistViewModel = playlistViewModel;
        TrackListViewModel = trackListViewModel;
        StatusBarViewModel = statusBarViewModel;
        _databaseService = databaseService;
        _mainApp = app;
    }
    
    public PlaybackControllerViewModel PlaybackControllerViewModel { get; }
    public PlaylistViewModel PlaylistViewModel { get; }
    public TrackListViewModel TrackListViewModel { get; }
    public StatusBarViewModel StatusBarViewModel { get; }
    
    public void SelectNewFolderCommand()
    {
        _mainApp.ShowFolderSelectionDialog();
    }
    
    public void RebuildDatabaseCommand()
    {
        _databaseService.UpdateSongEntry();
        TrackListViewModel.UpdateTrackList();
    }
    
    public void ExitApplicationCommand()
    {
        Environment.Exit(0);
    }
}