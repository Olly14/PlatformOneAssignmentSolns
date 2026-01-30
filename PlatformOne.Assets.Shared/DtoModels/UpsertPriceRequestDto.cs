namespace PlatformOne.Assets.Shared.DtoModels;

public class UpsertPriceRequestDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateOnly Date { get; set; } = new();
    public decimal Price { get; set; }
}
