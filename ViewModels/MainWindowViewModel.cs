using KanaMelody.Services;

namespace KanaMelody.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly App _mainApp;
    
    public MainWindowViewModel(PlaybackControllerViewModel playbackControllerViewModel, PlaylistViewModel playlistViewModel, App app)
    {
        PlaybackControllerViewModel = playbackControllerViewModel;
        PlaylistViewModel = playlistViewModel;
        _mainApp = app;
    }
    
    public PlaybackControllerViewModel PlaybackControllerViewModel { get; }
    public PlaylistViewModel PlaylistViewModel { get; }
    
    public void SelectNewFolderCommand()
    {
        _mainApp.ShowFolderSelectionDialog();
    }
}