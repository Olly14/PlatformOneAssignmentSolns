namespace PlatformOne.Assets.Api.tests.Fixtures;

public class AssetPriceControllerFixture : BaseControllerFixture
{
    public AssetPriceControllerFixture()
    {
        var fixture = new Fixture() { OmitAutoProperties = true }.Customize(new AutoMoqCustomization());

        MockAssetPriceService = fixture.Freeze<Mock<IAssetPriceService>>();
        Random rnd = new();
        LastUpdatedDate = DateTimeOffset.UtcNow;
        DateOnly = new DateOnly(2026, 01, 29);
        UpsertPriceRequestDto = fixture.Build<UpsertPriceRequestDto>()
                    .With(x => x.Symbol, "MSFT")
                    .With(x => x.Source, "Reuters market data")
                    .With(x => x.Price, 123.45m)
                    .With(x => x.Date, new DateOnly(2026, 01, 29)).Create();

        UpsertPriceResponseDto = fixture.Build<UpsertPriceResponseDto>()
                    .With(x => x.Symbol, UpsertPriceRequestDto.Symbol)
                    .With(x => x.Source, UpsertPriceRequestDto.Source)
                    .With(x => x.Isin, "UpsertPriceRequestDto.Isi")
                    .With(x => x.Date, UpsertPriceRequestDto.Date)
                    .With(x => x.LastUpdatedUtc, LastUpdatedDate)
                    .With(x => x.Price, UpsertPriceRequestDto.Price).Create();


        PriceRows = fixture.Build<PriceRowDto>()
                    .With(x => x.Isin, $"UFG93736468{rnd.Next(10)}")
                    .With(x => x.LastUpdatedUtc, LastUpdatedDate)
                    .With(x => x.Price, 125.59m + rnd.Next(10))
                    .With(x => x.Source, "Reuter")
                    .With(x => x.Symbol, "MSFT")
                    .CreateMany(5).ToList();
        GetPricesAsyncDateOnlyResult = fixture.Build<GetPricesResponseDto>()
                    .With(x => x.Source, "Reuters market data")
                    .With(x => x.Date, DateOnly)
                    .With(x => x.Results, PriceRows)
                    .Create();

        Sut = new PricesController(MockAssetPriceService.Object);
    }

    public PricesController Sut { get; }
    public DateTimeOffset LastUpdatedDate { get; set; }
    public UpsertPriceRequestDto UpsertPriceRequestDto { get;}
    public UpsertPriceResponseDto UpsertPriceResponseDto { get; }
    public DateOnly DateOnly { get; }
    public IReadOnlyList<PriceRowDto> PriceRows { get; }
    public GetPricesResponseDto GetPricesAsyncDateOnlyResult { get; }
    public Mock<IAssetPriceService> MockAssetPriceService { get; }

    public void ClearAll()
    {
        MockAssetPriceService.Reset();
    }
}
