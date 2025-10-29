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

namespace ContosoCrafts.WebSite.Pages.Admin
{
    public class SubcategoryManagerModel : PageModel
    {
        private readonly JsonFileProductService _productService;
        private readonly IWebHostEnvironment _env;

        public SubcategoryManagerModel(JsonFileProductService productService, IWebHostEnvironment env)
        {
            _productService = productService;
            _env = env;
        }

        public IEnumerable<ProductModel> Products { get; private set; }

        [BindProperty]
        public NewCategoryInput NewCategory { get; set; } = new();

        [BindProperty]
        public IFormFile NewCategoryImage { get; set; }

        [BindProperty]
        public NewSubInput NewSub { get; set; } = new();

        [BindProperty]
        public IFormFile NewSubImage { get; set; }

        // upload for the new category image when creating new category together with sub
        [BindProperty]
        public IFormFile NewSubCategoryImage { get; set; }

        [BindProperty]
        public EditCategoryInput EditCategory { get; set; }

        [BindProperty]
        public IFormFile EditCategoryImageFile { get; set; }

        [BindProperty]
        public EditSubInput EditSub { get; set; }

        [BindProperty]
        public IFormFile EditSubImageFile { get; set; }

        public void OnGet()
        {
            Products = _productService.GetProducts();
        }

        public async Task<IActionResult> OnPostCreateCategoryAsync()
        {
            Products = _productService.GetProducts();

            if (string.IsNullOrWhiteSpace(NewCategory.Title))
            {
                ModelState.AddModelError("NewCategory.Title", "Title is required.");
                return Page();
            }

            try
            {
                string imagePath = null;
                if (NewCategoryImage != null && NewCategoryImage.Length > 0)
                {
                    imagePath = await SaveUploadedFileAsync(NewCategoryImage);
                }
                else if (!string.IsNullOrWhiteSpace(NewCategory.ImageUrl))
                {
                    imagePath = NewCategory.ImageUrl;
                }

                var id = string.IsNullOrWhiteSpace(NewCategory.Id) ? MakeSafeId(NewCategory.Title) : NewCategory.Id;

                var product = new ProductModel
                {
                    Id = id,
                    Maker = "admin",
                    Image = imagePath ?? "/assets/default_category.png",
                    Url = NewCategory.Url ?? "",
                    Title = NewCategory.Title,
                    Description = NewCategory.Description ?? "",
                    Ratings = null,
                    SubCategories = new List<SubCategoryModel>()
                };

                var ok = _productService.AddCategory(product);
                if (!ok)
                {
                    ModelState.AddModelError("", "Failed to add category (id may already exist).");
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

        public async Task<IActionResult> OnPostCreateSubcategoryAsync()
        {
            Products = _productService.GetProducts();

            if (string.IsNullOrWhiteSpace(NewSub.ProductName))
            {
                ModelState.AddModelError("NewSub.ProductName", "Product name is required.");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(NewSub.ParentProductId) && string.IsNullOrWhiteSpace(NewSub.NewCategoryTitle))
            {
                ModelState.AddModelError("NewSub.ParentProductId", "Choose parent category or create new one.");
                return Page();
            }

            try
            {
                // sub image (URL or upload)
                string subImagePath = null;
                if (NewSubImage != null && NewSubImage.Length > 0)
                    subImagePath = await SaveUploadedFileAsync(NewSubImage);
                else if (!string.IsNullOrWhiteSpace(NewSub.ImageUrl))
                    subImagePath = NewSub.ImageUrl;

                // new category image (URL or upload) when creating a new category at the same time
                string categoryImagePath = null;
                if (NewSubCategoryImage != null && NewSubCategoryImage.Length > 0)
                    categoryImagePath = await SaveUploadedFileAsync(NewSubCategoryImage);
                else if (!string.IsNullOrWhiteSpace(NewSub.NewCategoryImageUrl))
                    categoryImagePath = NewSub.NewCategoryImageUrl;

                var sub = new SubCategoryModel
                {
                    Brand = NewSub.Brand,
                    ProductName = NewSub.ProductName,
                    ProductDescription = NewSub.ProductDescription,
                    url = NewSub.Url,
                    Image = subImagePath,
                    Category = NewSub.ParentProductId == "__new__" ? NewSub.NewCategoryTitle : Products.FirstOrDefault(p => string.Equals(p.Id, NewSub.ParentProductId, StringComparison.OrdinalIgnoreCase))?.Title,
                    // ensure SubCategory is set (default to Brand)
                    SubCategory = NewSub.Brand
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

                    var ok = _productService.AddCategory(newProduct);
                    if (!ok) { ModelState.AddModelError("", "Failed to create category."); return Page(); }
                }
                else
                {
                    var ok = _productService.AddSubCategory(NewSub.ParentProductId, sub);
                    if (!ok) { ModelState.AddModelError("", "Failed to add subcategory."); return Page(); }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Unexpected error: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostEditCategoryAsync()
        {
            Products = _productService.GetProducts();

            if (string.IsNullOrWhiteSpace(EditCategory?.Id))
            {
                ModelState.AddModelError("", "Invalid request.");
                return Page();
            }

            try
            {
                var existing = _productService.GetProductById(EditCategory.Id);
                if (existing == null)
                {
                    ModelState.AddModelError("", "Category not found.");
                    return Page();
                }

                string imagePath = EditCategory.ImageUrl ?? existing.Image;
                if (EditCategoryImageFile != null && EditCategoryImageFile.Length > 0)
                {
                    imagePath = await SaveUploadedFileAsync(EditCategoryImageFile);
                }

                var updated = new ProductModel
                {
                    Id = existing.Id,
                    Maker = existing.Maker,
                    Title = EditCategory.Title ?? existing.Title,
                    Description = EditCategory.Description ?? existing.Description,
                    Image = imagePath,
                    Url = EditCategory.Url ?? existing.Url,
                    Ratings = existing.Ratings,
                    SubCategories = existing.SubCategories
                };

                var ok = _productService.UpdateCategory(updated);
                if (!ok) ModelState.AddModelError("", "Failed to update category.");

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
            Products = _productService.GetProducts();

            if (string.IsNullOrWhiteSpace(EditSub?.ParentProductId) || string.IsNullOrWhiteSpace(EditSub?.Id))
            {
                ModelState.AddModelError("", "Invalid request.");
                return Page();
            }

            try
            {
                var prod = _productService.GetProductById(EditSub.ParentProductId);
                if (prod == null) { ModelState.AddModelError("", "Parent not found."); return Page(); }

                string imagePath = EditSub.ImageUrl;
                if (EditSubImageFile != null && EditSubImageFile.Length > 0)
                {
                    imagePath = await SaveUploadedFileAsync(EditSubImageFile);
                }

                var updated = new SubCategoryModel
                {
                    Id = EditSub.Id,
                    Brand = EditSub.Brand,
                    ProductName = EditSub.ProductName,
                    ProductDescription = EditSub.ProductDescription,
                    url = EditSub.Url,
                    Image = imagePath,
                    Category = prod.Title,
                    // keep SubCategory consistent (default to Brand if missing)
                    SubCategory = EditSub.Brand
                };

                var ok = _productService.UpdateSubCategory(EditSub.ParentProductId, updated);
                if (!ok) ModelState.AddModelError("", "Failed to update subcategory.");

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
            Products = _productService.GetProducts();

            if (string.IsNullOrWhiteSpace(productId) || string.IsNullOrWhiteSpace(subId))
            {
                ModelState.AddModelError("", "Invalid delete request.");
                return Page();
            }

            var ok = _productService.DeleteSubCategory(productId, subId);
            if (!ok) ModelState.AddModelError("", "Delete failed (item not found or internal error).");

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteCategory(string productId)
        {
            Products = _productService.GetProducts();

            if (string.IsNullOrWhiteSpace(productId))
            {
                ModelState.AddModelError("", "Invalid delete request.");
                return Page();
            }

            var ok = _productService.DeleteCategory(productId);
            if (!ok) ModelState.AddModelError("", "Delete failed (item not found or internal error).");

            return RedirectToPage();
        }

        // one-off normalize helper endpoint
        public IActionResult OnPostNormalizeSubcategoryIds()
        {
            var ok = _productService.EnsureSubCategoryIds();
            if (!ok) ModelState.AddModelError("", "Normalize failed.");
            return RedirectToPage();
        }

        #region helpers

        private async Task<string> SaveUploadedFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        private string MakeSafeId(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return Guid.NewGuid().ToString("N");
            var s = text.ToLowerInvariant().Trim();
            s = Regex.Replace(s, @"\s+", "-");
            s = Regex.Replace(s, @"[^a-z0-9\-]", "");
            return s;
        }

        #endregion

        #region small DTOs

        public class NewCategoryInput
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public string Url { get; set; }
        }

        public class NewSubInput
        {
            public string ParentProductId { get; set; }
            public string NewCategoryId { get; set; }
            public string NewCategoryTitle { get; set; }
            public string NewCategoryDescription { get; set; }

            public string Id { get; set; }
            public string Brand { get; set; }
            public string ProductName { get; set; }
            public string Url { get; set; }
            public string ProductDescription { get; set; }
            public string ImageUrl { get; set; }

            // optional explicit image for the new category
            public string NewCategoryImageUrl { get; set; }
        }

        public class EditCategoryInput
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public string Url { get; set; }
        }

        public class EditSubInput
        {
            public string ParentProductId { get; set; }
            public string Id { get; set; }
            public string Brand { get; set; }
            public string ProductName { get; set; }
            public string Url { get; set; }
            public string ProductDescription { get; set; }
            public string ImageUrl { get; set; }
        }

        #endregion
    }
}