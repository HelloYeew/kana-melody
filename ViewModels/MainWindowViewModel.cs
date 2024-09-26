using System.Reactive.Linq;
using ReactiveUI;

namespace KanaMelody.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(PlaybackControllerViewModel playbackControllerViewModel, PlaylistViewModel playlistViewModel)
    {
        PlaybackControllerViewModel = playbackControllerViewModel;
        PlaylistViewModel = playlistViewModel;
    }
    
    public PlaybackControllerViewModel PlaybackControllerViewModel { get; }
    public PlaylistViewModel PlaylistViewModel { get; }
}