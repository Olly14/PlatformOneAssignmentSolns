namespace PlatformOne.Assets.Infrastructure.Configurations;

public class AssetPriceConfig : IEntityTypeConfiguration<AssetPrice>
{
    public void Configure(EntityTypeBuilder<AssetPrice> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price).HasColumnType("decimal(18,6)").IsRequired();
        builder.Property(x => x.LastUpdatedUtc).IsRequired();

        builder.HasOne(x => x.Asset)
            .WithMany(x => x.Prices)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Source)
            .WithMany(x => x.Prices)
            .HasForeignKey(x => x.SourceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Core rule: max one price per source per asset per day
        builder.HasIndex(x => new { x.AssetId, x.SourceId, x.PriceDate }).IsUnique();
    }
}
