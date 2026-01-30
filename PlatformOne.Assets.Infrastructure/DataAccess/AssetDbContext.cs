namespace PlatformOne.Assets.Infrastructure.DataAccess;


/// <summary>
/// Represents the Entity Framework Core database context for managing asset-related entities, including assets,
/// sources, and asset prices.
/// </summary>
/// <remarks>The AssetDbContext is configured to apply all entity configurations from the assembly containing the
/// context. It provides DbSet properties for querying and saving instances of asset, source, and asset price entities.
/// This context is intended for use in applications that require data access and manipulation for asset management
/// scenarios.</remarks>
public class AssetDbContext : DbContext
{

    public AssetDbContext(DbContextOptions<AssetDbContext> options)
        : base(options) { }

    /// <summary>
    /// Gets the set of assets in the database, allowing for querying and manipulation of asset data.
    /// </summary>
    /// <remarks>This property provides access to the underlying DbSet for the Asset entity, enabling
    /// operations such as adding, removing, and querying assets. It is important to ensure that the context is properly
    /// configured before accessing this property.</remarks>
    public DbSet<Asset> Assets => Set<Asset>();

    /// <summary>
    /// Gets the set of sources available in the database context.
    /// </summary>
    public DbSet<Source> Sources => Set<Source>();

    /// <summary>
    /// Gets the set of asset price entities in the context.
    /// </summary>
    /// <remarks>Use this property to query, add, update, or remove asset price records from the database.
    /// Changes made to the returned set are tracked by the context and persisted to the database when SaveChanges is
    /// called.</remarks>
    public DbSet<AssetPrice> AssetPrices => Set<AssetPrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssetDbContext).Assembly);
    }
}
