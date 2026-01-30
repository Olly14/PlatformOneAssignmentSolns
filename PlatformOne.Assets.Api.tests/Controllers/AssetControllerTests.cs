namespace PlatformOne.Assets.Api.tests.Controllers;

public class AssetControllerTests : IClassFixture<AssetsControllerFixture>, IDisposable
{
    private readonly AssetsControllerFixture _fixture;

    public AssetControllerTests(AssetsControllerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GivenOnlycancellationToken_WhenCallingGet_ThenApiResultWithAssetDtoAsDatareturns()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var expectedResponse = _fixture.GetAssetsResponse;
        _fixture.MockAssetService.Setup(x => x.GetAssetsAsync(It.IsAny<CancellationToken>(), It.IsAny<string?>(), It.IsAny<string?>())).ReturnsAsync(expectedResponse);

        // Act
        var response = await _fixture.Sut.Get(null, null, cts.Token);

        //Asset
        var apiResult = _fixture.Asserter.AssertOkResult(response);
        apiResult.Data.Should().BeAssignableTo<IReadOnlyList<AssetDto>>()
            .And.BeEquivalentTo(expectedResponse);

        _fixture.MockAssetService.Verify(x => x.GetAssetsAsync((cts.Token)), Times.Once);
    }

    [Fact]
    public async Task GivenIsinAndCancellationTokenOnly_WhenCallingGet_ThenApiResultWithFilteredAssetDtoAsDatareturns()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var isin = _fixture.Isin;
        var expectedResponse = _fixture.GetIsinAssetsResponse;
        _fixture.MockAssetService.Setup(x => x.GetAssetsAsync(It.IsAny<CancellationToken>(), It.IsAny<string?>(), It.IsAny<string?>())).ReturnsAsync(expectedResponse);

        // Act
        var response = await _fixture.Sut.Get(null, isin: isin, cts.Token);

        //Asset
        var apiResult = _fixture.Asserter.AssertOkResult(response);
        apiResult.Data.Should().BeAssignableTo<IReadOnlyList<AssetDto>>()
            .And.BeEquivalentTo(expectedResponse);

        _fixture.MockAssetService.Verify(x => x.GetAssetsAsync(cts.Token, null, isin), Times.Once);
    }

    [Fact]
    public async Task GivenSymbolAndCancellationTokenOnly_WhenCallingGet_ThenApiResultWithFilteredAssetDtoAsDatareturns()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var symbol = _fixture.Symbol;
        var expectedResponse = _fixture.GetSymbolAssetsResponse;
        _fixture.MockAssetService.Setup(x => x.GetAssetsAsync(It.IsAny<CancellationToken>(), It.IsAny<string?>(), It.IsAny<string?>())).ReturnsAsync(expectedResponse);

        // Act
        var response = await _fixture.Sut.Get(symbol: symbol, null, cts.Token);

        //Asset
        var apiResult = _fixture.Asserter.AssertOkResult(response);
        apiResult.Data.Should().BeAssignableTo<IReadOnlyList<AssetDto>>()
            .And.BeEquivalentTo(expectedResponse);

        _fixture.MockAssetService.Verify(x => x.GetAssetsAsync(cts.Token, symbol), Times.Once);
    }

    [Fact]
    public async Task GivenSymbolUpsertAssetRequestDtoAndCancellationToken_WhenCallingUpsert_ThenCreatesAsset()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var expectedResponse = _fixture.UpsertAssetDtoResponse;
        _fixture.MockAssetService.Setup(x => x.UpsertAsync(It.IsAny<string>(), It.IsAny<UpsertAssetRequestDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

        // Act
        var response = await _fixture.Sut.Upsert(_fixture.UpsertSymbol, _fixture.UpsertAssetRequestDto, cts.Token);

        //Asset
        var apiResult = _fixture.Asserter.AssertOkResult(response);
        apiResult.Data.Should().BeAssignableTo<AssetDto>()
            .And.BeEquivalentTo(expectedResponse);

        _fixture.MockAssetService.Verify(x => x.UpsertAsync(_fixture.UpsertSymbol, _fixture.UpsertAssetRequestDto, cts.Token), Times.Once);
    }

    public void Dispose()
    {
        _fixture.ClearAllInvocations();
        GC.SuppressFinalize(this);
    }
}
