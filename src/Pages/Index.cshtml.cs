using System.Collections.Generic;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{

    /// <summary>
    /// Page model for the home index page
    /// </summary>
    public class IndexModel : PageModel
    {

        // Logger instance for index page logging
        private readonly ILogger<IndexModel> Logger;

        // Service for handling product data operations
        public JsonFileProductService ProductService { get; }

        // Collection of all products to display
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// Constructor to initialize the IndexModel with logger and product service
        /// </summary>
        /// <param name="logger">Logger instance for tracking</param>
        /// <param name="productService">Service for product data operations</param>
        public IndexModel(ILogger<IndexModel> logger, JsonFileProductService productService)
        {

            Logger = logger;

            ProductService = productService;

        }

        /// <summary>
        /// Handles GET request to load and display all products
        /// </summary>
        public void OnGet()
        {

            // Retrieve all products from service
            Products = ProductService.GetProducts();

        }

    }

}