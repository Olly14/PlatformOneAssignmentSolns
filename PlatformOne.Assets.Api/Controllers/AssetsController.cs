namespace PlatformOne.Assets.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    //private readonly
    private readonly IAssetService _assetService;

    public AssetsController(IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResultDto<IReadOnlyList<AssetDto>>>> Get([FromQuery] string? symbol, [FromQuery] string? isin, CancellationToken ct)
    {
        var response = await _assetService.GetAssetsAsync(ct, symbol, isin);

        return Ok(ApiResultDto<IReadOnlyList<AssetDto>>.OkWithData(response));
    }

    [HttpPut("{symbol}")]
    public async Task<ActionResult<ApiResultDto<AssetDto>>> Upsert([FromRoute] string symbol, [FromBody] UpsertAssetRequestDto upsertAssetRequestDto, CancellationToken ct)
    {
        var response = await _assetService.UpsertAsync(symbol, upsertAssetRequestDto, ct);
        return Ok(ApiResultDto<AssetDto>.OkWithData(response));
    }
}
