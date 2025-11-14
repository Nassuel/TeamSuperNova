using System;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{

    /// <summary>
    /// Page model for creating a new product
    /// </summary>
    public class CreateModel : PageModel
    {

        // Service for handling product data operations
        public JsonFileProductService ProductService { get; set; }

        // The product data to create, bound for post
        [BindProperty]
        public ProductModel Product { get; set; }

        // The uploaded image file for the product
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        /// <summary>
        /// Constructor to initialize the CreateModel with product service
        /// </summary>
        /// <param name="productService">Service for product data operations</param>
        public CreateModel(JsonFileProductService productService)
        {

            ProductService = productService;

        }

        /// <summary>
        /// Handles GET request to display the create product page
        /// </summary>
        /// <returns>Page result or redirect to index</returns>
        public IActionResult OnGet()
        {

            // Initialize new product with default values
            Product = new ProductModel
            {
                Id = Guid.NewGuid().ToString(),
                Brand = "",
                ProductDescription = "",
                Url = "",
                Image = "",
                Ratings = null,
                ProductName = "",
                ProductType = 0
            };

            // Fast fail: Check if product initialization failed
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            return Page();

        }

        /// <summary>
        /// Handles POST request to create a new product
        /// Validates the model and saves the product with uploaded image
        /// </summary>
        /// <returns>Redirect to product read page or current page if validation fails</returns>
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

            // Save uploaded image file and get path
            Product.Image = await ProductService.SaveUploadedFileAsync(ImageFile);

            // Log product details to console
            Console.WriteLine(Product);

            // Add new product to service
            ProductService.AddCategory(Product);

            // Redirect to read page for the new product
            return RedirectToPage("/Product/Read", new { id = Product.Id });

        }

    }

}