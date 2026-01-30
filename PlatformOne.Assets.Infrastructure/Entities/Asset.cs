namespace PlatformOne.Assets.Infrastructure.Entities;

public class Asset
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public string Isin { get; set; } = null!;
    public ICollection<AssetPrice> Prices { get; set; } = new List<AssetPrice>();
}
