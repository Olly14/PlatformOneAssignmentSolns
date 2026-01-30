namespace PlatformOne.Assets.Shared.DtoModels;

public class ApiErrorDto
{
    public string Code { get; init; } = default!;
    public string Message { get; init; } = default!;
    public string? Details { get; init; }

    /* --------------------
     * Factory helpers
     * -------------------- */

    public static ApiErrorDto Validation(string message, string? details = null)
        => new()
        {
            Code = "VALIDATION_ERROR",
            Message = message,
            Details = details
        };

    public static ApiErrorDto Internal(string message = "An unexpected error occurred", string? details = null)
        => new()
        {
            Code = "INTERNAL_ERROR",
            Message = message,
            Details = details
        };
}
