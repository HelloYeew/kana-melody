using System;
using KanaMelody.Services;

namespace KanaMelody.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly App _mainApp;
    private readonly DatabaseService _databaseService;
    
    public MainWindowViewModel(PlaybackControllerViewModel playbackControllerViewModel, PlaylistViewModel playlistViewModel, App app, DatabaseService databaseService)
    {
        PlaybackControllerViewModel = playbackControllerViewModel;
        PlaylistViewModel = playlistViewModel;
        _databaseService = databaseService;
        _mainApp = app;
    }
    
    public PlaybackControllerViewModel PlaybackControllerViewModel { get; }
    public PlaylistViewModel PlaylistViewModel { get; }
    
    public void SelectNewFolderCommand()
    {
        _mainApp.ShowFolderSelectionDialog();
    }
    
    public void RebuildDatabaseCommand()
    {
        _databaseService.UpdateSongEntry(true);
    }
    
    public void ExitApplicationCommand()
    {
        Environment.Exit(0);
    }
}