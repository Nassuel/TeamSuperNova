using Microsoft.AspNetCore.Http;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Model for the product form partial view containing product data and placeholder text
    /// </summary>
    public class ProductFormModel
    {
        // The product data being created or updated
        public ProductModel Product { get; set; }

        // The uploaded image file for the product
        public IFormFile ImageFile { get; set; }

        // Placeholder text for the brand input field
        public string BrandPlaceholder { get; set; }

        // Placeholder text for the product name input field
        public string ProductNamePlaceholder { get; set; }

        // Placeholder text for the description textarea
        public string DescriptionPlaceholder { get; set; }

        // Placeholder text for the URL input field
        public string UrlPlaceholder { get; set; }

    }

}