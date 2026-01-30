namespace PlatformOne.Assets.Shared.DtoModels;


/// <summary>
/// Represents the base data transfer object for API operation results, providing a standard structure for indicating
/// success or failure and conveying error details.
/// </summary>
/// <remarks>Use this class as a base for API response models to ensure consistent handling of operation outcomes.
/// The Success property indicates whether the operation completed successfully. If the operation fails, the Error
/// property contains details about the failure. The Fail method can be used to create a failed result with the
/// specified error information.</remarks>
public class ApiResultBaseDto
{
    /// <summary>
    /// Gets a value indicating whether the operation completed successfully.
    /// </summary>
    /// <remarks>This property is set to <see langword="true"/> if the operation finished without errors;
    /// otherwise, it is <see langword="false"/>. Check this property to determine the outcome of the operation before
    /// proceeding with further actions.</remarks>
    public bool Success { get; init; }

    /// <summary>
    /// Gets the detailed success message that provides additional context about the operation's successful outcome.
    /// </summary>
    /// <remarks>This property is initialized to an empty string. Use this property to convey specific
    /// information about the success of an operation to the caller, such as additional details or user-facing
    /// messages.</remarks>
    public string DetailedSuccessMessage { get; init; } = string.Empty;

    /// <summary>
    /// Gets the error details associated with the API response, if any.
    /// </summary>
    /// <remarks>This property may contain additional information about an error that occurred during the API
    /// operation. If no error occurred, this property will be null.</remarks>
    public ApiErrorDto? Error { get; init; }


    /// <summary>
    /// Creates a new instance of the ApiResultBaseDto class that represents a failed operation with the specified error
    /// details.
    /// </summary>
    /// <remarks>Use this method to standardize error responses in API operations, ensuring consistent
    /// handling of failure cases across the application.</remarks>
    /// <param name="error">The error details that describe the reason for the failure. This parameter cannot be null.</param>
    /// <returns>An ApiResultBaseDto object with the Success property set to <see langword="false"/> and the Error property
    /// populated with the provided error details.</returns>
    public static ApiResultBaseDto Fail(ApiErrorDto error)
        => new()
        {
            Success = false,
            Error = error
        };
}

