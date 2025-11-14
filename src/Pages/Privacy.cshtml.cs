using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{

    /// <summary>
    /// Page model for the Privacy page
    /// </summary>
    public class PrivacyModel : PageModel
    {

        // Logger instance for privacy page logging
        private readonly ILogger<PrivacyModel> Logger;

        /// <summary>
        /// Constructor to initialize the PrivacyModel with logger
        /// </summary>
        /// <param name="logger">Logger instance for tracking</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {

            Logger = logger;

        }

        /// <summary>
        /// Handles GET request to display the Privacy page
        /// </summary>
        public void OnGet()
        {

        }

    }

}