using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{

    /// <summary>
    /// Page model for displaying error information
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {

        // Logger instance for error logging
        private readonly ILogger<ErrorModel> Logger;

        // Unique identifier for the current request
        public string RequestId { get; set; }

        // Indicates whether to display the request ID
        public bool ShowRequestId { get; set; }

        /// <summary>
        /// Constructor to initialize the ErrorModel with logger
        /// </summary>
        /// <param name="logger">Logger instance for error tracking</param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {

            Logger = logger;

        }

        /// <summary>
        /// Handles GET request to display error page and capture request information
        /// </summary>
        public void OnGet()
        {

            // Get current activity ID if available
            var currentActivityId = Activity.Current?.Id;

            // Set RequestId to activity ID or fallback to trace identifier
            RequestId = currentActivityId;

            if (string.IsNullOrEmpty(RequestId))
            {
                RequestId = HttpContext.TraceIdentifier;
            }

            // Determine if request ID should be shown
            ShowRequestId = false;

            if (string.IsNullOrEmpty(RequestId) == false)
            {
                ShowRequestId = true;
            }

        }

    }

}