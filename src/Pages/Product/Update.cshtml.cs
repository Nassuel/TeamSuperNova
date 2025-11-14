using System.Linq;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{

    /// <summary>
    /// Page model for updating an existing product
    /// </summary>
    public class UpdateModel : PageModel
    {

        // Service for handling product data operations
        public JsonFileProductService ProductService { get; }

        // The product data to update, bound for post
        [BindProperty]
        public ProductModel Product { get; set; }

        // The uploaded image file for the product
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        /// <summary>
        /// Constructor to initialize the UpdateModel with product service
        /// </summary>
        /// <param name="productService">Service for product data operations</param>
        public UpdateModel(JsonFileProductService productService)
        {

            ProductService = productService;

        }

        /// <summary>
        /// Handles GET request to load and display product data for editing
        /// </summary>
        /// <param name="id">ID of the product to update</param>
        /// <returns>Page result or redirect to index if product not found</returns>
        public IActionResult OnGet(string id)
        {

            // Fast fail: Check if id is null or empty
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Product/Index");
            }

            // Retrieve product by ID
            Product = ProductService.GetProducts().FirstOrDefault(x => x.Id.Equals(id));

            // Fast fail: Check if product was not found
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            return Page();

        }

        /// <summary>
        /// Handles POST request to save updated product data
        /// Validates the model and updates the product with new image if provided
        /// </summary>
        /// <returns>Redirect to product index page or current page if validation fails</returns>
        public async Task<IActionResult> OnPostAsync()
        {

            // Fast fail: Check if product is null
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            // Fast fail: Check if model state is invalid
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            // Save uploaded image file and update product image path
            Product.Image = await ProductService.SaveUploadedFileAsync(ImageFile);

            // Update product data in service
            ProductService.UpdateData(Product);

            // Redirect to index page
            return RedirectToPage("/Product/Index");

        }

    }

}