namespace PlatformOne.Assets.Shared.Tests.Fixtures;

public class AssetServiceFixture
{
    public AssetServiceFixture()
    {
        var fixture = new Fixture() { OmitAutoProperties = true }.Customize(new AutoMoqCustomization());
        MockMapper = fixture.Freeze<Mock<IMapper>>();
        SqliteInMemoryDbFactory = new SqliteInMemoryDbFactory();
        AssetEntities = NewAssets();
        Assets = MapAssetEntitiesToAssetDtos(AssetEntities);
        Symbol = "AAPL";
        ExistingIsin = "US0378331005";
        MSTFExistingIsin = "US5949181045";
        UpsertAssetRequestDto = fixture.Build<UpsertAssetRequestDto>()
                    .With(x => x.Name, "New Asset")
                    .With(x => x.Isin, ExistingIsin)
                    .Create();
        NewAsset = new Asset
        {
            Id = Guid.NewGuid(),
            Name = "Microsoft",
            Symbol = "MSFT",
            Isin = MSTFExistingIsin
        };
    }
    public Mock<IMapper> MockMapper { get; }
    public string Symbol { get; }
    public string ExistingIsin { get; }
    public string MSTFExistingIsin { get; }
    public Asset NewAsset { get; }
    public UpsertAssetRequestDto UpsertAssetRequestDto { get; }
    public IReadOnlyList<AssetDto> Assets { get; }
    public IEnumerable<Asset> AssetEntities { get; }
    public SqliteInMemoryDbFactory SqliteInMemoryDbFactory { get; }
    public void ClearAll()
    {
        MockMapper.Reset();
    }

    private static IEnumerable<Asset> NewAssets()
    {
        return [
            new Asset
            {
                Id = Guid.NewGuid(),
                Name = "Apple Inc",
                Symbol = "AAPL",
                Isin = "US0378331005"
            },
            new Asset
            {
                Id = Guid.NewGuid(),
                Name = "Microsoft Corporation",
                Symbol = "MSFT",
                Isin = "US5949181045"
            },
            new Asset
            {
                Id = Guid.NewGuid(),
                Name = "Tesla Inc",
                Symbol = "TSLA",
                Isin = "USTSLA0331005"
            },
            new Asset
            {
                Id = Guid.NewGuid(),
                Name = "AirBus Inc",
                Symbol = "ARBUS",
                Isin = "EARBUS0331005"
            },
        ];
    }

    public IReadOnlyList<AssetDto> MapAssetEntitiesToAssetDtos(IEnumerable<Asset> entities)
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
