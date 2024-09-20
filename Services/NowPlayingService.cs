using System;
using KanaMelody.Models;
using ManagedBass;

namespace KanaMelody.Services;

public class NowPlayingService
{ 
    private readonly NowPlaying _nowPlaying;
    
    public NowPlayingService(NowPlaying nowPlaying)
    {
        _nowPlaying = nowPlaying;
        _nowPlaying.Title = "Title";
    }
    
    public void Play()
    {
        if (_nowPlaying.SongStream == 0)
        {
            PlayMusic();
        }
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
    
    public void PlayMusic()
    {
        if (_nowPlaying.SongStream != 0)
        {
            Bass.ChannelStop(_nowPlaying.SongStream);
            Bass.StreamFree(_nowPlaying.SongStream);
        }
        _nowPlaying.SongStream = Bass.CreateStream("/Users/helloyeew/Music/Personal/[M3-2020秋] At the Garret - Invitations to Black Theater/00-02-A rumor of Black Theater.flac");
        Console.WriteLine(_nowPlaying.SongStream);
        if (_nowPlaying.SongStream == 0)
        {
            return;
        }
        Bass.ChannelFlags(_nowPlaying.SongStream, BassFlags.Loop, BassFlags.Loop);
        Bass.ChannelPlay(_nowPlaying.SongStream);
        Bass.ChannelPlay(_nowPlaying.SongStream);
        
        var tfile = TagLib.File.Create("/Users/helloyeew/Music/Personal/[M3-2020秋] At the Garret - Invitations to Black Theater/00-02-A rumor of Black Theater.flac");
        _nowPlaying.Title = tfile.Tag.Title ?? "";
        _nowPlaying.Artist = tfile.Tag.FirstPerformer ?? "";
        _nowPlaying.Album = tfile.Tag.Album ?? "";
    }
}