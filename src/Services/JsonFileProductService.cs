//using ContosoCrafts.WebSite.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace ContosoCrafts.WebSite.Services
//{
//    public class JsonFileProductService
//    {
//        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
//        {
//            WebHostEnvironment = webHostEnvironment;
//        }

//        public IWebHostEnvironment WebHostEnvironment { get; }

//        private string JsonFileName
//        {
//            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
//        }

//        public IEnumerable<ProductModel> GetProducts()
//        {
//            using (var jsonFileReader = File.OpenText(JsonFileName))
//            {
//                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
//                    new JsonSerializerOptions
//                    {
//                        PropertyNameCaseInsensitive = true
//                    });
//            }
//        }

//        public ProductModel GetProductById(string productId)
//        {
//            return GetProducts().FirstOrDefault(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));
//        }

//        public bool AddRating(string productId, int rating)
//        {
//            try
//            {
//                var products = GetProducts().ToList();
//                var product = products.FirstOrDefault(x => x.Id == productId);
//                if (product == null) return false;

//                if (product.Ratings == null)
//                {
//                    product.Ratings = new int[] { rating };
//                }
//                else
//                {
//                    var ratings = product.Ratings.ToList();
//                    ratings.Add(rating);
//                    product.Ratings = ratings.ToArray();
//                }

//                SaveData(products);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public bool UpdateData(ProductModel data)
//        {
//            try
//            {
//                var products = GetProducts().ToList();
//                var index = products.FindIndex(p => string.Equals(p.Id, data.Id, StringComparison.OrdinalIgnoreCase));
//                if (index < 0) return false;

//                var existing = products[index];
//                data.Ratings ??= existing.Ratings;

//                products[index] = data;
//                SaveData(products);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public bool AddCategory(ProductModel newProduct)
//        {
//            try
//            {
//                var products = GetProducts().ToList();
//                if (products.Any(p => string.Equals(p.Id, newProduct.Id, StringComparison.OrdinalIgnoreCase)))
//                {
//                    return false;
//                }

//                products.Add(newProduct);
//                SaveData(products);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public bool UpdateCategory(ProductModel updatedProduct)
//        {
//            try
//            {
//                var products = GetProducts().ToList();
//                var idx = products.FindIndex(p => string.Equals(p.Id, updatedProduct.Id, StringComparison.OrdinalIgnoreCase));
//                if (idx < 0) return false;

//                var existing = products[idx];
//                updatedProduct.Ratings ??= existing.Ratings;

//                products[idx] = updatedProduct;
//                SaveData(products);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        public bool DeleteCategory(string productId)
//        {
//            try
//            {
//                var products = GetProducts().ToList();
//                var removed = products.RemoveAll(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));
//                if (removed == 0) return false;
//                SaveData(products);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        private void SaveData(IEnumerable<ProductModel> products)
//        {
//            var dir = Path.GetDirectoryName(JsonFileName);
//            if (!Directory.Exists(dir))
//            {
//                Directory.CreateDirectory(dir);
//            }

//            using (var outputStream = File.Create(JsonFileName))
//            {
//                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
//                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
//                    {
//                        SkipValidation = true,
//                        Indented = true
//                    }),
//                    products
//                );
//            }
//        }

//        /// <summary>
//        /// Take in file, saves it to /assets/ folder and
//        /// returns the path as a string.
//        /// </summary>
//        /// <param name="file"></param>
//        /// <returns></returns>
//        public async Task<string> SaveUploadedFileAsync(IFormFile file)
//        {
//            if (file == null || file.Length == 0) return null;

//            var uploads = Path.Combine(WebHostEnvironment.WebRootPath, "assets");
//            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

//            var ext = Path.GetExtension(file.FileName);
//            var fileName = $"{Guid.NewGuid():N}{ext}";
//            var filePath = Path.Combine(uploads, fileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            return $"/assets/{fileName}";
//        }

//        private string MakeSafeId(string text)
//        {
//            if (string.IsNullOrWhiteSpace(text))
//            {
//                return Guid.NewGuid().ToString("N");
//            }

//            var s = text.ToLowerInvariant().Trim();
//            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", "-");
//            s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-z0-9\-]", "");
//            return s;
//        }
//    }
//}










using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContosoCrafts.WebSite.Services
{
    public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        public virtual IEnumerable<ProductModel> GetProducts()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
        }

        public virtual ProductModel GetProductById(string productId)
        {
            return GetProducts().FirstOrDefault(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));
        }

        public virtual bool AddRating(string productId, int rating)
        {
            try
            {
                var products = GetProducts().ToList();
                var product = products.FirstOrDefault(x => x.Id == productId);
                if (product == null) return false;

                if (product.Ratings == null)
                {
                    product.Ratings = new int[] { rating };
                }
                else
                {
                    var ratings = product.Ratings.ToList();
                    ratings.Add(rating);
                    product.Ratings = ratings.ToArray();
                }

                SaveData(products);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool UpdateData(ProductModel data)
        {
            try
            {
                var products = GetProducts().ToList();
                var index = products.FindIndex(p => string.Equals(p.Id, data.Id, StringComparison.OrdinalIgnoreCase));
                if (index < 0) return false;

                var existing = products[index];
                data.Ratings ??= existing.Ratings;

                products[index] = data;
                SaveData(products);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool AddCategory(ProductModel newProduct)
        {
            try
            {
                var products = GetProducts().ToList();
                if (products.Any(p => string.Equals(p.Id, newProduct.Id, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }

                products.Add(newProduct);
                SaveData(products);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool UpdateCategory(ProductModel updatedProduct)
        {
            try
            {
                var products = GetProducts().ToList();
                var idx = products.FindIndex(p => string.Equals(p.Id, updatedProduct.Id, StringComparison.OrdinalIgnoreCase));
                if (idx < 0) return false;

                var existing = products[idx];
                updatedProduct.Ratings ??= existing.Ratings;

                products[idx] = updatedProduct;
                SaveData(products);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool DeleteCategory(string productId)
        {
            try
            {
                var products = GetProducts().ToList();
                var removed = products.RemoveAll(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));
                if (removed == 0) return false;
                SaveData(products);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SaveData(IEnumerable<ProductModel> products)
        {
            var dir = Path.GetDirectoryName(JsonFileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var outputStream = File.Create(JsonFileName))
            {
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

        /// <summary>
        /// Saves an uploaded file to the /assets/ folder and returns the web path.
        /// Files from outside the project are copied into wwwroot/assets/ so they become accessible.
        /// </summary>
        /// <param name="file">The uploaded file from the form</param>
        /// <returns>Web-accessible path like "/assets/filename.png" or null if no file</returns>
        public virtual async Task<string> SaveUploadedFileAsync(IFormFile file)
        {
            // Return null if no file was uploaded
            if (file == null || file.Length == 0)
                return null;

            // Create the assets folder if it doesn't exist
            var assetsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "assets");
            if (!Directory.Exists(assetsFolder))
            {
                Directory.CreateDirectory(assetsFolder);
            }

            // Generate a unique filename to avoid conflicts
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid():N}{fileExtension}";
            var physicalPath = Path.Combine(assetsFolder, uniqueFileName);

            // Copy the uploaded file to wwwroot/assets/
            using (var fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the web path (NOT the physical path)
            // This matches the format used by existing products: "/assets/filename.png"
            return $"/assets/{uniqueFileName}";
        }

        private string MakeSafeId(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Guid.NewGuid().ToString("N");
            }

            var s = text.ToLowerInvariant().Trim();
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", "-");
            s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-z0-9\-]", "");
            return s;
        }
    }
}