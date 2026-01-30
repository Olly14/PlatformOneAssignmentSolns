namespace PlatformOne.Assets.Shared.Tests.DbFactory;

public sealed class SqliteTestAssetDbContext : AssetDbContext
{
    public SqliteTestAssetDbContext(DbContextOptions<AssetDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Remove SQL Server-specific default SQL expressions for SQLite tests
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                // If a property has HasDefaultValueSql("...") it will break SQLite in EnsureCreated
                if (!string.IsNullOrWhiteSpace(property.GetDefaultValueSql()))
                {
                    property.SetDefaultValueSql(null);
                }
            }
        }
    }
}

