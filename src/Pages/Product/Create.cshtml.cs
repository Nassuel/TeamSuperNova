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

        // The form model containing product data and placeholders, bound for post
        [BindProperty]
        public ProductFormModel FormModel { get; set; }

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
            var product = new ProductModel
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

            // Initialize form model with product data and create placeholders
            FormModel = new ProductFormModel
            {
                Product = product,
                ImageFile = null,
                BrandPlaceholder = "Enter the brand of new product",
                ProductNamePlaceholder = "Enter the new product name",
                DescriptionPlaceholder = "Enter Description Here",
                UrlPlaceholder = "Enter URL of Product Website"
            };

            return Page();

        }

        /// <summary>
        /// Handles POST request to create a new product
        /// Validates the model and saves the product with uploaded image
        /// </summary>
        /// <returns>Redirect to product read page or current page if validation fails</returns>
        public async Task<IActionResult> OnPostAsync()
        {

            // Fast fail: Check if form model is null
            if (FormModel == null)
            {
                return RedirectToPage("/Product/Index");
            }

            // Fast fail: Check if product is null
            if (FormModel.Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            // Fast fail: Check if model state is invalid
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            // Save uploaded image file only if a new file was provided
            if (FormModel.ImageFile != null)
            {
                FormModel.Product.Image = await ProductService.SaveUploadedFileAsync(FormModel.ImageFile);
            }

            // Log product details to console
            Console.WriteLine(FormModel.Product);

            // Add new product to service
            ProductService.AddCategory(FormModel.Product);

            // Redirect to read page for the new product
            return RedirectToPage("/Product/Read", new { id = FormModel.Product.Id });

        }

    }

}