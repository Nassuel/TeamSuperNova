using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{

    /// <summary>
    /// Page model for deleting a product
    /// </summary>
    public class DeleteModel : PageModel
    {

        // Service for handling product data operations
        private readonly JsonFileProductService ProductService;

        // The product data to delete, bound for post
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// Constructor to initialize the DeleteModel with product service
        /// </summary>
        /// <param name="productService">Service for product data operations</param>
        public DeleteModel(JsonFileProductService productService)
        {

            ProductService = productService;

        }

        /// <summary>
        /// Handles GET request to display the delete confirmation page
        /// </summary>
        /// <param name="id">ID of the product to delete</param>
        /// <returns>Page result or redirect to index</returns>
        public IActionResult OnGet(string id)
        {

            // Fast fail: Check if id is null or empty
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Product/Index");
            }

            // Retrieve product by ID
            Product = ProductService.GetProductById(id);

            // Fast fail: Check if product was not found
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            return Page();

        }

        /// <summary>
        /// Handles POST request to confirm and execute product deletion
        /// </summary>
        /// <returns>Redirect to product index page</returns>
        public IActionResult OnPost()
        {

            // Fast fail: Check if Product is null
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            // Fast fail: Check if Product.Id is null
            if (Product.Id == null)
            {
                return RedirectToPage("/Product/Index");
            }

            // Delete the product
            ProductService.DeleteCategory(Product.Id);

            // Redirect to index page
            return RedirectToPage("/Product/Index");

        }

    }

}