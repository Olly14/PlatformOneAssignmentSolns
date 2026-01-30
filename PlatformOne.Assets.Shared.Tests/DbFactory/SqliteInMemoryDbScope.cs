namespace PlatformOne.Assets.Shared.Tests.DbFactory;

public sealed class SqliteInMemoryDbScope : IAsyncDisposable
{
    private readonly DbContextOptions<AssetDbContext> _options;

    public SqliteConnection Connection { get; }

    public SqliteInMemoryDbScope(DbContextOptions<AssetDbContext> options, SqliteConnection connection)
    {
        _options = options;
        Connection = connection;
    }

    // IMPORTANT: return your test context type (if you have one)
    public AssetDbContext CreateDbContext()
        => new SqliteTestAssetDbContext(_options);

    public async ValueTask DisposeAsync()
    {
        await Connection.CloseAsync();
        await Connection.DisposeAsync();
    }
}


