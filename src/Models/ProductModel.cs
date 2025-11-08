using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public int[] Ratings { get; set; }
        public string Brand { get; set; }
        public string ProductName { get; set; }
        public string Type { get; set; }
        public string url { get; set; }
        public string ProductDescription { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Image { get; set; }

        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);
    }
}