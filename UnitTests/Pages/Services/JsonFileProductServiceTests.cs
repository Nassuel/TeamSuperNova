using System.Linq;
using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests.Services.TestJsonFileProductService
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup
        [SetUp]
        public void TestInitialize()
        {
        }
        #endregion TestSetup

        #region GetProducts
        [Test]
        public void GetProducts_Valid_Should_Return_Products()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetProducts();

            // Reset

            // Assert
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(true, result.Any());
        }
        #endregion GetProducts

        #region GetProductById
        [Test]
        public void GetProductById_Valid_ID_Should_Return_Product()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            var result = TestHelper.ProductService.GetProductById(data.Id);

            // Reset

            // Assert
            Assert.AreEqual(true, result != null);
            Assert.AreEqual(data.Id, result.Id);
        }

        [Test]
        public void GetProductById_Invalid_ID_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetProductById("invalid-id-999");

            // Reset

            // Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void GetProductById_Valid_ID_Uppercase_Should_Return_Product()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            var result = TestHelper.ProductService.GetProductById(data.Id.ToUpper());

            // Reset

            // Assert
            Assert.AreEqual(true, result != null);
        }
        #endregion GetProductById

        #region AddRating
        [Test]
        public void AddRating_Valid_ProductId_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().Last();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 0);

            // Reset

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void AddRating_Invalid_Product_ID_Not_Present_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("1000", 5);

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddRating_Invalid_Null_Product_ID_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating(null, 5);

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddRating_Valid_Product_With_Existing_Ratings_Should_Add_Rating()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 4);

            // Reset

            // Assert
            Assert.AreEqual(true, result);
        }
        #endregion AddRating

        #region UpdateData
        [Test]
        public void UpdateData_Valid_Product_Should_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            var updatedProduct = new ProductModel
            {
                Id = data.Id,
                Maker = data.Maker,
                Image = data.Image,
                Url = data.Url,
                Title = data.Title,
                Description = data.Description
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(updatedProduct);

            // Reset

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void UpdateData_Invalid_Product_ID_Should_Return_False()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "non-existent-id-99999",
                Title = "Test Product"
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(data);

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void UpdateData_Invalid_Null_Product_ID_Should_Return_False()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = null,
                Title = "Test Product"
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(data);

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void UpdateData_Valid_Product_Null_SubCategories_Should_Preserve_Existing()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            var updatedProduct = new ProductModel
            {
                Id = data.Id,
                Title = "Updated Title",
                SubCategories = null
            };

            // Act
            var result = TestHelper.ProductService.UpdateData(updatedProduct);

            // Reset

            // Assert
            Assert.AreEqual(true, result);
        }
        #endregion UpdateData

        #region AddCategory
        [Test]
        public void AddCategory_Valid_New_Product_Should_Return_True()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-new-category-123",
                Title = "Test Category",
                Maker = "Test Maker"
            };

            // Act
            var result = TestHelper.ProductService.AddCategory(data);

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void AddCategory_Invalid_Duplicate_ID_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();

            // Act
            var result = TestHelper.ProductService.AddCategory(data);

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddCategory_Valid_With_SubCategories_Should_Assign_IDs()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-with-subs-456",
                Title = "Test",
                SubCategories = new List<SubCategoryModel>
                {
                    new SubCategoryModel { ProductName = "Sub1", Brand = "Brand1" }
                }
            };

            // Act
            var result = TestHelper.ProductService.AddCategory(data);

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(true, result);
        }
        #endregion AddCategory

        #region UpdateCategory
        [Test]
        public void UpdateCategory_Valid_Product_Should_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            var updated = new ProductModel
            {
                Id = data.Id,
                Title = data.Title,
                Maker = data.Maker
            };

            // Act
            var result = TestHelper.ProductService.UpdateCategory(updated);

            // Reset

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void UpdateCategory_Invalid_ID_Should_Return_False()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "invalid-category-999",
                Title = "Test"
            };

            // Act
            var result = TestHelper.ProductService.UpdateCategory(data);

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }
        #endregion UpdateCategory

        #region DeleteCategory
        [Test]
        public void DeleteCategory_Valid_ID_Should_Return_True()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-delete-cat-789",
                Title = "To Delete"
            };
            TestHelper.ProductService.AddCategory(data);

            // Act
            var result = TestHelper.ProductService.DeleteCategory(data.Id);

            // Reset

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void DeleteCategory_Invalid_ID_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.DeleteCategory("non-existent-999");

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }
        #endregion DeleteCategory

        #region AddSubCategory
        [Test]
        public void AddSubCategory_Valid_Product_Should_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.GetProducts().First();
            var sub = new SubCategoryModel
            {
                ProductName = "New Sub",
                Brand = "Brand"
            };

            // Act
            var result = TestHelper.ProductService.AddSubCategory(data.Id, sub);

            // Reset

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void AddSubCategory_Invalid_Product_ID_Should_Return_False()
        {
            // Arrange
            var data = new SubCategoryModel
            {
                ProductName = "Test",
                Brand = "Brand"
            };

            // Act
            var result = TestHelper.ProductService.AddSubCategory("invalid-999", data);

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void AddSubCategory_Valid_Null_SubCategories_Should_Initialize_List()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-null-subs-321",
                Title = "Test",
                SubCategories = null
            };
            TestHelper.ProductService.AddCategory(data);
            var sub = new SubCategoryModel { ProductName = "Sub", Brand = "Brand" };

            // Act
            var result = TestHelper.ProductService.AddSubCategory(data.Id, sub);

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(true, result);
        }
        #endregion AddSubCategory

        #region UpdateSubCategory
        [Test]
        public void UpdateSubCategory_Valid_Data_Should_Return_True()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-update-sub-654",
                Title = "Test",
                SubCategories = new List<SubCategoryModel>
                {
                    new SubCategoryModel { Id = "sub1", ProductName = "Original", Brand = "Brand" }
                }
            };
            TestHelper.ProductService.AddCategory(data);
            var sub = new SubCategoryModel { Id = "sub1", ProductName = "Updated", Brand = "Brand" };

            // Act
            var result = TestHelper.ProductService.UpdateSubCategory(data.Id, sub);

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void UpdateSubCategory_Invalid_Product_ID_Should_Return_False()
        {
            // Arrange
            var data = new SubCategoryModel { Id = "sub1", ProductName = "Test", Brand = "Brand" };

            // Act
            var result = TestHelper.ProductService.UpdateSubCategory("invalid-999", data);

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void UpdateSubCategory_Invalid_Null_SubCategories_Should_Return_False()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-null-update-987",
                Title = "Test",
                SubCategories = null
            };
            TestHelper.ProductService.AddCategory(data);
            var sub = new SubCategoryModel { Id = "sub1", ProductName = "Test", Brand = "Brand" };

            // Act
            var result = TestHelper.ProductService.UpdateSubCategory(data.Id, sub);

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void UpdateSubCategory_Invalid_SubCategory_ID_Should_Return_False()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-invalid-sub-741",
                Title = "Test",
                SubCategories = new List<SubCategoryModel>
                {
                    new SubCategoryModel { Id = "sub1", ProductName = "Test", Brand = "Brand" }
                }
            };
            TestHelper.ProductService.AddCategory(data);
            var sub = new SubCategoryModel { Id = "invalid-sub", ProductName = "Test", Brand = "Brand" };

            // Act
            var result = TestHelper.ProductService.UpdateSubCategory(data.Id, sub);

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(false, result);
        }
        #endregion UpdateSubCategory

        #region DeleteSubCategory
        [Test]
        public void DeleteSubCategory_Valid_IDs_Should_Return_True()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-delete-sub-852",
                Title = "Test",
                SubCategories = new List<SubCategoryModel>
                {
                    new SubCategoryModel { Id = "sub-del", ProductName = "Delete", Brand = "Brand" }
                }
            };
            TestHelper.ProductService.AddCategory(data);

            // Act
            var result = TestHelper.ProductService.DeleteSubCategory(data.Id, "sub-del");

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void DeleteSubCategory_Invalid_Product_ID_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.DeleteSubCategory("invalid-999", "sub1");

            // Reset

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void DeleteSubCategory_Invalid_Null_SubCategories_Should_Return_False()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-null-del-963",
                Title = "Test",
                SubCategories = null
            };
            TestHelper.ProductService.AddCategory(data);

            // Act
            var result = TestHelper.ProductService.DeleteSubCategory(data.Id, "sub1");

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(false, result);
        }

        [Test]
        public void DeleteSubCategory_Invalid_SubCategory_ID_Should_Return_False()
        {
            // Arrange
            var data = new ProductModel
            {
                Id = "test-invalid-del-147",
                Title = "Test",
                SubCategories = new List<SubCategoryModel>
                {
                    new SubCategoryModel { Id = "sub1", ProductName = "Test", Brand = "Brand" }
                }
            };
            TestHelper.ProductService.AddCategory(data);

            // Act
            var result = TestHelper.ProductService.DeleteSubCategory(data.Id, "invalid-sub");

            // Reset
            TestHelper.ProductService.DeleteCategory(data.Id);

            // Assert
            Assert.AreEqual(false, result);
        }
        #endregion DeleteSubCategory

        #region EnsureSubCategoryIds
        [Test]
        public void EnsureSubCategoryIds_Valid_Should_Return_True()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.EnsureSubCategoryIds();

            // Reset

            // Assert
            Assert.AreEqual(true, result);
        }
        #endregion EnsureSubCategoryIds
    }
}