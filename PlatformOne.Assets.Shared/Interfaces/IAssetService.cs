namespace PlatformOne.Assets.Shared.Interfaces;

public interface IAssetService
{
    Task<IReadOnlyList<AssetDto>> GetAssetsAsync(CancellationToken ct, string? symbol = null, string? isin = null);
    Task<AssetDto> UpsertAsync(string symbol, UpsertAssetRequestDto upsertAssetRequest, CancellationToken ct);

    Task<bool> AnyOtherWithIsinAsync(string isin, string excludingSymbol, CancellationToken ct);
    Task<Asset?> GetBySymbolAsync(string symbol, CancellationToken ct);
}
