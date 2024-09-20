using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KanaMelody.Components;

public partial class PlaybackController : UserControl
{
    public PlaybackController()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}