using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace KanaMelody.Components;

public partial class StatusBar : UserControl
{
    public StatusBar()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}