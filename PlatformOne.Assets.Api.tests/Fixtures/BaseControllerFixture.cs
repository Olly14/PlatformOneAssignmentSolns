namespace PlatformOne.Assets.Api.tests.Fixtures;

public class BaseControllerFixture
{
    public IActionResultAsserter Asserter { get; }

    public BaseControllerFixture()
    {
        Asserter = new ActionResultAsserter();
    }

    internal static IEnumerable<Asset> NewAssets()
    {
        return [
            new Asset
            {
                Id = Guid.NewGuid(),
                Name = "Apple Inc",
                Isin = "AAPL",
                Symbol = "US0378331005"
            },
            new Asset
            {
                Id = Guid.NewGuid(),
                Name = "Microsoft Corporation",
                Isin = "MSFT",
                Symbol = "US5949181045"
            },
            new Asset
            {
                Id = Guid.NewGuid(),
                Name = "Tesla Inc",
                Isin = "TSLA",
                Symbol = "USTSLA0331005"
            },
            new Asset
            {
                Id = Guid.NewGuid(),
                Name = "AirBus Inc",
                Isin = "ARBUS",
                Symbol = "EARBUS0331005"
            },
        ];
    }

    internal static IReadOnlyList<AssetDto> MapAssetEntitiesToAssetDtos(IEnumerable<Asset> entities)
    {
        var dtos = new List<AssetDto>();
        foreach (var entity in entities)
        {
            var asset = entity as Asset;
            if (asset != null)
            {

                var dto = new AssetDto
                {
                    Isin = asset.Isin,
                    Name = asset.Name,
                    Symbol = asset.Symbol!
                };
                dtos.Add(dto);
            }
        }

        return dtos;
    }
}
