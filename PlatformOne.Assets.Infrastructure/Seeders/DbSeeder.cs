namespace PlatformOne.Assets.Infrastructure.Seeders;

public class DbSeeder
{
    public static async Task SeedAsync(AssetDbContext db)
    {
        if (await db.Assets!.AnyAsync()) return;

        var msft = new Asset { Id = Guid.NewGuid(), Name = "Microsoft Corporation", Symbol = "MSFT", Isin = "US5949181045" };
        var aapl = new Asset { Id = Guid.NewGuid(), Name = "Apple Inc.", Symbol = "AAPL", Isin = "US0378331005" };

        var reuters = new Source { Id = Guid.NewGuid(), Name = "Reuters market data" };
        var bloomberg = new Source { Id = Guid.NewGuid(), Name = "Bloomberg" };

        db.Assets!.AddRange(msft, aapl);
        db.Sources!.AddRange(reuters, bloomberg);

        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var now = DateTimeOffset.UtcNow;

        db.AssetPrices!.AddRange(
            new AssetPrice { Id = Guid.NewGuid(), AssetId = msft.Id, SourceId = reuters.Id, PriceDate = today, Price = 400.123456m, LastUpdatedUtc = now },
            new AssetPrice { Id = Guid.NewGuid(), AssetId = aapl.Id, SourceId = bloomberg.Id, PriceDate = today, Price = 200.654321m, LastUpdatedUtc = now }
        );

        await db.SaveChangesAsync();
    }
}
