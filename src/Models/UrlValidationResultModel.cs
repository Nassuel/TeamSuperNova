/// <summary>
/// Model for URL validation result
/// </summary>
public class UrlValidationResultModel
{
    // Indicates if the URL is valid and accessible
    public bool IsValid { get; set; }

    // HTTP status code returned by the URL
    public int StatusCode { get; set; }

    // Message describing the validation result
    public string Message { get; set; }
}