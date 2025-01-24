using System;
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
    private readonly PlaylistService _playlistService;
    
    private bool loop;
    
    public NowPlayingService(ConfigService configService, PlaylistService playlistService)
    {
        _nowPlaying = new NowPlaying();
        _configService = configService;
        _playlistService = playlistService;
        if (configService.PlayerSettings.LatestSongPath != string.Empty)
        {
            PlayMusic(configService.PlayerSettings.LatestSongPath);
            Pause();
            Log.Information("🎵 Found latest song path, set current song to {Path}", configService.PlayerSettings.LatestSongPath);
        }
        // Add event listener for when the song ends
        Bass.ChannelSetSync(_nowPlaying.SongStream, SyncFlags.End, 0, (handle, channel, data, user) =>
        {
            Next();
        }, IntPtr.Zero);
    }
    
    public void Play()
    {
        Bass.ChannelPlay(_nowPlaying.SongStream);
    }
    
    public void Pause()
    {
        Bass.ChannelPause(_nowPlaying.SongStream);
    }
    
    public void Next()
    {
        if (_playlistService.CurrentIndex + 1 < _playlistService.GetPlaylist().Length)
        {
            _playlistService.CurrentIndex++;
            PlayMusic(_playlistService.GetPlaylist()[_playlistService.CurrentIndex]);
        }
        else
        {
            _playlistService.CurrentIndex = 0;
            PlayMusic(_playlistService.GetPlaylist()[_playlistService.CurrentIndex]);
        }
    }
    
    public void Previous()
    {
        if (_playlistService.CurrentIndex - 1 >= 0)
        {
            _playlistService.CurrentIndex--;
            PlayMusic(_playlistService.GetPlaylist()[_playlistService.CurrentIndex]);
        }
        else
        {
            _playlistService.CurrentIndex = _playlistService.GetPlaylist().Length - 1;
            PlayMusic(_playlistService.GetPlaylist()[_playlistService.CurrentIndex]);
        }
    }
    
    public bool IsPlaying => Bass.ChannelIsActive(_nowPlaying.SongStream) == PlaybackState.Playing;
    public bool IsLooping
    {
        get => loop;
        set
        {
            loop = value;
            if (loop)
            {
                Bass.ChannelFlags(_nowPlaying.SongStream, BassFlags.Loop, BassFlags.Loop);
            }
            else
            {
                Bass.ChannelFlags(_nowPlaying.SongStream, BassFlags.Loop, BassFlags.Default);
            }
        }
    }
    
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
    public void PlayMusic(Song song)
    {
        _nowPlaying.Title = song.Metadata.Title;
        _nowPlaying.Artist = song.Metadata.Artist;
        _nowPlaying.Album = song.Metadata.Album;
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