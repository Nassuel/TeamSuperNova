using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{

    /// <summary>
    /// Page model for reading and displaying a single product
    /// </summary>
    public class ReadModel : PageModel
    {

        // Service for handling product data operations
        public JsonFileProductService ProductService { get; }

        // The product data to display
        public ProductModel Product;

        /// <summary>
        /// Constructor to initialize the ReadModel with product service
        /// </summary>
        /// <param name="productService">Service for product data operations</param>
        public ReadModel(JsonFileProductService productService)
        {

            ProductService = productService;

        }

        /// <summary>
        /// Handles GET request to retrieve and display a specific product
        /// </summary>
        /// <param name="id">ID of the product to retrieve</param>
        /// <returns>Page result or redirect to index if product not found</returns>
        public IActionResult OnGet(string id)
        {

            // Fast fail: Check if id is null or empty
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("./Index");
            }

            // Retrieve product by ID
            Product = ProductService.GetProducts().FirstOrDefault(m => m.Id.Equals(id));

            // Fast fail: Check if product was not found
            if (Product == null)
            {
                return RedirectToPage("./Index");
            }

            return Page();

        }

    }

}