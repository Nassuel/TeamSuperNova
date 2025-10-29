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

        // Bind properties for create/edit forms
        [BindProperty]
        public SubCategoryInputModel NewSub { get; set; } = new();

        [BindProperty]
        public IFormFile NewImageFile { get; set; }
        
        // Image upload / URL for the *new parent category* when creating a category together with a sub
        [BindProperty]
        public IFormFile NewCategoryImageFile { get; set; }

        [BindProperty]
        public SubCategoryInputModel EditSub { get; set; } = new();

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

        public async Task<IActionResult> OnPostCreateSubcategoryAsync()
        {
            Products = ProductService.GetProducts();
            ShowCreateForm = true;

            if (string.IsNullOrWhiteSpace(NewSub?.ProductName))
            {
                ModelState.AddModelError("NewSub.ProductName", "Product name is required.");
            }

            if (string.IsNullOrWhiteSpace(NewSub?.ParentProductId) && string.IsNullOrWhiteSpace(NewSub?.NewCategoryTitle))
            {
                ModelState.AddModelError("NewSub.ParentProductId", "Select an existing category or create a new one.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // sub image (URL or upload)
                string subImagePath = null;
                if (NewImageFile != null && NewImageFile.Length > 0)
                {
                    subImagePath = await SaveUploadedFileAsync(NewImageFile);
                }
                else if (!string.IsNullOrWhiteSpace(NewSub.ImageUrl))
                {
                    subImagePath = NewSub.ImageUrl;
                }

                // If creating a new category at the same time, allow separate category image (URL or upload)
                string categoryImagePath = null;
                if (NewCategoryImageFile != null && NewCategoryImageFile.Length > 0)
                {
                    categoryImagePath = await SaveUploadedFileAsync(NewCategoryImageFile);
                }
                else if (!string.IsNullOrWhiteSpace(NewSub.NewCategoryImageUrl))
                {
                    categoryImagePath = NewSub.NewCategoryImageUrl;
                }

                var sub = new SubCategoryModel
                {
                    Brand = NewSub.Brand,
                    ProductName = NewSub.ProductName,
                    ProductDescription = NewSub.ProductDescription,
                    url = NewSub.Url,
                    Image = subImagePath,
                    Category = NewSub.ParentProductId == "__new__" ? NewSub.NewCategoryTitle : Products.FirstOrDefault(p => p.Id == NewSub.ParentProductId)?.Title
                };

                if (NewSub.ParentProductId == "__new__")
                {
                    var newProdId = string.IsNullOrWhiteSpace(NewSub.NewCategoryId) ? MakeSafeId(NewSub.NewCategoryTitle) : NewSub.NewCategoryId;
                    var newProduct = new ProductModel
                    {
                        Id = newProdId,
                        Maker = "admin",
                        // prefer explicit category image; fallback to the sub image only if no category image provided
                        Image = categoryImagePath ?? subImagePath ?? "/assets/default_category.png",
                        Url = "",
                        Title = NewSub.NewCategoryTitle,
                        Description = NewSub.NewCategoryDescription ?? "",
                        Ratings = null,
                        SubCategories = new List<SubCategoryModel> { sub }
                    };

                    var ok = ProductService.AddCategory(newProduct);
                    if (!ok)
                    {
                        ModelState.AddModelError("", "Failed to create category (an item with the same id may already exist).");
                        return Page();
                    }
                }
                else
                {
                    var ok = ProductService.AddSubCategory(NewSub.ParentProductId, sub);
                    if (!ok)
                    {
                        ModelState.AddModelError("", "Failed to add subcategory (parent not found).");
                        return Page();
                    }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostEditSubcategoryAsync()
        {
            Products = ProductService.GetProducts();

            if (string.IsNullOrWhiteSpace(EditSub?.ParentProductId) || string.IsNullOrWhiteSpace(EditSub?.Id))
            {
                ModelState.AddModelError("", "Invalid edit request.");
                return Page();
            }

            ShowEditProductId = EditSub.ParentProductId;
            ShowEditSubId = EditSub.Id;

            try
            {
                string imagePath = EditSub.ImageUrl; // keep existing image by default

                if (EditImageFile != null && EditImageFile.Length > 0)
                {
                    imagePath = await SaveUploadedFileAsync(EditImageFile);
                }

                var updated = new SubCategoryModel
                {
                    Id = EditSub.Id,
                    Brand = EditSub.Brand,
                    ProductName = EditSub.ProductName,
                    ProductDescription = EditSub.ProductDescription,
                    url = EditSub.Url,
                    Image = imagePath,
                    Category = Products.FirstOrDefault(p => p.Id == EditSub.ParentProductId)?.Title
                };

                var ok = ProductService.UpdateSubCategory(EditSub.ParentProductId, updated);
                if (!ok)
                {
                    ModelState.AddModelError("", "Failed to update subcategory.");
                    return Page();
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
                return Page();
            }
        }

        public IActionResult OnPostDeleteSubcategory(string productId, string subId)
        {
            Products = ProductService.GetProducts();

            if (string.IsNullOrWhiteSpace(productId) || string.IsNullOrWhiteSpace(subId))
            {
                ModelState.AddModelError("", "Invalid delete request.");
                return Page();
            }

            ShowDeleteProductId = productId;
            ShowDeleteSubId = subId;

            try
            {
                var ok = ProductService.DeleteSubCategory(productId, subId);
                if (!ok)
                {
                    ModelState.AddModelError("", "Failed to delete subcategory (item not found).");
                    return Page();
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
                return Page();
            }
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

        public class SubCategoryInputModel
        {
            // For create
            public string ParentProductId { get; set; }
            public string NewCategoryId { get; set; }
            public string NewCategoryTitle { get; set; }
            public string NewCategoryDescription { get; set; }
            // NEW: allow supplying an explicit image URL for the new parent category
            public string NewCategoryImageUrl { get; set; }

            // For create/edit
            public string Id { get; set; }
            public string Brand { get; set; }
            public string ProductName { get; set; }
            public string Url { get; set; }
            public string ProductDescription { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}