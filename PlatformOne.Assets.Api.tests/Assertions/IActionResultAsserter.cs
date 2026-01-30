namespace PlatformOne.Assets.Api.tests.Assertions;

public interface IActionResultAsserter
{
    ApiResultDto<object> AssertOkCommand(ActionResult<ApiResultDto<object>> result);
    ApiResultDto<T> AssertOkResult<T>(ActionResult<ApiResultDto<T>> result);
}
