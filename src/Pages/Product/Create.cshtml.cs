using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateModel : PageModel
    {
        public JsonFileProductService ProductService { get; set; }
        // The data to show, bind to it for the post
        [BindProperty]
        public ProductModel Product { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        public CreateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        public IActionResult OnGet()
        {
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

            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }

            return Page();
        }

        /// <summary>
        /// Post the model back to the page
        /// The model is in the class variable Product
        /// Call the data layer to Update that data
        /// Then return to the index page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add image path after we have awaited for file to be saved
            Product.Image = await ProductService.SaveUploadedFileAsync(ImageFile);
            Console.WriteLine(Product);

            ProductService.AddCategory(Product);

            return RedirectToPage("/Product/Read", new { id = Product.Id });
        }

    }
}


