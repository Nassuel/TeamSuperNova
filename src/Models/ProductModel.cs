using ContosoCrafts.WebSite.Enums;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{

    /// <summary>
    /// Model representing a product with its details and ratings
    /// </summary>
    public class ProductModel
    {

        // Unique identifier for the product
        public string Id { get; set; }

        // Array of ratings given to the product
        public int[] Ratings { get; set; }

        // Brand name of the product
        public string Brand { get; set; }

        // Name of the product
        public string ProductName { get; set; }

        // Type category of the product
        public ProductTypeEnum ProductType { get; set; }

        // URL link to the product
        public string Url { get; set; }

        // Description of the product
        public string ProductDescription { get; set; }

        // Image URL for the product
        public string Image { get; set; }

        // Comments associated with the product
        public List<CommentModel> CommentList { get; set; }

        /// <summary>
        /// Converts the ProductModel to a JSON string representation
        /// </summary>
        /// <returns>JSON string of the product model</returns>
        public override string ToString()
        {

            // Serialize the product model to JSON format
            return JsonSerializer.Serialize<ProductModel>(this);

        }

    }

}