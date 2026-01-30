namespace PlatformOne.Assets.Shared.Services;

public class AssetService : IAssetService
{
    private readonly AssetDbContext _dbContext;
    private readonly IMapper _mapper;

    public AssetService(AssetDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AssetDto>> GetAssetsAsync(CancellationToken ct, string? symbol = null, string? isin = null)
    {
        var query = _dbContext.Assets!.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(symbol))
        {
            var trimmedSymbol = symbol.Trim().ToUpperInvariant();
            query = query.Where(a => a.Symbol == trimmedSymbol);
        }

        if (!string.IsNullOrWhiteSpace(isin))
        {
            var trimmedIsin = isin.Trim().ToUpperInvariant();
            query = query.Where(a => a.Isin == trimmedIsin);
        }

        return _mapper.Map<IReadOnlyList<AssetDto>>( await query.OrderBy(a => a.Symbol).ToListAsync(ct));
    }


    public async Task<bool> AnyOtherWithIsinAsync(string isin, string excludingSymbol, CancellationToken ct)
    {
        return await _dbContext.Assets!
            .AsNoTracking()
            .AnyAsync(a => a.Isin == isin.Trim().ToUpperInvariant() && a.Symbol != excludingSymbol.Trim().ToUpperInvariant(), ct);
    }

    public async Task<Asset?> GetBySymbolAsync(string symbol, CancellationToken ct)
    {
        return await _dbContext.Assets!
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Symbol == symbol.Trim().ToUpperInvariant(), ct);
    }

    public async Task<AssetDto> UpsertAsync(string symbol, UpsertAssetRequestDto upsertAssetRequest, CancellationToken ct)
    {
        symbol = symbol.Trim().ToUpperInvariant();
        var isin = upsertAssetRequest.Isin?.Trim().ToUpperInvariant();

        if (await AnyOtherWithIsinAsync(isin!, symbol, ct))
        {
            throw new ConflictException(isin!, $"ISIN '{isin}' already exists.");
        }
        var existingAsset = await GetBySymbolAsync(symbol, ct);
        if (existingAsset is null)
        {
            var asset = new Asset { Id = Guid.NewGuid(), Name = upsertAssetRequest.Name.Trim(), Symbol = symbol, Isin = isin! };
            await _dbContext.Assets!.AddAsync(asset, ct);
            await _dbContext.SaveChangesAsync(ct);

            return _mapper.Map<AssetDto>(asset);
        }
        existingAsset.Name = upsertAssetRequest.Name.Trim();
        existingAsset.Isin = isin!;
        await _dbContext.SaveChangesAsync(ct);

        return _mapper.Map<AssetDto>(existingAsset);
    }
}
