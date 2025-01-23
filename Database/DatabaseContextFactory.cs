using KanaMelody.Services;
using Microsoft.EntityFrameworkCore.Design;

namespace KanaMelody.Database;

/// <summary>
/// Context factory for design-time tools
/// </summary>
public class DatabaseContextFactory : IDesignTimeDbContextFactory<SongDatabaseContext>
{
    public SongDatabaseContext CreateDbContext(string[] args)
    {
        var configService = new ConfigService();
        return new SongDatabaseContext(configService.StorageSettings);
    }
}