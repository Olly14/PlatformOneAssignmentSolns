namespace PlatformOne.Assets.Infrastructure.DataAccess;

public class AssetDbContext : DbContext
{
    public AssetDbContext(DbContextOptions<AssetDbContext> options)
        : base(options) { }

    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Source> Sources => Set<Source>();
    public DbSet<AssetPrice> AssetPrices => Set<AssetPrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetDbContext).Assembly);
    }
}
