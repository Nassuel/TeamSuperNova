using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Components.Forms;
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
    /// <summary>
    /// Manage the Update of the data for a single record
    /// </summary>
    public class UpdateModel : PageModel
    {
        // Data middletier
        public JsonFileProductService ProductService { get; }
        private readonly IWebHostEnvironment _env;
        // public IBrowserFile file = new ();

        /// <summary>
        /// Defualt Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService"></param>
        public UpdateModel(JsonFileProductService productService, IWebHostEnvironment env)
        {
            ProductService = productService;
            _env = env;
        }

        // The data to show, bind to it for the post
        [BindProperty]
        public ProductModel Product { get; set; }
        [BindProperty]
        public IFormFile NewCategoryImage { get; set; }

        /// <summary>
        /// REST Get request
        /// Loads the Data
        /// </summary>
        /// <param name="id"></param>
        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToPage("/Product/Index");
            }

            Product = ProductService.GetProducts().FirstOrDefault(x => x.Id.Equals(id));
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

            ProductService.UpdateData(Product);
            return RedirectToPage("/Product/Index");
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