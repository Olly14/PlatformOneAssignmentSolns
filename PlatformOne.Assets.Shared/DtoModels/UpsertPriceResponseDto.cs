namespace PlatformOne.Assets.Shared.DtoModels;

public class UpsertPriceResponseDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Isin { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public DateOnly Date { get; set; } = new();
    public decimal Price { get; set; }
    public DateTimeOffset LastUpdatedUtc { get; set; }
    public bool Created { get; set; } = true;

}
