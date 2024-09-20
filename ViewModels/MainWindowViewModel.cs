namespace KanaMelody.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(PlaybackControllerViewModel playbackControllerViewModel)
    {
        PlaybackControllerViewModel = playbackControllerViewModel;
    }
    
    public PlaybackControllerViewModel PlaybackControllerViewModel { get; }
}