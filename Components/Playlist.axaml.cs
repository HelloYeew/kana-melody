using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KanaMelody.ViewModels;

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