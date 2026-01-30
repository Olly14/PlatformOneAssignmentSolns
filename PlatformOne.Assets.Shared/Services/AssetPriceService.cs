namespace PlatformOne.Assets.Shared.Services;

public class AssetPriceService : IAssetPriceService
{
    private readonly AssetDbContext _dbContext;

    public AssetPriceService(AssetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetPricesResponseDto> GetPricesAsync(CancellationToken ct, DateOnly date, string? source = null, IReadOnlyList<string>? symbols = null)
    {
        var query = _dbContext.AssetPrices.AsNoTracking().Where(ap => ap.PriceDate == date);

        if (!string.IsNullOrWhiteSpace(source))
        {
            var sourceName = source.Trim();
            query = query.Where(ap => ap.Source.Name == sourceName);
        }

        if(symbols is not null && symbols.Count > 0)
        {
            var trimmedSymbols = symbols.Select(s => s.Trim().ToUpper()).ToList();
            query = query.Where(ap => trimmedSymbols.Contains(ap.Asset.Symbol));
            var set = symbols.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim().ToUpperInvariant()).ToHashSet();

            query = query.Where(ap => set.Contains(ap.Asset.Symbol));
        }

        var rows = await query.OrderBy(ap => ap.Asset.Symbol).ThenBy(ap => ap.Source.Name)
                              .Select(ap => new PriceRowDto
                              {
                                  Symbol = ap.Asset.Symbol,
                                  Isin = ap.Asset.Isin,
                                  Source = ap.Source.Name,
                                  Price = ap.Price,
                                  LastUpdatedUtc = ap.LastUpdatedUtc
                              })
                              .ToListAsync(ct);

        return 
            new GetPricesResponseDto
            {
                Date = date,
                Source = string.IsNullOrWhiteSpace(source) ? null : source.Trim(),
                Results = rows
            };
    }

    public async Task<UpsertPriceResponseDto> UpsertPriceAsync(CancellationToken ct, UpsertPriceRequestDto upsertPriceRequest)
    {
        if (string.IsNullOrWhiteSpace(upsertPriceRequest.Symbol))
        {
            //global handling
            throw new ArgumentException("Symbol is required.", nameof(upsertPriceRequest));
        }

        if (string.IsNullOrWhiteSpace(upsertPriceRequest.Source))
        {
            //global handling
            throw new ArgumentException("Source is required.", nameof(upsertPriceRequest));
        }

        var symbol = upsertPriceRequest.Symbol.Trim().ToUpper();
        var sourceName = upsertPriceRequest.Source.Trim();

        //1) find asset
        var asset = await _dbContext.Assets.AsNoTracking().FirstOrDefaultAsync(a => a.Symbol.ToUpper() == symbol, ct);

        if (asset is null)
        {
            //global handling
            throw new KeyNotFoundException($"Asset i'{symbol}' not found.");
        }

        //2) find source(create if missing - optional design choice)
        var source = await _dbContext.Sources.FirstOrDefaultAsync(ps => ps.Name == sourceName, ct);

        if (source is null)
        {
            source = new Source
            {
                Id = Guid.NewGuid(),
                Name = sourceName
            };
            _dbContext.Sources.Add(source);
            await _dbContext.SaveChangesAsync(ct); // ensure source is saved before using its Id
        }

        //3) Upsertprice
        var existingPrice = await _dbContext.AssetPrices.FirstOrDefaultAsync(ap => ap.AssetId == asset.Id &&
            ap.SourceId == source.Id &&
            ap.PriceDate == upsertPriceRequest.Date, ct);

        var now = DateTimeOffset.UtcNow;

        if (existingPrice == null)
        {
            var createdEntity = new AssetPrice
            {
                Id = Guid.NewGuid(),
                AssetId = asset.Id,
                SourceId = source.Id,
                PriceDate = upsertPriceRequest.Date,
                Price = upsertPriceRequest.Price,
                LastUpdatedUtc = now
            };

            _dbContext.AssetPrices.Add(createdEntity);
            await _dbContext.SaveChangesAsync(ct);

            return new UpsertPriceResponseDto
            {
                Symbol = asset.Symbol,
                Isin = asset.Isin,
                Source = source.Name,
                Date = upsertPriceRequest.Date,
                Price = upsertPriceRequest.Price,
                LastUpdatedUtc = now
            };
        }

        existingPrice.Price = upsertPriceRequest.Price;
        existingPrice.LastUpdatedUtc = now;

        await _dbContext.SaveChangesAsync(ct);

        return new UpsertPriceResponseDto
        {
            Symbol = asset.Symbol,
            Isin = asset.Isin,
            Source = source.Name,
            Date = existingPrice.PriceDate,
            Price = existingPrice.Price,
            LastUpdatedUtc = now,
            Created = false
        };  
    }
}
