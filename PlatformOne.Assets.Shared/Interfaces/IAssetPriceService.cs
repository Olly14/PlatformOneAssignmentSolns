namespace PlatformOne.Assets.Shared.Interfaces;

public interface IAssetPriceService
{
    /// <summary>
    /// Asynchronously retrieves asset prices for the specified symbols on a given date.
    /// </summary>
    /// <remarks>This method may throw exceptions if the request fails due to network issues or invalid
    /// parameters. The returned data may vary depending on the availability of prices for the specified date, source,
    /// or symbols.</remarks>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <param name="date">The date for which to retrieve asset prices.</param>
    /// <param name="source">An optional source identifier used to filter the price data. If null, prices from all available sources are
    /// included.</param>
    /// <param name="symbols">An optional list of asset symbols to retrieve prices for. If null, prices for all available symbols are
    /// returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="GetPricesResponseDto"/>
    /// with the retrieved price data.</returns>
    Task<GetPricesResponseDto> GetPricesAsync(CancellationToken ct, DateOnly date, string? source = null, IReadOnlyList<string>? symbols = null);


    /// <summary>
    /// Inserts a new price or updates an existing price using the specified request data.
    /// </summary>
    /// <remarks>An exception may be thrown if the request data is invalid or if the operation fails due to
    /// database constraints.</remarks>
    /// <param name="ct">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <param name="upsertPriceRequest">An object containing the price information to insert or update. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a response object with the outcome
    /// of the upsert operation.</returns>
    Task<UpsertPriceResponseDto> UpsertPriceAsync(CancellationToken ct, UpsertPriceRequestDto upsertPriceRequest);
}
