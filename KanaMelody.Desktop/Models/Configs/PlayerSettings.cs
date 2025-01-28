using System.ComponentModel;

namespace KanaMelody.Models.Configs;

public class PlayerSettings : INotifyPropertyChanged
{
    private double _volume;
    private string _latestSongPath = string.Empty;
    private bool _loop;
    
    public double Volume
    {
        get => _volume;
        set
        {
            _volume = value;
            OnPropertyChanged(nameof(Volume));
        }
    }
    
    public string LatestSongPath
    {
        get => _latestSongPath;
        set
        {
            _latestSongPath = value;
            OnPropertyChanged(nameof(LatestSongPath));
        }
    }
    
    public bool Loop
    {
        get => _loop;
        set
        {
            _loop = value;
            OnPropertyChanged(nameof(Loop));
        }
    }

    public static PlayerSettings Default => new PlayerSettings
    {
        Volume = 100,
        LatestSongPath = string.Empty,
        Loop = false
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}