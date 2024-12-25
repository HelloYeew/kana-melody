using System.ComponentModel;

namespace KanaMelody.Models.Configs;

public class PlayerSettings : INotifyPropertyChanged
{
    private double _volume;
    public double Volume
    {
        get => _volume;
        set
        {
            _volume = value;
            OnPropertyChanged(nameof(Volume));
        }
    }

    public static PlayerSettings Default => new PlayerSettings
    {
        Volume = 100
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}