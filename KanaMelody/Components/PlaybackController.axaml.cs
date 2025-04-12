using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace KanaMelody.Components;

public partial class PlaybackController : UserControl
{
    private Window? _pictureWindow;
    
    public PlaybackController()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void Image_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_pictureWindow is not null)
        {
            _pictureWindow.Focus();
            return;
        }
        if (sender is Image image && image.Source is not null)
        {
            _pictureWindow = new Window
            {
                Content = new Image
                {
                    Source = image.Source,
                    Stretch = Avalonia.Media.Stretch.Uniform
                },
                Title = "Album Art",
                Width = 800,
                Height = 600
            };
            _pictureWindow.Show();
            _pictureWindow.Closed += (s, e) => _pictureWindow = null;
        }
    }
}