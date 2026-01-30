namespace PlatformOne.Assets.Shared.DtoModels;

public sealed class ApiResultDto<T> : ApiResultBaseDto
{
    public T? Data { get; init; }

    /* --------------------
     * Success factories
     * -------------------- */

    public static ApiResultDto<T> Ok()
        => new() { Success = true };

    public static ApiResultDto<T> OkWithData(T data)
        => new() { Success = true, Data = data };

    public static ApiResultDto<T> OkWithData(T data, string successMessage)
        => new()
        {
            Success = true,
            Data = data,
            DetailedSuccessMessage = successMessage
        };

    public static ApiResultDto<T> OkWithMessage(string successMessage)
        => new()
        {
            Success = true,
            DetailedSuccessMessage = successMessage
        };

    /* --------------------
     * Failure factory
     * -------------------- */

    public new static ApiResultDto<T> Fail(ApiErrorDto error)
        => new()
        {
            Success = false,
            Error = error
        };
}

