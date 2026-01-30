namespace PlatformOne.Assets.Infrastructure.Configurations;

internal class AssetConfig : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Symbol).IsRequired().HasMaxLength(32);
        builder.Property(x => x.Isin).IsRequired().HasMaxLength(32);

        builder.HasIndex(x => x.Symbol).IsUnique();
        builder.HasIndex(x => x.Isin).IsUnique();
    }
}
