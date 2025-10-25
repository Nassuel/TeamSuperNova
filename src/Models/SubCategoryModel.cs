namespace ContosoCrafts.WebSite.Models
{
    public class SubCategoryModel
    {
        public string Id { get; set; }
        public string Brand { get; set; }
        public string ProductName { get; set; }
        public string url { get; set; }
        public string ProductDescription { get; set; }

        /// <summary>
        /// Adding these 2 for subcategories
        /// </summary>
        public string Category { get; set; }      // e.g. "Laptop"
        public string SubCategory { get; set; }   // e.g. "Dell", "HP"
        public string Image { get; set; }
    }
}
