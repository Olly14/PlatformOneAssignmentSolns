namespace PlatformOne.Assets.Shared.Interfaces;

public interface IAssetService
{
    /// <summary>
    /// Asynchronously retrieves a list of assets that match the specified criteria.
    /// </summary>
    /// <remarks>The method may throw exceptions if the operation is canceled or if an error occurs while
    /// retrieving assets.</remarks>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <param name="symbol">An optional asset symbol used to filter the results. If null, assets are not filtered by symbol.</param>
    /// <param name="isin">An optional International Securities Identification Number (ISIN) used to filter the results. If null, assets
    /// are not filtered by ISIN.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of asset data
    /// transfer objects that match the specified criteria.</returns>
    Task<IReadOnlyList<AssetDto>> GetAssetsAsync(CancellationToken ct, string? symbol = null, string? isin = null);
    /// <summary>
    /// Inserts a new asset or updates an existing asset with the specified symbol using the provided asset details.
    /// </summary>
    /// <remarks>If an asset with the specified symbol exists, its details are updated; otherwise, a new asset
    /// is created. The symbol must be unique to prevent conflicts.</remarks>
    /// <param name="symbol">The unique symbol that identifies the asset to insert or update. Cannot be null or empty.</param>
    /// <param name="upsertAssetRequest">An object containing the details of the asset to be inserted or updated. Must not be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an AssetDto representing the
    /// inserted or updated asset.</returns>
    Task<AssetDto> UpsertAsync(string symbol, UpsertAssetRequestDto upsertAssetRequest, CancellationToken ct);


    /// <summary>
    /// Determines asynchronously whether any items exist with the specified ISIN, excluding the item identified by the
    /// given symbol.
    /// </summary>
    /// <remarks>If the cancellation token is triggered before the operation completes, the returned task will
    /// be canceled.</remarks>
    /// <param name="isin">The International Securities Identification Number (ISIN) to search for among items.</param>
    /// <param name="excludingSymbol">The symbol of the item to exclude from the search results.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if any other
    /// items with the specified ISIN exist, excluding the given symbol; otherwise, <see langword="false"/>.</returns>
    Task<bool> AnyOtherWithIsinAsync(string isin, string excludingSymbol, CancellationToken ct);
    /// <summary>
    /// Asynchronously retrieves the asset associated with the specified symbol.
    /// </summary>
    /// <remarks>The operation may throw exceptions if the cancellation token is triggered or if an error
    /// occurs during retrieval.</remarks>
    /// <param name="symbol">The symbol of the asset to retrieve. This value must not be null or empty.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the asset corresponding to the
    /// specified symbol, or null if no asset is found.</returns>
    Task<Asset?> GetBySymbolAsync(string symbol, CancellationToken ct);
}
