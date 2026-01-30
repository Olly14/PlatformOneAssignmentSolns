namespace PlatformOne.Assets.Shared.Interfaces;

public interface IAssetPriceService
{
    Task<GetPricesResponseDto> GetPricesAsync(CancellationToken ct, DateOnly date, string? source = null, IReadOnlyList<string>? symbols = null);

    Task<UpsertPriceResponseDto> UpsertPriceAsync(CancellationToken ct, UpsertPriceRequestDto upsertPriceRequest);
}
