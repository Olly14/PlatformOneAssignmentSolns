namespace PlatformOne.Assets.Shared.Tests.DbFactory;

public sealed class SqliteInMemoryDbFactory
{
    public async Task<SqliteInMemoryDbScope> CreateScopeAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AssetDbContext>()
            .UseSqlite(connection)
            .EnableSensitiveDataLogging()
            .Options;

        // create schema for this brand-new DB
        //await using var ctx = new MonitoringDbContext(options);
        //await ctx.Database.EnsureCreatedAsync();

        // Create schema using the SAME context type you’ll use in tests
        await using (var ctx = new SqliteTestAssetDbContext(options))
        {
            await ctx.Database.EnsureCreatedAsync();
        }
        

        return new SqliteInMemoryDbScope(options, connection);
    }
}

