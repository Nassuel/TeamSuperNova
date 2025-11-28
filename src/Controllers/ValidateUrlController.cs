using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSite.Controllers
{

    /// <summary>
    /// API controller for validating URLs
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ValidateUrlController : ControllerBase
    {

        // HTTP client factory for making requests
        private readonly IHttpClientFactory HttpClientFactory;

        /// <summary>
        /// Constructor to initialize the controller with HTTP client factory
        /// </summary>
        /// <param name="httpClientFactory">Factory for creating HTTP clients</param>
        public ValidateUrlController(IHttpClientFactory httpClientFactory)
        {

            HttpClientFactory = httpClientFactory;

        }

        /// <summary>
        /// Validates a URL by making a HEAD request and checking the response status
        /// </summary>
        /// <param name="url">The URL to validate</param>
        /// <returns>JSON result indicating if URL is valid</returns>
        [HttpGet]
        public async Task<IActionResult> OnGet([FromQuery] string url)
        {

            // Fast fail: Check if URL is null or empty
            if (string.IsNullOrEmpty(url))
            {
                return Ok(new UrlValidationResultModel
                {
                    IsValid = false,
                    StatusCode = 0,
                    Message = "URL is required"
                });
            }

            // Try to parse URL
            var uriParseSuccessful = Uri.TryCreate(url, UriKind.Absolute, out var uri);

            // Fast fail: Check if URL format is invalid
            if (uriParseSuccessful == false)
            {
                return Ok(new UrlValidationResultModel
                {
                    IsValid = false,
                    StatusCode = 0,
                    Message = "Invalid URL format"
                });
            }

            // Fast fail: Check if URL scheme is HTTP
            if (uri.Scheme == Uri.UriSchemeHttp)
            {
                return await ValidateUrlAsync(uri);
            }

            // Fast fail: Check if URL scheme is HTTPS
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                return await ValidateUrlAsync(uri);
            }

            // URL scheme is neither HTTP nor HTTPS
            return Ok(new UrlValidationResultModel
            {
                IsValid = false,
                StatusCode = 0,
                Message = "URL must use HTTP or HTTPS"
            });

        }

        /// <summary>
        /// Validates URL by making HTTP HEAD request
        /// </summary>
        /// <param name="uri">URI to validate</param>
        /// <returns>Validation result</returns>
        private async Task<IActionResult> ValidateUrlAsync(Uri uri)
        {

            // Create HTTP client
            var client = HttpClientFactory.CreateClient();

            // Set timeout to 10 seconds
            client.Timeout = TimeSpan.FromSeconds(10);

            // Make HEAD request to check URL
            var request = new HttpRequestMessage(HttpMethod.Head, uri);

            HttpResponseMessage response = null;

            try
            {

                // Send request
                response = await client.SendAsync(request);

                // Get status code
                var statusCode = (int)response.StatusCode;

                // Check if status code is in success range
                var isValid = IsSuccessStatusCode(statusCode);

                // Determine message
                var message = GetValidationMessage(isValid);

                return Ok(new UrlValidationResultModel
                {
                    IsValid = isValid,
                    StatusCode = statusCode,
                    Message = message
                });

            }
            catch (TaskCanceledException)
            {

                // Request timed out
                return Ok(new UrlValidationResultModel
                {
                    IsValid = false,
                    StatusCode = 0,
                    Message = "Request timed out"
                });

            }
            catch (HttpRequestException)
            {

                // Request failed
                return Ok(new UrlValidationResultModel
                {
                    IsValid = false,
                    StatusCode = 0,
                    Message = "Unable to reach URL"
                });

            }

        }

        /// <summary>
        /// Checks if status code indicates success (2xx range)
        /// </summary>
        /// <param name="statusCode">HTTP status code</param>
        /// <returns>True if success, false otherwise</returns>
        private bool IsSuccessStatusCode(int statusCode)
        {

            // Fast fail: Check if status code is less than 200
            if (statusCode < 200)
            {
                return false;
            }

            // Fast fail: Check if status code is 300 or greater
            if (statusCode >= 300)
            {
                return false;
            }

            // Status code is in success range
            return true;

        }

        /// <summary>
        /// Gets validation message based on validity
        /// </summary>
        /// <param name="isValid">Whether URL is valid</param>
        /// <returns>Validation message</returns>
        private string GetValidationMessage(bool isValid)
        {

            // Fast fail: Check if valid
            if (isValid)
            {
                return "URL is valid";
            }

            // URL is not valid
            return "URL returned non-success status";

        }

    }

}