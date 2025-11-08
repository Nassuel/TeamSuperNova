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
        public ProductType Type { get; set; }
        public string Url { get; set; }
        public string ProductDescription { get; set; }
        public string Image { get; set; }

        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);
    }
}