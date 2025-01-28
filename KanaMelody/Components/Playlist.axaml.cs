using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KanaMelody.Components;

public partial class Playlist : UserControl
{
    public Playlist()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}