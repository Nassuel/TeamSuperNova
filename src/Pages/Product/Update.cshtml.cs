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

        // The form model containing product data and placeholders, bound for post
        [BindProperty]
        public ProductFormModel FormModel { get; set; }

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
            var product = ProductService.GetProducts().FirstOrDefault(x => x.Id.Equals(id));

            // Fast fail: Check if product was not found
            if (product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            // Initialize form model with product data and update placeholders
            FormModel = new ProductFormModel
            {
                Product = product,
                ImageFile = null,
                BrandPlaceholder = "Update the brand of product",
                ProductNamePlaceholder = "Update the product name",
                DescriptionPlaceholder = "Update description here",
                UrlPlaceholder = "Update URL of product website"
            };

            return Page();

        }

        /// <summary>
        /// Handles POST request to save updated product data
        /// Validates the model and updates the product with new image if provided
        /// </summary>
        /// <returns>Redirect to product index page or current page if validation fails</returns>
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

            // Update product data in service
            ProductService.UpdateData(FormModel.Product);

            // Redirect to index page
            return RedirectToPage("/Product/Index");

        }

    }

}