using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Index Page will return all the data to show
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="env"></param>
        public IndexModel(JsonFileProductService productService, IWebHostEnvironment env)
        {
            ProductService = productService;
            WebHostEnvironment = env;
        }

        // Data Service
        public JsonFileProductService ProductService { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // Collection of the Data
        public IEnumerable<ProductModel> Products { get; private set; }

        [BindProperty]
        public IFormFile NewImageFile { get; set; }
        
        // Image upload / URL for the *new parent category* when creating a category together with a sub
        [BindProperty]
        public IFormFile NewCategoryImageFile { get; set; }

        [BindProperty]
        public IFormFile EditImageFile { get; set; }

        // UI hints to keep forms open on validation errors
        public bool ShowCreateForm { get; set; } = false;
        public string ShowEditProductId { get; set; }
        public string ShowEditSubId { get; set; }
        public string ShowDeleteProductId { get; set; }
        public string ShowDeleteSubId { get; set; }

        /// <summary>
        /// REST OnGet, return all data
        /// </summary>
        public void OnGet()
        {
            Products = ProductService.GetProducts();
        }

        private async Task<string> SaveUploadedFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploads = Path.Combine(WebHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // return relative path for UI
            return $"/uploads/{fileName}";
        }

        private string MakeSafeId(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Guid.NewGuid().ToString("N");
            }

            var s = text.ToLowerInvariant().Trim();
            s = Regex.Replace(s, @"\s+", "-");
            s = Regex.Replace(s, @"[^a-z0-9\-]", "");
            return s;
        }
    }
}