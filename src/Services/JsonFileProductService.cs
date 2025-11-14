using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ContosoCrafts.WebSite.Services
{

    /// <summary>
    /// Service for managing product data stored in JSON file
    /// </summary>
    public class JsonFileProductService
    {

        // Web hosting environment for accessing file paths
        public IWebHostEnvironment WebHostEnvironment { get; }

        // Path to the JSON file containing product data
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        /// <summary>
        /// Constructor to initialize the JsonFileProductService with web host environment
        /// </summary>
        /// <param name="webHostEnvironment">Web hosting environment instance</param>
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {

            WebHostEnvironment = webHostEnvironment;

        }

        /// <summary>
        /// Retrieves all products from the JSON file
        /// </summary>
        /// <returns>Collection of all products</returns>
        public virtual IEnumerable<ProductModel> GetProducts()
        {

            // Open and read the JSON file
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {

                // Deserialize JSON content to product array
                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            }

        }

        /// <summary>
        /// Retrieves a specific product by its ID
        /// </summary>
        /// <param name="productId">ID of the product to retrieve</param>
        /// <returns>Product matching the ID or null if not found</returns>
        public virtual ProductModel GetProductById(string productId)
        {

            // Find and return product with matching ID
            return GetProducts().FirstOrDefault(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));

        }

        /// <summary>
        /// Adds a rating to a specific product
        /// </summary>
        /// <param name="productId">ID of the product to rate</param>
        /// <param name="rating">Rating value to add</param>
        /// <returns>True if successful, false otherwise</returns>
        public virtual bool AddRating(string productId, int rating)
        {

            // Load all products
            var products = GetProducts().ToList();

            // Find the target product
            var product = products.FirstOrDefault(x => x.Id == productId);

            // Fast fail: Check if product was not found
            if (product == null)
            {
                return false;
            }

            // Fast fail: Check if ratings array is null
            if (product.Ratings == null)
            {
                product.Ratings = new int[] { rating };
                SaveData(products);
                return true;
            }

            // Add rating to existing ratings
            var ratings = product.Ratings.ToList();

            ratings.Add(rating);

            product.Ratings = ratings.ToArray();

            // Save updated data
            SaveData(products);

            return true;

        }

        /// <summary>
        /// Updates an existing product with new data
        /// </summary>
        /// <param name="data">Product data to update</param>
        /// <returns>True if successful, false otherwise</returns>
        public virtual bool UpdateData(ProductModel data)
        {

            // Load all products
            var products = GetProducts().ToList();

            // Find index of product to update
            var index = products.FindIndex(p => string.Equals(p.Id, data.Id, StringComparison.OrdinalIgnoreCase));

            // Fast fail: Check if product was not found
            if (index < 0)
            {
                return false;
            }

            // Get existing product
            var existing = products[index];

            // Preserve existing ratings if none provided
            if (data.Ratings == null)
            {
                data.Ratings = existing.Ratings;
            }

            // Update product in list
            products[index] = data;

            // Save updated data
            SaveData(products);

            return true;

        }

        /// <summary>
        /// Adds a new product to the catalog
        /// </summary>
        /// <param name="newProduct">Product to add</param>
        /// <returns>True if successful, false if product already exists or error occurs</returns>
        public virtual bool AddCategory(ProductModel newProduct)
        {

            // Load all products
            var products = GetProducts().ToList();

            // Check if product with same ID already exists
            var productExists = products.Any(p => string.Equals(p.Id, newProduct.Id, StringComparison.OrdinalIgnoreCase));

            // Fast fail: Check if product already exists
            if (productExists)
            {
                return false;
            }

            // Add new product to list
            products.Add(newProduct);

            // Save updated data
            SaveData(products);

            return true;

        }

        /// <summary>
        /// Updates an existing product category
        /// </summary>
        /// <param name="updatedProduct">Updated product data</param>
        /// <returns>True if successful, false otherwise</returns>
        public virtual bool UpdateCategory(ProductModel updatedProduct)
        {

            // Load all products
            var products = GetProducts().ToList();

            // Find index of product to update
            var idx = products.FindIndex(p => string.Equals(p.Id, updatedProduct.Id, StringComparison.OrdinalIgnoreCase));

            // Fast fail: Check if product was not found
            if (idx < 0)
            {
                return false;
            }

            // Get existing product
            var existing = products[idx];

            // Preserve existing ratings if none provided
            if (updatedProduct.Ratings == null)
            {
                updatedProduct.Ratings = existing.Ratings;
            }

            // Update product in list
            products[idx] = updatedProduct;

            // Save updated data
            SaveData(products);

            return true;

        }

        /// <summary>
        /// Deletes a product from the catalog
        /// </summary>
        /// <param name="productId">ID of the product to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public virtual bool DeleteCategory(string productId)
        {

            // Load all products
            var products = GetProducts().ToList();

            // Remove product with matching ID
            var removed = products.RemoveAll(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));

            // Fast fail: Check if no products were removed
            if (removed == 0)
            {
                return false;
            }

            // Save updated data
            SaveData(products);

            return true;

        }

        /// <summary>
        /// Saves an uploaded file to the assets folder and returns the web path
        /// </summary>
        /// <param name="file">The uploaded file from the form</param>
        /// <returns>Web-accessible path like /assets/filename.png or null if no file</returns>
        public virtual async Task<string> SaveUploadedFileAsync(IFormFile file)
        {

            // Fast fail: Check if file is null
            if (file == null)
            {
                return null;
            }

            // Fast fail: Check if file length is zero
            if (file.Length == 0)
            {
                return null;
            }

            // Create the assets folder path
            var assetsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "assets");

            // Check if assets folder exists
            var assetsFolderExists = Directory.Exists(assetsFolder);

            // Create directory if it does not exist
            if (assetsFolderExists == false)
            {
                Directory.CreateDirectory(assetsFolder);
            }

            // Get file extension
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            // Generate a unique filename to avoid conflicts
            var uniqueFileName = $"{Guid.NewGuid():N}{fileExtension}";

            // Combine folder path with unique filename
            var physicalPath = Path.Combine(assetsFolder, uniqueFileName);

            // Copy the uploaded file to the assets folder
            using (var fileStream = new FileStream(physicalPath, FileMode.Create))
            {

                await file.CopyToAsync(fileStream);

            }

            // Return the web path for accessing the file
            return $"/assets/{uniqueFileName}";

        }

        /// <summary>
        /// Saves product data to the JSON file
        /// </summary>
        /// <param name="products">Collection of products to save</param>
        private void SaveData(IEnumerable<ProductModel> products)
        {

            // Get directory path from file name
            var dir = Path.GetDirectoryName(JsonFileName);

            // Check if directory exists
            var directoryExists = Directory.Exists(dir);

            // Create directory if it does not exist
            if (directoryExists == false)
            {
                Directory.CreateDirectory(dir);
            }

            // Create and write to JSON file
            using (var outputStream = File.Create(JsonFileName))
            {

                // Serialize products to JSON with formatting
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );

            }

        }

    }

}