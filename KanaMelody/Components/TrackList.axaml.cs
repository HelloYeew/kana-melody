using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KanaMelody.Components;

public partial class TrackList : UserControl
{
    public TrackList()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}