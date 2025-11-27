using System.Net;
using System.Text.RegularExpressions;

/// <summary>
/// Service for sanitizing user input to prevent XSS attacks
/// </summary>
public class InputSanitizationService
{
    /// <summary>
    /// Sanitizes comment text by encoding HTML characters
    /// </summary>
    /// <param name="input">Raw user input</param>
    /// <returns>Sanitized safe string</returns>
    public string SanitizeComment(string input)
    {
        // Fast fail for null or empty input
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Encode HTML special characters
        string sanitizedInput = WebUtility.HtmlEncode(input);

        return sanitizedInput;
    }

    /// <summary>
    /// Validates that comment length is within acceptable range
    /// </summary>
    /// <param name="input">User input to validate</param>
    /// <param name="maxLength">Maximum allowed length</param>
    /// <returns>True if valid, false otherwise</returns>
    public bool ValidateCommentLength(string input, int maxLength)
    {
        // Fast fail for null input
        if (input == null)
        {
            return false;
        }

        // Check length is within bounds
        bool isWithinMaxLength = input.Length <= maxLength;
        bool isNotEmpty = input.Length > 0;

        if (isWithinMaxLength == false)
        {
            return false;
        }

        if (isNotEmpty == false)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Removes dangerous patterns from input as additional safety layer
    /// </summary>
    /// <param name="input">User input</param>
    /// <returns>Input with dangerous patterns removed</returns>
    public string RemoveDangerousPatterns(string input)
    {
        // Fast fail for null or empty
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Remove script tags
        string cleanedInput = Regex.Replace(input, @"<script[^>]*>.*?</script>", string.Empty, RegexOptions.IgnoreCase);

        // Remove javascript: protocol
        cleanedInput = Regex.Replace(cleanedInput, @"javascript:", string.Empty, RegexOptions.IgnoreCase);

        // Remove on* event handlers
        cleanedInput = Regex.Replace(cleanedInput, @"on\w+\s*=", string.Empty, RegexOptions.IgnoreCase);

        return cleanedInput;
    }
}