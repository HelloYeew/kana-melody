using System.ComponentModel;

namespace KanaMelody.Models.Configs;

public class PlayerSettings : INotifyPropertyChanged
{
    private double _volume;
    private string _latestSongPath = string.Empty;
    
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

    public static PlayerSettings Default => new PlayerSettings
    {
        Volume = 100,
        LatestSongPath = string.Empty
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}