namespace PlatformOne.Assets.Api.tests.Fixtures;

public class AssetsControllerFixture : BaseControllerFixture
{
    public AssetsControllerFixture()
    {
        var fixture = new Fixture() { OmitAutoProperties = true }.Customize(new AutoMoqCustomization());

        MockAssetService = fixture.Freeze<Mock<IAssetService>>();
        Symbol = "AAPL";
        UpsertSymbol = "TST123";
        Isin = "USTSLA0331005";
        var isinFiltered = NewAssets().Where(x => x.Symbol.Equals(Isin)).ToList();
        var symbolFiltered = NewAssets().Where(x => x.Symbol.Equals(Symbol)).ToList();
        Sut = new AssetsController(MockAssetService.Object);
        GetAssetsResponse = MapAssetEntitiesToAssetDtos(NewAssets());
        GetIsinAssetsResponse = MapAssetEntitiesToAssetDtos(isinFiltered);
        GetSymbolAssetsResponse = MapAssetEntitiesToAssetDtos(symbolFiltered);
        UpsertAssetRequestDto = fixture.Build<UpsertAssetRequestDto>()
                    .With(x => x.Name, "Test Asset")
                    .With(x => x.Isin, "TESTISIN12345")
                    .Create();
        UpsertAssetDtoResponse = fixture.Build<AssetDto>()
                    .With(x => x.Name, UpsertAssetRequestDto.Name)
                    .With(x => x.Isin, UpsertAssetRequestDto.Isin)
                    .With(x => x.Symbol, "TST123")
                    .Create();
    }

    public Mock<IAssetService> MockAssetService { get; }

    public AssetsController Sut {  get; }
    public string Isin { get; }
    public string Symbol { get; }
    public string UpsertSymbol { get; }
    public UpsertAssetRequestDto UpsertAssetRequestDto { get;  }
    public AssetDto UpsertAssetDtoResponse { get; }
    public IReadOnlyList<AssetDto> GetAssetsResponse { get; }
    public IReadOnlyList<AssetDto> GetIsinAssetsResponse { get; }
    public IReadOnlyList<AssetDto> GetSymbolAssetsResponse { get; }

    public void ClearAllInvocations()
    {
        MockAssetService.Reset();
    }
}
