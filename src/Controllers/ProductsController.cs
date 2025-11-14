using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Controllers
{

    /// <summary>
    /// API Controller for managing product operations
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        // Service for handling product data operations
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Constructor to initialize the ProductsController with product service
        /// </summary>
        /// <param name="productService">Service for product data operations</param>
        public ProductsController(JsonFileProductService productService)
        {

            ProductService = productService;

        }

        /// <summary>
        /// HTTP GET endpoint to retrieve all products
        /// </summary>
        /// <returns>Collection of all products</returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {

            // Retrieve and return all products from service
            return ProductService.GetProducts();

        }

        /// <summary>
        /// HTTP PATCH endpoint to add a rating to a product
        /// </summary>
        /// <param name="request">Request containing product ID and rating</param>
        /// <returns>OK result if successful</returns>
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {

            // Fast fail: Check if request is null
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            // Fast fail: Check if ProductId is null or empty
            if (string.IsNullOrWhiteSpace(request.ProductId))
            {
                return BadRequest("ProductId cannot be empty");
            }

            // Fast fail: Check if Rating is within valid range
            if (request.Rating < 1)
            {
                return BadRequest("Rating must be at least 1");
            }

            if (request.Rating > 5)
            {
                return BadRequest("Rating must be at most 5");
            }

            // Add rating to the product
            ProductService.AddRating(request.ProductId, request.Rating);

            return Ok();

        }

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

}