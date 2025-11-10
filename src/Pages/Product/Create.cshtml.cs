using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public IFormFile NewCategoryImage { get; set; }

        private readonly IWebHostEnvironment _env;
        public CreateModel(JsonFileProductService productService, IWebHostEnvironment env)
        {
            ProductService = productService;
            _env = env;
        }

        public IActionResult OnGet()
        {
            var newProduct = new ProductModel
            {
                Id = Guid.NewGuid().ToString(),
                Brand = "New Product",
                ProductDescription = "Default description",
                Url = "Enter the Url",
                Image = "Upload Image",
                Ratings = null,
                ProductName = "New Product Name",
                ProductType = 0
            };

            //ProductService.AddCategory(newProduct);

            //return RedirectToPage("/Product/Read", new { id = newProduct.Id });

            Product = newProduct;
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
        public IActionResult OnPost()
        {
            if (Product == null)
            {
                return RedirectToPage("/Product/Index");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ProductService.AddCategory(Product);
            //return RedirectToPage("/Product/Index");

            return RedirectToPage("/Product/Read", new { id = Product.Id });
        }


        public async Task<string> SaveUploadedFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var uploads = Path.Combine(_env.WebRootPath, "assets");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"~/assets/{fileName}";
        }

    }
}


