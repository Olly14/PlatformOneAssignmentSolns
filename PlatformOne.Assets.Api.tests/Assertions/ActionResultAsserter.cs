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

    /* -------------------------------------------------
     * NOT FOUND (404)
     * ------------------------------------------------- */

    /*public ApiResultDto<T> AssertNotFound<T>(
        ActionResult<ApiResultDto<T>> result,
        string? expectedErrorCode = "NOT_FOUND")
    {
        result.Should().NotBeNull();

        var notFound = result.Result.Should().BeOfType<NotFoundObjectResult>().Which;
        notFound.Value.Should().BeOfType<ApiResultDto<T>>();

        var apiResult = (ApiResultDto<T>)notFound.Value!;

        using (new AssertionScope())
        {
            apiResult.Success.Should().BeFalse();
            apiResult.Error.Should().NotBeNull();
            apiResult.Error!.Code.Should().Be(expectedErrorCode);
        }

        return apiResult;
    }*/

    /* -------------------------------------------------
     * BAD REQUEST (400)
     * ------------------------------------------------- */

    /*public ApiResultDto<T> AssertBadRequest<T>(
        ActionResult<ApiResultDto<T>> result,
        string? expectedErrorCode = "VALIDATION_ERROR")
    {
        result.Should().NotBeNull();

        var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
        badRequest.Value.Should().BeOfType<ApiResultDto<T>>();

        var apiResult = (ApiResultDto<T>)badRequest.Value!;

        using (new AssertionScope())
        {
            apiResult.Success.Should().BeFalse();
            apiResult.Error.Should().NotBeNull();
            apiResult.Error!.Code.Should().Be(expectedErrorCode);
        }

        return apiResult;
    }*/

    /* -------------------------------------------------
     * Arbitrary status (fallback)
     * ------------------------------------------------- */

    /*public ApiResultDto<T> AssertStatus<T>(
        ActionResult<ApiResultDto<T>> result,
        int expectedStatusCode)
    {
        result.Should().NotBeNull();

        var objectResult = result.Result.Should().BeOfType<ObjectResult>().Which;
        objectResult.StatusCode.Should().Be(expectedStatusCode);
        objectResult.Value.Should().BeOfType<ApiResultDto<T>>();

        return (ApiResultDto<T>)objectResult.Value!;
    }

    public ApiResultDto<T> AssertCreatedAtAction<T>(
    ActionResult<ApiResultDto<T>> result,
    string expectedActionName,
    string expectedRouteIdKey = "id",
    object? expectedRouteIdValue = null)
    {
        result.Should().NotBeNull();

        var created = result.Result.Should().BeOfType<CreatedAtActionResult>().Which;

        // 201 Created
        created.StatusCode.Should().Be(StatusCodes.Status201Created);

        // Action name matches nameof(GetByIdAsync)
        created.ActionName.Should().Be(expectedActionName);

        // Value is ApiResultDto<T>
        created.Value.Should().BeOfType<ApiResultDto<T>>();
        var apiResult = (ApiResultDto<T>)created.Value!;

        using (new AssertionScope())
        {
            apiResult.Success.Should().BeTrue();
            apiResult.Error.Should().BeNull();
            apiResult.Data.Should().NotBeNull();
        }

        // Route values should contain id = company.CompanyId
        created.RouteValues.Should().NotBeNull();
        created.RouteValues!.Should().ContainKey(expectedRouteIdKey);

        if (expectedRouteIdValue is not null)
            created.RouteValues![expectedRouteIdKey].Should().Be(expectedRouteIdValue);

        return apiResult;
    }


    public void AssertOk(IActionResult result)
    {
        result.Should().BeOfType<OkResult>();
    }

    public void AssertBadRequest(IActionResult result)
    {
        result.Should().BeOfType<BadRequestResult>();
    }


    public void AssertStatus(IActionResult result, int expectedStatusCode)
    {
        result.Should().NotBeNull();

        switch (result)
        {
            case StatusCodeResult statusCodeResult:
                statusCodeResult.StatusCode.Should().Be(expectedStatusCode);
                break;

            case ObjectResult objectResult:
                objectResult.StatusCode.Should().Be(expectedStatusCode);
                break;

            default:
                throw new AssertionFailedException(
                    $"Expected HTTP {expectedStatusCode}, but received result type '{result.GetType().Name}'.");
        }
    }


    /// <summary>
    /// Asserts that the result is a RedirectResult and returns the redirect URL.
    /// </summary>
    public string AssertRedirect(IActionResult result)
    {
        result.Should().NotBeNull();

        var redirect = result.Should().BeOfType<RedirectResult>().Which;
        redirect.Url.Should().NotBeNullOrWhiteSpace();

        return redirect.Url!;
    }

    /// <summary>
    /// Asserts redirect and verifies the URL matches exactly.
    /// </summary>
    public void AssertRedirectTo(IActionResult result, string expectedUrl)
    {
        var url = AssertRedirect(result);
        url.Should().Be(expectedUrl);
    }

    /// <summary>
    /// Asserts that the result is a permanent redirect (301).
    /// </summary>
    public string AssertPermanentRedirect(IActionResult result)
    {
        result.Should().NotBeNull();

        var redirect = result.Should().BeOfType<RedirectResult>().Which;
        redirect.Permanent.Should().BeTrue("expected a permanent (301) redirect");
        redirect.Url.Should().NotBeNullOrWhiteSpace();

        return redirect.Url!;
    }

    public void AssertNotFound(IActionResult result)
    {
        result.Should().NotBeNull();

        result.Should().Match<IActionResult>(r =>
            r is NotFoundResult ||
            (r is ObjectResult && ((ObjectResult)r).StatusCode == StatusCodes.Status404NotFound),
            "because a 404 Not Found result was expected");
    }*/

}
