namespace PlatformOne.Assets.Infrastructure.Configurations;

internal class SourceConfig : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.Name).IsUnique();
    }
}
