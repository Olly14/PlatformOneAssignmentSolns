namespace PlatformOne.Assets.Api.tests.Assertions;

public sealed class ActionResultAsserter : IActionResultAsserter
{
    /* -------------------------------------------------
     * OK (200) — ApiResult success
     * ------------------------------------------------- */

    public ApiResultDto<T> AssertOkResult<T>(ActionResult<ApiResultDto<T>> result)
    {
        result.Should().NotBeNull();

        var ok = result.Result.Should().BeOfType<OkObjectResult>().Which;
        ok.Value.Should().BeOfType<ApiResultDto<T>>();

        var apiResult = (ApiResultDto<T>)ok.Value!;

        using (new AssertionScope())
        {
            apiResult.Success.Should().BeTrue();
            apiResult.Error.Should().BeNull();
        }

        return apiResult;
    }

    /* -------------------------------------------------
     * OK (200) — command (no data)
     * ------------------------------------------------- */

    public ApiResultDto<object> AssertOkCommand(ActionResult<ApiResultDto<object>> result)
    {
        var apiResult = AssertOkResult<object>(result);
        apiResult.Data.Should().BeNull();
        return apiResult;
    }

}
