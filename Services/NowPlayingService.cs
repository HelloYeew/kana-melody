using System;
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
    
    public bool IsPlaying => _nowPlaying.IsPlaying;
    
    public string Title => _nowPlaying.Title;
    public string Artist => _nowPlaying.Artist;
    public string Album => _nowPlaying.Album;
    
    public NowPlaying GetNowPlaying()
    {
        return _nowPlaying;
    }
    
    public void PlayMusic(string path)
    {
        if (_nowPlaying.SongStream != 0)
        {
            Bass.ChannelStop(_nowPlaying.SongStream);
            Bass.StreamFree(_nowPlaying.SongStream);
        }
        _nowPlaying.SongStream = Bass.CreateStream(path);
        Console.WriteLine(_nowPlaying.SongStream);
        if (_nowPlaying.SongStream == 0)
        {
            return;
        }
        Bass.ChannelFlags(_nowPlaying.SongStream, BassFlags.Loop, BassFlags.Loop);
        Bass.ChannelPlay(_nowPlaying.SongStream);
        
        var tfile = TagLib.File.Create(path);
        _nowPlaying.Title = tfile.Tag.Title ?? "";
        _nowPlaying.Artist = tfile.Tag.FirstPerformer ?? "";
        _nowPlaying.Album = tfile.Tag.Album ?? "";
    }
}