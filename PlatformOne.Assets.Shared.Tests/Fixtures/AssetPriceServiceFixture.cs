namespace PlatformOne.Assets.Shared.Tests.Fixtures;

public sealed class AssetPriceServiceFixture
{
    public SqliteInMemoryDbFactory SqliteInMemoryDbFactory { get; }

    // Constants used by tests
    public string MsftSymbol { get; } = "MSFT";
    public string MsftIsin { get; } = "US5949181045";

    public string AaplSymbol { get; } = "AAPL";
    public string AaplIsin { get; } = "US0378331005";

    public string Reuters { get; } = "Reuters market data";
    public string Bloomberg { get; } = "Bloomberg";

    public DateOnly PriceDate { get; } = new DateOnly(2026, 01, 29);

    public AssetPriceServiceFixture()
    {
        SqliteInMemoryDbFactory = new SqliteInMemoryDbFactory();
    }

    public async Task SeedAsync(AssetDbContext ctx, CancellationToken ct)
    {
        // assets
        var msft = new Asset { Id = Guid.NewGuid(), Name = "Microsoft Corporation", Symbol = MsftSymbol, Isin = MsftIsin };
        var aapl = new Asset { Id = Guid.NewGuid(), Name = "Apple Inc.", Symbol = AaplSymbol, Isin = AaplIsin };

        // sources
        var reuters = new Source { Id = Guid.NewGuid(), Name = Reuters };
        var bloomberg = new Source { Id = Guid.NewGuid(), Name = Bloomberg };

        ctx.Assets.AddRange(msft, aapl);
        ctx.Sources.AddRange(reuters, bloomberg);

        var now = DateTimeOffset.UtcNow;

        // prices for same day, different sources/assets
        ctx.AssetPrices.AddRange(
            new AssetPrice
            {
                Id = Guid.NewGuid(),
                AssetId = msft.Id,
                SourceId = reuters.Id,
                PriceDate = PriceDate,
                Price = 100.10m,
                LastUpdatedUtc = now.AddMinutes(-10)
            },
            new AssetPrice
            {
                Id = Guid.NewGuid(),
                AssetId = msft.Id,
                SourceId = bloomberg.Id,
                PriceDate = PriceDate,
                Price = 101.20m,
                LastUpdatedUtc = now.AddMinutes(-9)
            },
            new AssetPrice
            {
                Id = Guid.NewGuid(),
                AssetId = aapl.Id,
                SourceId = reuters.Id,
                PriceDate = PriceDate,
                Price = 200.30m,
                LastUpdatedUtc = now.AddMinutes(-8)
            }
        );

        await ctx.SaveChangesAsync(ct);
    }

    public void ClearAll()
    {
        // nothing special unless you cache state
    }
}
