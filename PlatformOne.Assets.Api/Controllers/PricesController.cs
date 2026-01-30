namespace PlatformOne.Assets.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly IAssetPriceService _assetPriceService;

        public PricesController(IAssetPriceService assetPriceService)
        {
            _assetPriceService = assetPriceService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResultDto<GetPricesResponseDto>>> Get([FromQuery] DateOnly date, [FromQuery] string? source, string? symbols, CancellationToken ct)
        {
            IReadOnlyList<string>? symbolList = null;

            if (!string.IsNullOrWhiteSpace(symbols))
            {
                symbolList = symbols.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }

            var result = await _assetPriceService.GetPricesAsync(ct, date, source, symbolList);

            return Ok(ApiResultDto<GetPricesResponseDto>.OkWithData(result));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResultDto<UpsertPriceResponseDto>>> Upsert([FromBody] UpsertPriceRequestDto upsertPriceRequest, CancellationToken ct)
        {
            var result = await _assetPriceService.UpsertPriceAsync(ct, upsertPriceRequest);

            return Ok(ApiResultDto<UpsertPriceResponseDto>.OkWithData(result));
        }
    }
}
