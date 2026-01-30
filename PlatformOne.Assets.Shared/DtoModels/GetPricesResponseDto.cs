namespace PlatformOne.Assets.Shared.DtoModels;

public class GetPricesResponseDto
{
    public DateOnly Date { get; set; } = new();
    public string? Source { get; set; } = string.Empty;
    public IReadOnlyList<PriceRowDto> Results { get; set; } = Array.Empty<PriceRowDto>();
}
