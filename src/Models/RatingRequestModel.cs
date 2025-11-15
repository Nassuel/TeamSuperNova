namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Model for rating request containing product ID and rating value
    /// </summary>
    public class RatingRequest
    {

        // Unique identifier for the product
        public string ProductId { get; set; }

        // Rating value for the product
        public int Rating { get; set; }

    }

}
