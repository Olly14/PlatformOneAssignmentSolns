namespace PlatformOne.Assets.Api.tests.Controllers;

public class AssetPriceControllerTests : IClassFixture<AssetPriceControllerFixture>, IDisposable
{
    private readonly AssetPriceControllerFixture _fixture;

    public AssetPriceControllerTests(AssetPriceControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GivenUpsertPriceRequestDtoAndCancellationToken_WhenCallingUpsert_ThenCreatesPrice()
    {
        // Arrange: ensure asset exists
        var cts = new CancellationTokenSource();
        var expectedResponse = _fixture.UpsertPriceResponseDto;
        _fixture.MockAssetPriceService.Setup( x => x.UpsertPriceAsync(It.IsAny<CancellationToken>(), It.IsAny<UpsertPriceRequestDto>())).ReturnsAsync(expectedResponse);

        // Act
        var response = await _fixture.Sut.Upsert(_fixture.UpsertPriceRequestDto, cts.Token);

        //Asset

        var apiResult = _fixture.Asserter.AssertOkResult(response);
        apiResult.Data.Should().BeAssignableTo<UpsertPriceResponseDto>()
            .And.BeEquivalentTo(expectedResponse);

        _fixture.MockAssetPriceService.Verify(x => x.UpsertPriceAsync(cts.Token, _fixture.UpsertPriceRequestDto), Times.Once);
    }

    [Fact]
    public async Task GivenDateOnlyNoSourceNoSymbolsAndCancellationToken_WhenCallingGet_ThenListOfGetPricesResponseDtoReturns()
    {
        // Arrange: ensure asset exists
        var date = new DateOnly(2026, 1, 29);
        var cts = new CancellationTokenSource();
        var expectedResponse = _fixture.GetPricesAsyncDateOnlyResult;
        _fixture.MockAssetPriceService.Setup(x => x.GetPricesAsync(It.IsAny<CancellationToken>(), It.IsAny<DateOnly>())).ReturnsAsync(expectedResponse);

        // Act
        var response = await _fixture.Sut.Get(date, null, null, cts.Token);

        //Assert

        var apiResult = _fixture.Asserter.AssertOkResult(response);
        apiResult.Data.Should().BeAssignableTo<GetPricesResponseDto>()
            .And.BeEquivalentTo(expectedResponse);

        _fixture.MockAssetPriceService.Verify(x => x.GetPricesAsync(cts.Token, date), Times.Once);
    }

    /*[Fact]
    public async Task GivenExistingAssetSourceDate_WhenUpsertingPrice_ThenUpdatesPriceAndTimestamp()
    {
        // Arrange: ensure asset exists
        await UpsertAssetAsync("MSFT", "Microsoft Corporation", "US5949181045");

        var date = new DateOnly(2026, 01, 29);

        var first = new UpsertPriceRequestDto("MSFT", "Reuters market data", date, 100m);
        var r1 = await _client.PutAsJsonAsync("/api/prices", first);
        r1.StatusCode.Should().Be(HttpStatusCode.OK);

        var firstBody = await r1.Content.ReadFromJsonAsync<UpsertPriceResponseDto>();
        firstBody.Should().NotBeNull();
        var firstTimestamp = firstBody!.LastUpdatedUtc;

        // Small delay to ensure timestamp changes
        await Task.Delay(50);

        // Act: upsert same key, different price
        var second = new UpsertPriceRequestDto("MSFT", "Reuters market data", date, 200m);
        var r2 = await _client.PutAsJsonAsync("/api/prices", second);

        // Assert
        r2.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondBody = await r2.Content.ReadFromJsonAsync<UpsertPriceResponseDto>();
        secondBody.Should().NotBeNull();
        secondBody!.Created.Should().BeFalse();
        secondBody.Price.Should().Be(200m);
        secondBody.LastUpdatedUtc.Should().BeAfter(firstTimestamp);

        // Optional: verify GET reflects the update
        var get = await _client.GetAsync($"/api/prices?date={date:yyyy-MM-dd}&symbols=MSFT&source=Reuters%20market%20data");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var getBody = await get.Content.ReadFromJsonAsync<GetPricesResponseDto>();
        getBody.Should().NotBeNull();
        getBody!.Results.Should().HaveCount(1);
        getBody.Results[0].Price.Should().Be(200m);
    }

    // --- helpers ---

    private async Task UpsertAssetAsync(string symbol, string name, string isin)
    {
        var req = new UpsertAssetRequest { Name = name, Isin = isin };
        var res = await _client.PutAsJsonAsync($"/api/assets/{symbol}", req);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }*/

    public void Dispose()
    {
        _fixture.ClearAll();
    }
}
