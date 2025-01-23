using System.Collections.Generic;
using System.Linq;
using KanaMelody.Models;
using KanaMelody.Models.Configs;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace KanaMelody.Database;

public sealed class SongDatabaseContext : DbContext
{
    public DbSet<Song> Songs { get; set; }
    public DbSet<SongMetadata> SongMetadatas { get; set; }
    
    public string DbPath { get; }
    
    public SongDatabaseContext(StorageSettings storageSettings)
    {
        DbPath = storageSettings.GetDatabaseLocation();

        Database.EnsureCreated();
        
        Log.Information("Database found at {DbPath}", DbPath);
        // Check if there are any pending migrations
        var pendingMigrations = Database.GetPendingMigrations();

        IEnumerable<string> migrations = pendingMigrations as string[] ?? pendingMigrations.ToArray();
        
        if (migrations.Any())
        {
            Log.Information("Database is outdated, migrating...");
            foreach (var migration in migrations)
            {
                Log.Information("Applying migration {Migration}", migration);
            }
            Log.Information("Database migrated successfully");
        }
        else
        {
            Log.Information("Database is up-to-date, no migration needed");
        }
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}