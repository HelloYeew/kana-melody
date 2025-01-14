using System.Globalization;
using System.IO;
using ATL;
using Avalonia.Media.Imaging;
using KanaMelody.Models;
using ManagedBass;
using Serilog;

namespace KanaMelody.Services;

public class NowPlayingService
{ 
    private readonly NowPlaying _nowPlaying;

    private readonly ConfigService _configService;
    
    public NowPlayingService(ConfigService configService)
    {
        _nowPlaying = new NowPlaying();
        _configService = configService;
        if (configService.PlayerSettings.LatestSongPath != string.Empty)
        {
            PlayMusic(configService.PlayerSettings.LatestSongPath);
            Pause();
            Log.Information("🎵 Found latest song path, set current song to {Path}", configService.PlayerSettings.LatestSongPath);
        }
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
    public string FileInfo = "";
    public Bitmap AlbumArt;
    
    public double Volume
    {
        get => _configService.PlayerSettings.Volume;
        set
        {
            _configService.PlayerSettings.Volume = value;
            Bass.ChannelSetAttribute(_nowPlaying.SongStream, ChannelAttribute.Volume, value / 100f);
        }
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
        Bass.ChannelSetAttribute(_nowPlaying.SongStream, ChannelAttribute.Volume, Volume / 100f);
        if (_nowPlaying.SongStream == 0)
        {
            return;
        }
        Bass.ChannelFlags(_nowPlaying.SongStream, BassFlags.Loop, BassFlags.Loop);
        Bass.ChannelPlay(_nowPlaying.SongStream);
        _configService.PlayerSettings.LatestSongPath = path;
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
        Log.Information("🎵 Playing {Title} by {Artist} from {Album}", _nowPlaying.Title, _nowPlaying.Artist, _nowPlaying.Album);
        PlayTrack(path);
        UpdateTrackInfo(path);
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
        Log.Information("🎵 Playing {Title} by {Artist} from {Album}", _nowPlaying.Title, _nowPlaying.Artist, _nowPlaying.Album);
        PlayTrack(song.Path);
        UpdateTrackInfo(song.Path);
    }
    
    /// <summary>
    /// Update the track information from the file that don't need to be record in the database
    /// </summary>
    /// <param name="path">The path to the song</param>
    private void UpdateTrackInfo(string path)
    {
        var trackFile = new Track(path);
        FileInfo = $"{trackFile.AudioFormat.ShortName} {trackFile.Bitrate}kbps {trackFile.SampleRate}Hz";
        if (trackFile.EmbeddedPictures.Count > 0)
        {
            var picture = trackFile.EmbeddedPictures[0];
            using (var ms = new MemoryStream(picture.PictureData))
            {
                AlbumArt = new Bitmap(ms);
            }
        }
        else
        {
            AlbumArt = null!;
        }
    }
}