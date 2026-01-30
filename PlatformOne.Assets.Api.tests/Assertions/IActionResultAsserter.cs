namespace PlatformOne.Assets.Api.tests.Assertions;

public interface IActionResultAsserter
{
    /*ApiResultDto<T> AssertBadRequest<T>(ActionResult<ApiResultDto<T>> result, string? expectedErrorCode = "VALIDATION_ERROR");
    ApiResultDto<T> AssertNotFound<T>(ActionResult<ApiResultDto<T>> result, string? expectedErrorCode = "NOT_FOUND");*/
    ApiResultDto<object> AssertOkCommand(ActionResult<ApiResultDto<object>> result);
    ApiResultDto<T> AssertOkResult<T>(ActionResult<ApiResultDto<T>> result);
    /*ApiResultDto<T> AssertStatus<T>(ActionResult<ApiResultDto<T>> result, int expectedStatusCode);
    ApiResultDto<T> AssertCreatedAtAction<T>(ActionResult<ApiResultDto<T>> result, string expectedActionName, string expectedRouteIdKey = "id", object? expectedRouteIdValue = null);
    public void AssertStatus(IActionResult result, int expectedStatusCode);
    public void AssertOk(IActionResult result);
    public void AssertBadRequest(IActionResult result);
    public void AssertNotFound(IActionResult result);
    string AssertRedirect(IActionResult result);*/
}
