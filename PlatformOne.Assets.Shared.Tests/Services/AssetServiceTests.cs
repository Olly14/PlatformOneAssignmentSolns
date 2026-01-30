namespace PlatformOne.Assets.Shared.Tests.Services;

public class AssetServiceTests : IClassFixture<AssetServiceFixture>, IDisposable
{
    private readonly AssetServiceFixture _fixture;
    public AssetServiceTests(AssetServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GivenOnlyCancellationToken_WhenCallingGetAssetsAsync_ThenReadOnlyListOfAssetDtoReturns()
    {
        //Arrange
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        ctx.Assets.AddRange(_fixture.AssetEntities);
        await ctx.SaveChangesAsync(ct);
        _fixture.MockMapper.Setup(m => m.Map<IReadOnlyList<AssetDto>>(It.IsAny<IReadOnlyList<Asset>>()))
            .Returns(_fixture.MapAssetEntitiesToAssetDtos(_fixture.AssetEntities));
        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        //Act
        var result = await service.GetAssetsAsync(ct);

        //Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(_fixture.MapAssetEntitiesToAssetDtos(_fixture.AssetEntities));
    }

    [Fact]
    public async Task GivenCancellationTokenAndSymbol_WhenCallingGetAssetsAsync_ThenFilteredReadOnlyListOfAssetDtoReturns()
    {
        //Arrange
        var exptedResult = _fixture.MapAssetEntitiesToAssetDtos([_fixture.AssetEntities.FirstOrDefault(x => x.Symbol == _fixture.Symbol)!]);
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        ctx.Assets.AddRange(_fixture.AssetEntities);
        await ctx.SaveChangesAsync(ct);
        _fixture.MockMapper.Setup(m => m.Map<IReadOnlyList<AssetDto>>(It.IsAny<IReadOnlyList<Asset>>()))
            .Returns(exptedResult);
        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        //Act
        var result = await service.GetAssetsAsync(ct, _fixture.Symbol);

        //Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(exptedResult);
    }

    [Fact]
    public async Task GivenCancellationTokenAndIsin_WhenCallingGetAssetsAsync_ThenFilteredReadOnlyListOfAssetDtoReturns()
    {
        //Arrange
        var exptedResult = _fixture.MapAssetEntitiesToAssetDtos([_fixture.AssetEntities.FirstOrDefault(x => x.Isin == _fixture.ExistingIsin)!]);
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        ctx.Assets.AddRange(_fixture.AssetEntities);
        await ctx.SaveChangesAsync(ct);
        _fixture.MockMapper.Setup(m => m.Map<IReadOnlyList<AssetDto>>(It.IsAny<IReadOnlyList<Asset>>()))
            .Returns(exptedResult);
        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        //Act
        var result = await service.GetAssetsAsync(ct, isin: _fixture.ExistingIsin);

        //Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(exptedResult);
    }

    [Fact]
    public async Task GivenIsinExistsOnDifferentSymbol_WhenCallingAnyOtherWithIsinAsync_ReturnsTrue()
    {
        // Arrange
        var ct = CancellationToken.None;

        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();

        ctx.Assets.AddRange(_fixture.AssetEntities);
        await ctx.SaveChangesAsync(ct);
        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        // Act
        var result = await service.AnyOtherWithIsinAsync(_fixture.MSTFExistingIsin, excludingSymbol: "AAPL", ct);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public async Task GivenSymbolCancellationTokenAndIsin_WhenCallingAnyOtherWithIsinAsyncAndExistNot_ThenFalseReturns()
    {
        //Arrange
        var ct = CancellationToken.None;
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        ctx.Assets.AddRange(_fixture.AssetEntities);
        await ctx.SaveChangesAsync(ct);

        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        //Act
        var result = await service.AnyOtherWithIsinAsync("NONE", _fixture.ExistingIsin, ct);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenIsinExistsOnlyOnSameSymbol_WhenCallingAnyOtherWithIsinAsync_ReturnsFalse()
    {
        // Arrange
        var ct = CancellationToken.None;

        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();

        ctx.Assets.Add(_fixture.NewAsset);

        await ctx.SaveChangesAsync(ct);

        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        // Act
        var result = await service.AnyOtherWithIsinAsync(_fixture.MSTFExistingIsin, excludingSymbol: "MSFT", ct);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenIsinDoesNotExist_WhenCallingAnyOtherWithIsinAsync_ReturnsFalse()
    {
        // Arrange
        var ct = CancellationToken.None;

        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();

        ctx.Assets.AddRange(_fixture.AssetEntities);

        await ctx.SaveChangesAsync(ct);

        var service = new AssetService(ctx, _fixture.MockMapper.Object);
        //var repo = new AssetRepo(ctx);

        // Act
        var result = await service.AnyOtherWithIsinAsync("US0000000000", excludingSymbol: "MSFT", ct);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GivenIsinWithSpacesAndLowercase_WhenCallingAnyOtherWithIsinAsync_StillMatches()
    {
        // Arrange
        var ct = CancellationToken.None;

        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();


        ctx.Assets.AddRange(_fixture.AssetEntities);

        await ctx.SaveChangesAsync(ct);

        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        // Act
        var result = await service.AnyOtherWithIsinAsync("  us5949181045  ", excludingSymbol: "aapl", ct);

        // Assert
        Assert.True(result);
    }


    [Fact]
    public async Task GivenSymbolAndCancellationToken_WhenCallingGetBySymbolAsyncAndExists_ThenAssetReturns()
    {
        //Arrange
        var ct = CancellationToken.None;
        var expectedResult = _fixture.AssetEntities.FirstOrDefault(x => x.Symbol == _fixture.Symbol);
        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();
        ctx.Assets.AddRange(_fixture.AssetEntities);
        await ctx.SaveChangesAsync(ct);

        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        //Act
        var result = await service.GetBySymbolAsync(_fixture.Symbol, ct);

        //Assert
        result.Should().BeOfType<Asset>();
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GivenSymbolAndIsin_WhenUpsertingAndIsinExistsOnAnotherAsset_ThenConflictExceptionIsThrown()
    {
        // Arrange
        var ct = CancellationToken.None;

        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();

        // Seed: an existing asset already using the ISIN
        ctx.AddRange(_fixture.AssetEntities);

        await ctx.SaveChangesAsync(ct);

        // We are trying to upsert a DIFFERENT symbol with the SAME ISIN => should conflict
        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        // Act
        var act = async () => await service.UpsertAsync("NONE", _fixture.UpsertAssetRequestDto, ct);

        // Assert
        var ex = await Assert.ThrowsAsync<ConflictException>(act);
        ex.Should().NotBeNull();
        ex.Isin.Should().Be(_fixture.ExistingIsin);
        ex.Message.Should().Contain("already exists");

        _fixture.MockMapper.Verify(m => m.Map<AssetDto>(It.IsAny<Asset>()), Times.Never);
    }

    [Fact]
    public async Task GivenSymbolAndIsin_WhenUpsertAndIsinExistsOnAnotherAsset_ThenConflictExceptionThrown()
    {
        // Arrange
        var ct = CancellationToken.None;

        await using var scope = await _fixture.SqliteInMemoryDbFactory.CreateScopeAsync();
        await using var ctx = scope.CreateDbContext();

        ctx.Assets.AddRange(_fixture.AssetEntities);
        await ctx.SaveChangesAsync(ct);

        var request = new UpsertAssetRequestDto
        {
            Name = "Whatever",
            Isin = _fixture.ExistingIsin
        };

        var service = new AssetService(ctx, _fixture.MockMapper.Object);

        // Act
        var act = async () => await service.UpsertAsync("NONE", request, ct);

        // Assert
        await Assert.ThrowsAsync<ConflictException>(act);
    }


    public void Dispose()
    {
        _fixture.ClearAll();
        GC.SuppressFinalize(this);
    }
}   
