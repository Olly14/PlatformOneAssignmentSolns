namespace PlatformOne.Assets.Shared.Tests.Services;

public sealed class AssetPriceServiceTests : IClassFixture<AssetPriceServiceFixture>, IDisposable
{
    private readonly AssetPriceServiceFixture _fixture;

    public AssetPriceServiceTests(AssetPriceServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GivenDate_WhenGetPricesAsync_ThenReturnsAllPricesForThatDate()
    {
        // Arrange
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        await _fixture.SeedAsync(ctx, ct);

        var service = new AssetPriceService(ctx);

        // Act
        var result = await service.GetPricesAsync(ct, _fixture.PriceDate);

        // Assert
        result.Should().NotBeNull();
        result.Date.Should().Be(_fixture.PriceDate);
        result.Source.Should().BeNull();
        result.Results.Should().HaveCount(3);
        result.Results.Select(r => r.LastUpdatedUtc).Should().AllSatisfy(d => d.Should().NotBe(default));
    }

    [Fact]
    public async Task GivenDateAndSource_WhenGetPricesAsync_ThenReturnsOnlyThatSourceForThatDate()
    {
        // Arrange
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        await _fixture.SeedAsync(ctx, ct);

        var service = new AssetPriceService(ctx);

        // Act
        var result = await service.GetPricesAsync(ct, _fixture.PriceDate, source: _fixture.Reuters);

        // Assert
        result.Source.Should().Be(_fixture.Reuters);
        result.Results.Should().HaveCount(2);
        result.Results.Should().OnlyContain(r => r.Source == _fixture.Reuters);
    }

    [Fact]
    public async Task GivenDateAndSymbols_WhenGetPricesAsync_ThenReturnsOnlyThoseAssets()
    {
        // Arrange
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        await _fixture.SeedAsync(ctx, ct);

        var service = new AssetPriceService(ctx);

        // Act (only MSFT)
        var result = await service.GetPricesAsync(ct, _fixture.PriceDate, symbols: new[] { "MSFT" });

        // Assert (MSFT has 2 prices in seed)
        result.Results.Should().HaveCount(2);
        result.Results.Should().OnlyContain(r => r.Symbol == _fixture.MsftSymbol);
    }

    [Fact]
    public async Task GivenNewPrice_WhenUpsertPriceAsync_ThenCreatesPriceAndReturnsTimestamp()
    {
        // Arrange
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        await _fixture.SeedAsync(ctx, ct);

        var service = new AssetPriceService(ctx);

        var request = new UpsertPriceRequestDto
        {
            Symbol = _fixture.AaplSymbol,               // AAPL exists
            Source = "NewSource",                      // does not exist (should be auto-created)
            Date = _fixture.PriceDate,
            Price = 555.55m
        };

        // Act
        var result = await service.UpsertPriceAsync(ct, request);

        // Assert
        result.Symbol.Should().Be(_fixture.AaplSymbol);
        result.Source.Should().Be("NewSource");
        result.Price.Should().Be(555.55m);
        result.LastUpdatedUtc.Should().BeAfter(DateTimeOffset.UtcNow.AddMinutes(-1));

        // Verify persisted
        var dbRow = await ctx.AssetPrices
            .AsNoTracking()
            .SingleAsync(ap => ap.PriceDate == _fixture.PriceDate && ap.Price == 555.55m, ct);

        dbRow.LastUpdatedUtc.Should().Be(result.LastUpdatedUtc);
    }

    [Fact]
    public async Task GivenExistingPrice_WhenUpsertPriceAsync_ThenUpdatesPriceAndPersists()
    {
        // Arrange
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        await _fixture.SeedAsync(ctx, ct);

        var service = new AssetPriceService(ctx);

        // Existing in seed: MSFT + Reuters + PriceDate
        var request = new UpsertPriceRequestDto
        {
            Symbol = _fixture.MsftSymbol,
            Source = _fixture.Reuters,
            Date = _fixture.PriceDate,
            Price = 999.99m
        };

        // Grab old timestamp
        var before = await ctx.AssetPrices
            .AsNoTracking()
            .SingleAsync(ap =>
                ap.Asset.Symbol == _fixture.MsftSymbol &&
                ap.Source.Name == _fixture.Reuters &&
                ap.PriceDate == _fixture.PriceDate, ct);



        // Act
        var result = await service.UpsertPriceAsync(ct, request);

        // Assert
        result.Created.Should().BeFalse();
        result.Price.Should().Be(999.99m);

        var updated = await ctx.AssetPrices.AsNoTracking()
            .SingleAsync(ap => ap.Asset.Symbol == _fixture.MsftSymbol
                            && ap.Source.Name == _fixture.Reuters
                            && ap.PriceDate == _fixture.PriceDate, ct);

        updated.Price.Should().Be(999.99m);
        updated.LastUpdatedUtc.Should().Be(result.LastUpdatedUtc);
    }

    [Fact]
    public async Task GivenUnknownSymbol_WhenUpsertPriceAsync_ThenThrowsKeyNotFound()
    {
        // Arrange
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        await _fixture.SeedAsync(ctx, ct);

        var service = new AssetPriceService(ctx);

        var request = new UpsertPriceRequestDto
        {
            Symbol = "XXXX",
            Source = _fixture.Reuters,
            Date = _fixture.PriceDate,
            Price = 1m
        };

        // Act
        var act = async () => await service.UpsertPriceAsync(ct, request);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    public void Dispose()
    {
        _fixture.ClearAll();
        GC.SuppressFinalize(this);
    }
}
