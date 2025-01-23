using System;
using System.Linq;
using ATL;
using KanaMelody.Database;
using KanaMelody.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KanaMelody.Services;

public class DatabaseService
{
    private readonly SongDatabaseContext _songDatabaseContext;
    private readonly ConfigService _configService;
    
    public DatabaseService(SongDatabaseContext songDatabaseContext, ConfigService configService)
    {
        _songDatabaseContext = songDatabaseContext;
        _configService = configService;
    }

    public void UpdateSongEntry(bool forceUpdate = false)
    {
        if (forceUpdate)
        {
            // Clear all songs in the database
            _songDatabaseContext.Songs.RemoveRange(_songDatabaseContext.Songs);
        }
        
        foreach (var folderPath in _configService.FolderSettings.FolderPath)
        {
            foreach (var songPath in SongEntryServices.GetAllSongs(folderPath))
            {
                Log.Information("Processing song {SongPath}", songPath);
                // Find the song in the database
                var song = _songDatabaseContext.Songs.Include(song => song.Metadata).FirstOrDefault(s => s.Path == songPath);
                if (song == null)
                {
                    try
                    {
                        var trackFile = new Track(songPath);
                        // Add the song to the database
                        _songDatabaseContext.Songs.Add(new Song
                        {
                            Path = songPath,
                            Metadata = new SongMetadata
                            {
                                Title = trackFile.Title,
                                Artist = trackFile.Artist,
                                Album = trackFile.Album,
                                TrackNumber = trackFile.TrackNumber
                            }
                        });
                        Log.Information("Added song {SongPath} to the database", songPath);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Failed to add song {SongPath} to the database", songPath);
                    }
                }
                else
                {
                    if (forceUpdate)
                    {
                        try
                        {
                            var trackFile = new Track(songPath);
                            // Update the song in the database
                            if (song.Metadata != null)
                            {
                                song.Metadata.Title = trackFile.Title;
                                song.Metadata.Artist = trackFile.Artist;
                                song.Metadata.Album = trackFile.Album;
                                song.Metadata.TrackNumber = trackFile.TrackNumber;
                            }
                            else
                            {
                                song.Metadata = new SongMetadata
                                {
                                    Title = trackFile.Title,
                                    Artist = trackFile.Artist,
                                    Album = trackFile.Album,
                                    TrackNumber = trackFile.TrackNumber
                                };
                            }
                            Log.Information("Updated song {SongPath} in the database", songPath);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, "Failed to update song {SongPath} in the database", songPath);
                        }
                    }
                }
            }
        }
        
        // Save changes to the database
        _songDatabaseContext.SaveChanges();
        Log.Information("Database updated successfully");
    }
}