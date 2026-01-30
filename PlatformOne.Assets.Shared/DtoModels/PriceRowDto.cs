namespace PlatformOne.Assets.Shared.DtoModels;

public class PriceRowDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Isin { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public decimal Price { get; set; } = decimal.Zero;
    public DateTimeOffset LastUpdatedUtc { get; set; } = DateTimeOffset.MinValue;
}
