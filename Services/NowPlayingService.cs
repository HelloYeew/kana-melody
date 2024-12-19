using System.Globalization;
using ATL;
using KanaMelody.Models;
using ManagedBass;

namespace KanaMelody.Services;

public class NowPlayingService
{ 
    private readonly NowPlaying _nowPlaying;
    
    public NowPlayingService()
    {
        _nowPlaying = new NowPlaying();
    }
    
    public void Play()
    {
        Bass.ChannelPlay(_nowPlaying.SongStream);
    }
    
    public void Pause()
    {
        Bass.ChannelPause(_nowPlaying.SongStream);
    }
    
    public bool IsPlaying => Bass.ChannelIsActive(_nowPlaying.SongStream) == PlaybackState.Playing;
    
    public string Title => _nowPlaying.Title;
    public string Artist => _nowPlaying.Artist;
    public string Album => _nowPlaying.Album;
    
    public int Volume
    {
        get => int.Parse((Bass.ChannelGetAttribute(_nowPlaying.SongStream, ChannelAttribute.Volume) * 100).ToString(CultureInfo.InvariantCulture));
        set => Bass.ChannelSetAttribute(_nowPlaying.SongStream, ChannelAttribute.Volume, value / 100f);
    }
    
    public double CurrentPosition => Bass.ChannelBytes2Seconds(_nowPlaying.SongStream, Bass.ChannelGetPosition(_nowPlaying.SongStream));
    public double TotalLength => Bass.ChannelBytes2Seconds(_nowPlaying.SongStream, Bass.ChannelGetLength(_nowPlaying.SongStream));
    
    public void SetPosition(double position)
    {
        Bass.ChannelSetPosition(_nowPlaying.SongStream, Bass.ChannelSeconds2Bytes(_nowPlaying.SongStream, position));
    }

    /// <summary>
    /// Play a tract to the audio device
    /// </summary>
    /// <param name="path">The path to the track</param>
    private void PlayTrack(string path)
    {
        if (_nowPlaying.SongStream != 0)
        {
            Bass.ChannelStop(_nowPlaying.SongStream);
            Bass.StreamFree(_nowPlaying.SongStream);
        }
        _nowPlaying.SongStream = Bass.CreateStream(path);
        if (_nowPlaying.SongStream == 0)
        {
            return;
        }
        Bass.ChannelFlags(_nowPlaying.SongStream, BassFlags.Loop, BassFlags.Loop);
        Bass.ChannelPlay(_nowPlaying.SongStream);
    }
    
    /// <summary>
    /// Play a song from a file path that hasn't been loaded yet
    /// </summary>
    /// <param name="path">The path to the song</param>
    public void PlayMusic(string path)
    {
        var trackFile = new Track(path);
        _nowPlaying.Title = trackFile.Title;
        _nowPlaying.Artist = trackFile.Artist;
        _nowPlaying.Album = trackFile.Album;
        PlayTrack(path);
    }
    
    /// <summary>
    /// Play a song from the playlist that's already been loaded
    /// </summary>
    /// <param name="song">The song to play</param>
    public void PlayMusic(SongEntry song)
    {
        _nowPlaying.Title = song.Title;
        _nowPlaying.Artist = song.Artist;
        _nowPlaying.Album = song.Album;
        PlayTrack(song.Path);
    }
}