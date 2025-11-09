using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;

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
                // Validate input data exists and has valid ID
                if (data == null)
                {
                    return false;
                }
                if (string.IsNullOrWhiteSpace(data.Id))
                {
                    return false;
                }
                var products = GetProducts().ToList();
                var index = products.FindIndex(p => string.Equals(p.Id, data.Id, StringComparison.OrdinalIgnoreCase));
                if (index < 0) return false;

                var existing = products[index];
                data.SubCategories ??= existing.SubCategories;
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

                // Ensure each subcategory in the new product has an Id and SubCategory set
                if (newProduct.SubCategories != null && newProduct.SubCategories.Count > 0)
                {
                    var existingIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var prod in products)
                    {
                        if (prod.SubCategories == null) continue;
                        foreach (var s in prod.SubCategories)
                        {
                            if (!string.IsNullOrWhiteSpace(s.Id))
                                existingIds.Add(s.Id);
                        }
                    }

                    var usedIdsInNew = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var sub in newProduct.SubCategories)
                    {
                        var baseId = string.IsNullOrWhiteSpace(sub.Id) ? MakeSafeId(sub.ProductName) : MakeSafeId(sub.Id);
                        var candidate = baseId;
                        var idx = 1;
                        while (string.IsNullOrWhiteSpace(candidate) || usedIdsInNew.Contains(candidate) || existingIds.Contains(candidate))
                        {
                            candidate = $"{baseId}_{idx++}";
                        }

                        sub.Id = candidate;
                        // set SubCategory if missing (default to Brand)
                        sub.SubCategory ??= sub.Brand;
                        usedIdsInNew.Add(candidate);
                    }
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
                updatedProduct.SubCategories ??= existing.SubCategories;
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

        public bool AddSubCategory(string productId, SubCategoryModel sub)
        {
            try
            {
                var products = GetProducts().ToList();
                var product = products.FirstOrDefault(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));
                if (product == null)
                {
                    return false;
                }

                product.SubCategories ??= new List<SubCategoryModel>();

                var baseId = string.IsNullOrWhiteSpace(sub.Id) ? MakeSafeId(sub.ProductName) : sub.Id;
                var candidate = baseId;
                var idx = 1;
                while (product.SubCategories.Any(s => string.Equals(s.Id, candidate, StringComparison.OrdinalIgnoreCase)))
                {
                    candidate = $"{baseId}_{idx++}";
                }

                sub.Id = candidate;

                // ensure SubCategory property exists (default to Brand)
                sub.SubCategory ??= sub.Brand;

                product.SubCategories.Add(sub);
                SaveData(products);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateSubCategory(string productId, SubCategoryModel updatedSub)
        {
            try
            {
                var products = GetProducts().ToList();
                var product = products.FirstOrDefault(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));
                if (product == null || product.SubCategories == null)
                {
                    return false;
                }

                // if SubCategory missing set to Brand (keeps consistency)
                updatedSub.SubCategory ??= updatedSub.Brand;

                var index = product.SubCategories.FindIndex(s => string.Equals(s.Id, updatedSub.Id, StringComparison.OrdinalIgnoreCase));
                if (index < 0)
                {
                    return false;
                }

                product.SubCategories[index] = updatedSub;
                SaveData(products);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSubCategory(string productId, string subId)
        {
            try
            {
                var products = GetProducts().ToList();
                var product = products.FirstOrDefault(p => string.Equals(p.Id, productId, StringComparison.OrdinalIgnoreCase));
                if (product == null || product.SubCategories == null)
                {
                    return false;
                }

                var removed = product.SubCategories.RemoveAll(s => string.Equals(s.Id, subId, StringComparison.OrdinalIgnoreCase));
                if (removed == 0)
                {
                    return false;
                }

                SaveData(products);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// One-off helper: finds any subcategories missing an Id or SubCategory and assigns stable values, then persists.
        /// </summary>
        public bool EnsureSubCategoryIds()
        {
            try
            {
                var products = GetProducts().ToList();
                var changed = false;

                var globalIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var p in products)
                {
                    if (p.SubCategories == null) continue;
                    foreach (var s in p.SubCategories)
                    {
                        if (!string.IsNullOrWhiteSpace(s.Id))
                            globalIds.Add(s.Id);
                    }
                }

                foreach (var p in products)
                {
                    if (p.SubCategories == null) continue;
                    var usedInProduct = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (var s in p.SubCategories)
                    {
                        // ensure SubCategory value exists
                        if (string.IsNullOrWhiteSpace(s.SubCategory))
                        {
                            s.SubCategory = s.Brand;
                            changed = true;
                        }

                        if (string.IsNullOrWhiteSpace(s.Id))
                        {
                            var baseId = MakeSafeId(s.ProductName);
                            var candidate = baseId;
                            var idx = 1;
                            while (string.IsNullOrWhiteSpace(candidate) || globalIds.Contains(candidate) || usedInProduct.Contains(candidate))
                            {
                                candidate = $"{baseId}_{idx++}";
                            }

                            s.Id = candidate;
                            globalIds.Add(candidate);
                            usedInProduct.Add(candidate);
                            changed = true;
                        }
                        else
                        {
                            usedInProduct.Add(s.Id);
                        }
                    }
                }

                if (changed)
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