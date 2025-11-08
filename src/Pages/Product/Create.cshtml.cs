using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateModel : PageModel
    {
        public JsonFileProductService ProductService { get; set; }
        public CreateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }
        public IActionResult OnGet()
        {
            var newProduct = new ProductModel
            {
                Id = Guid.NewGuid().ToString(),
                Brand = "New Product",
                Description = "Default description",
                Url = "Enter the Url",
                Image = "Upload Image",
            };

            ProductService.AddCategory(newProduct);

            return RedirectToPage("/Product/Update", new { id = newProduct.Id });
        }

    }
}


