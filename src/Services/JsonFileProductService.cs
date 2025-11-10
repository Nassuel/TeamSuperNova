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
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment, IWebHostEnvironment env)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }
        }

        public IEnumerable<ProductModel> GetProducts()
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

        public ProductModel GetProductById(string productId)
        {
            return GetProducts().FirstOrDefault(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));
        }

        public bool AddRating(string productId, int rating)
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

        public bool UpdateData(ProductModel data)
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

        public bool AddCategory(ProductModel newProduct)
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

        public bool UpdateCategory(ProductModel updatedProduct)
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

        public bool DeleteCategory(string productId)
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
        /// Take in file, saves it to /assets/ folder and
        /// returns the path as a string.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string> SaveUploadedFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var uploads = Path.Combine(WebHostEnvironment.WebRootPath, "assets");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"~/assets/{fileName}";
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