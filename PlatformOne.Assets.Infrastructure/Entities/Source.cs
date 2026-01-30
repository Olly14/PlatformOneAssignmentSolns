namespace PlatformOne.Assets.Infrastructure.Entities;

public class Source
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<AssetPrice> Prices { get; set; } = new List<AssetPrice>();
}
