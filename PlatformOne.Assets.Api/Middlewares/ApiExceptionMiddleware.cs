namespace PlatformOne.Assets.Api.Middlewares;

/// <summary>
/// Middleware that provides centralized exception handling for HTTP requests in an ASP.NET Core application.
/// </summary>
/// <remarks>ApiExceptionMiddleware intercepts exceptions thrown during request processing and translates them
/// into appropriate HTTP responses with standardized error payloads. It handles common scenarios such as validation
/// errors, missing resources, and database conflicts, logging each event and returning relevant status codes and error
/// messages. This middleware should be registered in the application's request pipeline to ensure consistent error
/// handling and logging across all endpoints.</remarks>
public sealed class ApiExceptionMiddleware : IMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(
        ILogger<ApiExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // Client disconnected or request aborted. Don't turn into 500.
            return;
        }

        // -------------------------
        // 400 - validation/inputs
        // -------------------------
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Bad request: {Message}", ex.Message);

            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                ApiErrorDto.Validation(ex.Message));
        }
        // -------------------------
        // None Existing Asset exceptions
        // -------------------------
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(
                ex,
                "Bad request: {Message}",
                ex.Message);

            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                ApiErrorDto.Validation(
                    message: "Asset with given symbol not found",
                    details: "Asset does not exit."));
        }
        // -------------------------
        // Db Conflict domain exceptions
        // -------------------------
        catch (ConflictException ex)
        {
            _logger.LogError(
                ex,
                "DATABASE UPDATE FAILURE | Isin={Isin}",
                ex.Isin);

            await WriteErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                ApiErrorDto.Validation(
                    message: "Update failure",
                    details: "Asset data may be incomplete or already in existence."));
        }
        // -------------------------
        // Default: 500
        // -------------------------
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            await WriteErrorAsync(
                context,
                StatusCodes.Status500InternalServerError,
                ApiErrorDto.Internal());
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, ApiErrorDto error)
    {
        if (context.Response.HasStarted)
            return;

        var payload = ApiResultBaseDto.Fail(error);

        context.Response.Clear();
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(payload, JsonOptions);
    }
}
