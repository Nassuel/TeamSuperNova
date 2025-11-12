using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for JsonFileProductService class
    /// Tests all methods to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        // Service instance for testing
        public JsonFileProductService TestService;

        // Mock web host environment
        public Mock<IWebHostEnvironment> MockWebHostEnvironment;

        // Path to test JSON file
        public string TestJsonFilePath;

        /// <summary>
        /// Initialize test environment before each test
        /// Creates mock service and temporary test file
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Arrange - Create mock web host environment
            MockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            // Setup test directory structure
            var testPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "wwwroot");
            var dataPath = Path.Combine(testPath, "data");
            var assetsPath = Path.Combine(testPath, "assets");
            TestJsonFilePath = Path.Combine(dataPath, "products.json");

            // Create directories if they don't exist
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            if (!Directory.Exists(assetsPath))
            {
                Directory.CreateDirectory(assetsPath);
            }

            // Configure mock to return test path
            MockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(testPath);

            // Create test JSON file with sample data
            CreateTestJsonFile();

            // Instantiate service with mock environment
            TestService = new JsonFileProductService(MockWebHostEnvironment.Object);
        }

        /// <summary>
        /// Clean up test environment after each test
        /// Removes temporary test files and directories
        /// </summary>
        [TearDown]
        public void TestCleanup()
        {
            // Reset - Delete test file if it exists
            if (File.Exists(TestJsonFilePath))
            {
                File.Delete(TestJsonFilePath);
            }

            // Clean up assets directory
            var assetsPath = Path.Combine(MockWebHostEnvironment.Object.WebRootPath, "assets");
            if (Directory.Exists(assetsPath))
            {
                var files = Directory.GetFiles(assetsPath);
                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Creates a test JSON file with sample product data
        /// </summary>
        private void CreateTestJsonFile()
        {
            // Create test products array
            var testProducts = new[]
            {
                new ProductModel
                {
                    Id = "test-laptop-1",
                    Brand = "TestBrand",
                    ProductName = "Test Laptop",
                    ProductType = ProductTypeEnum.Laptop,
                    Url = "https://test.com",
                    ProductDescription = "Test Description",
                    Image = "/assets/test.png",
                    Ratings = new int[] { 5, 4, 5 }
                },
                new ProductModel
                {
                    Id = "test-keyboard-1",
                    Brand = "KeyboardBrand",
                    ProductName = "Test Keyboard",
                    ProductType = ProductTypeEnum.Keyboard,
                    Url = "https://keyboard.com",
                    ProductDescription = "Keyboard Description",
                    Image = "/assets/keyboard.png",
                    Ratings = null
                }
            };

            // Serialize and write to file
            var json = System.Text.Json.JsonSerializer.Serialize(testProducts, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(TestJsonFilePath, json);
        }

        #endregion TestSetup

        #region GetProducts

        /// <summary>
        /// Test GetProducts returns all products from JSON file
        /// </summary>
        [Test]
        public void GetProducts_Valid_Should_Return_All_Products()
        {
            // Arrange - Service initialized in SetUp

            // Act
            var result = TestService.GetProducts();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        /// <summary>
        /// Test GetProducts returns correct product data
        /// </summary>
        [Test]
        public void GetProducts_Valid_Should_Return_Correct_Product_Data()
        {
            // Arrange - Service initialized in SetUp

            // Act
            var result = TestService.GetProducts().First();

            // Assert
            Assert.AreEqual("test-laptop-1", result.Id);
            Assert.AreEqual("TestBrand", result.Brand);
            Assert.AreEqual("Test Laptop", result.ProductName);
        }

        /// <summary>
        /// Test GetProducts returns products with ratings
        /// </summary>
        [Test]
        public void GetProducts_Valid_Should_Return_Products_With_Ratings()
        {
            // Arrange - Service initialized in SetUp

            // Act
            var result = TestService.GetProducts().First();

            // Assert
            Assert.AreEqual(3, result.Ratings.Length);
            Assert.AreEqual(5, result.Ratings[0]);
        }

        /// <summary>
        /// Test GetProducts returns products with null ratings
        /// </summary>
        [Test]
        public void GetProducts_Valid_Should_Return_Products_With_Null_Ratings()
        {
            // Arrange - Service initialized in SetUp

            // Act
            var result = TestService.GetProducts().Last();

            // Assert
            Assert.AreEqual(null, result.Ratings);
        }

        #endregion GetProducts

        #region GetProductById

        /// <summary>
        /// Test GetProductById returns correct product
        /// </summary>
        [Test]
        public void GetProductById_Valid_ProductId_Should_Return_Product()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            var result = TestService.GetProductById(productId);

            // Assert
            Assert.AreEqual("test-laptop-1", result.Id);
            Assert.AreEqual("TestBrand", result.Brand);
        }

        /// <summary>
        /// Test GetProductById with case insensitive search
        /// </summary>
        [Test]
        public void GetProductById_Valid_Mixed_Case_ProductId_Should_Return_Product()
        {
            // Arrange
            var productId = "TEST-LAPTOP-1";

            // Act
            var result = TestService.GetProductById(productId);

            // Assert
            Assert.AreEqual("test-laptop-1", result.Id);
        }

        /// <summary>
        /// Test GetProductById returns null for non-existent product
        /// </summary>
        [Test]
        public void GetProductById_Invalid_ProductId_Should_Return_Null()
        {
            // Arrange
            var productId = "non-existent-id";

            // Act
            var result = TestService.GetProductById(productId);

            // Assert
            Assert.AreEqual(null, result);
        }

        #endregion GetProductById

        #region AddRating

        /// <summary>
        /// Test AddRating adds rating to product with existing ratings
        /// </summary>
        [Test]
        public void AddRating_Valid_Existing_Ratings_Should_Add_Rating_Return_True()
        {
            // Arrange
            var productId = "test-laptop-1";
            var rating = 3;

            // Act
            var result = TestService.AddRating(productId, rating);

            // Assert
            Assert.AreEqual(true, result);
            var product = TestService.GetProductById(productId);
            Assert.AreEqual(4, product.Ratings.Length);
            Assert.AreEqual(3, product.Ratings[3]);
        }

        /// <summary>
        /// Test AddRating adds rating to product with null ratings
        /// </summary>
        [Test]
        public void AddRating_Valid_Null_Ratings_Should_Create_Ratings_Array_Return_True()
        {
            // Arrange
            var productId = "test-keyboard-1";
            var rating = 5;

            // Act
            var result = TestService.AddRating(productId, rating);

            // Assert
            Assert.AreEqual(true, result);
            var product = TestService.GetProductById(productId);
            Assert.AreEqual(1, product.Ratings.Length);
            Assert.AreEqual(5, product.Ratings[0]);
        }

        /// <summary>
        /// Test AddRating returns false for non-existent product
        /// </summary>
        [Test]
        public void AddRating_Invalid_ProductId_Should_Return_False()
        {
            // Arrange
            var productId = "non-existent-id";
            var rating = 5;

            // Act
            var result = TestService.AddRating(productId, rating);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Test AddRating handles exception and returns false
        /// </summary>
        [Test]
        public void AddRating_Exception_Should_Return_False()
        {
            // Arrange - Delete JSON file to cause exception
            File.Delete(TestJsonFilePath);
            var productId = "test-laptop-1";
            var rating = 5;

            // Act
            var result = TestService.AddRating(productId, rating);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion AddRating

        #region UpdateData

        /// <summary>
        /// Test UpdateData updates existing product successfully
        /// </summary>
        [Test]
        public void UpdateData_Valid_Product_Should_Update_Return_True()
        {
            // Arrange
            var updatedProduct = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "UpdatedBrand",
                ProductName = "Updated Laptop",
                ProductType = ProductTypeEnum.Laptop,
                Url = "https://updated.com",
                ProductDescription = "Updated Description",
                Image = "/assets/updated.png",
                Ratings = null
            };

            // Act
            var result = TestService.UpdateData(updatedProduct);

            // Assert
            Assert.AreEqual(true, result);
            var product = TestService.GetProductById("test-laptop-1");
            Assert.AreEqual("UpdatedBrand", product.Brand);
            Assert.AreEqual("Updated Laptop", product.ProductName);
        }

        /// <summary>
        /// Test UpdateData preserves existing ratings when new ratings are null
        /// </summary>
        [Test]
        public void UpdateData_Valid_Null_Ratings_Should_Preserve_Existing_Ratings()
        {
            // Arrange
            var originalRatings = TestService.GetProductById("test-laptop-1").Ratings;
            var updatedProduct = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "UpdatedBrand",
                ProductName = "Updated Laptop",
                ProductType = ProductTypeEnum.Laptop,
                Ratings = null
            };

            // Act
            var result = TestService.UpdateData(updatedProduct);

            // Assert
            Assert.AreEqual(true, result);
            var product = TestService.GetProductById("test-laptop-1");
            Assert.AreEqual(originalRatings.Length, product.Ratings.Length);
        }

        /// <summary>
        /// Test UpdateData returns false for non-existent product
        /// </summary>
        [Test]
        public void UpdateData_Invalid_ProductId_Should_Return_False()
        {
            // Arrange
            var updatedProduct = new ProductModel
            {
                Id = "non-existent-id",
                Brand = "Test",
                ProductName = "Test"
            };

            // Act
            var result = TestService.UpdateData(updatedProduct);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Test UpdateData handles exception and returns false
        /// </summary>
        [Test]
        public void UpdateData_Exception_Should_Return_False()
        {
            // Arrange - Delete JSON file to cause exception
            File.Delete(TestJsonFilePath);
            var updatedProduct = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "Test"
            };

            // Act
            var result = TestService.UpdateData(updatedProduct);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion UpdateData

        #region AddCategory

        /// <summary>
        /// Test AddCategory adds new product successfully
        /// </summary>
        [Test]
        public void AddCategory_Valid_New_Product_Should_Add_Return_True()
        {
            // Arrange
            var newProduct = new ProductModel
            {
                Id = "test-mice-1",
                Brand = "MiceBrand",
                ProductName = "Test Mouse",
                ProductType = ProductTypeEnum.Mice,
                Url = "https://mice.com",
                ProductDescription = "Mouse Description",
                Image = "/assets/mouse.png",
                Ratings = null
            };

            // Act
            var result = TestService.AddCategory(newProduct);

            // Assert
            Assert.AreEqual(true, result);
            var products = TestService.GetProducts();
            Assert.AreEqual(3, products.Count());
            var addedProduct = TestService.GetProductById("test-mice-1");
            Assert.AreEqual("MiceBrand", addedProduct.Brand);
        }

        /// <summary>
        /// Test AddCategory returns false for duplicate product ID
        /// </summary>
        [Test]
        public void AddCategory_Invalid_Duplicate_ProductId_Should_Return_False()
        {
            // Arrange
            var duplicateProduct = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "DuplicateBrand",
                ProductName = "Duplicate Product"
            };

            // Act
            var result = TestService.AddCategory(duplicateProduct);

            // Assert
            Assert.AreEqual(false, result);
            var products = TestService.GetProducts();
            Assert.AreEqual(2, products.Count());
        }

        /// <summary>
        /// Test AddCategory with case insensitive duplicate ID check
        /// </summary>
        [Test]
        public void AddCategory_Invalid_Case_Insensitive_Duplicate_Should_Return_False()
        {
            // Arrange
            var duplicateProduct = new ProductModel
            {
                Id = "TEST-LAPTOP-1",
                Brand = "DuplicateBrand"
            };

            // Act
            var result = TestService.AddCategory(duplicateProduct);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Test AddCategory handles exception and returns false
        /// </summary>
        [Test]
        public void AddCategory_Exception_Should_Return_False()
        {
            // Arrange - Delete JSON file to cause exception
            File.Delete(TestJsonFilePath);
            var newProduct = new ProductModel
            {
                Id = "test-mice-1",
                Brand = "Test"
            };

            // Act
            var result = TestService.AddCategory(newProduct);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion AddCategory

        #region UpdateCategory

        /// <summary>
        /// Test UpdateCategory updates existing product successfully
        /// </summary>
        [Test]
        public void UpdateCategory_Valid_Product_Should_Update_Return_True()
        {
            // Arrange
            var updatedProduct = new ProductModel
            {
                Id = "test-keyboard-1",
                Brand = "UpdatedKeyboardBrand",
                ProductName = "Updated Keyboard",
                ProductType = ProductTypeEnum.Keyboard,
                Ratings = null
            };

            // Act
            var result = TestService.UpdateCategory(updatedProduct);

            // Assert
            Assert.AreEqual(true, result);
            var product = TestService.GetProductById("test-keyboard-1");
            Assert.AreEqual("UpdatedKeyboardBrand", product.Brand);
        }

        /// <summary>
        /// Test UpdateCategory preserves existing ratings when new ratings are null
        /// </summary>
        [Test]
        public void UpdateCategory_Valid_Null_Ratings_Should_Preserve_Existing_Ratings()
        {
            // Arrange
            var originalRatings = TestService.GetProductById("test-laptop-1").Ratings;
            var updatedProduct = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "UpdatedBrand",
                Ratings = null
            };

            // Act
            var result = TestService.UpdateCategory(updatedProduct);

            // Assert
            Assert.AreEqual(true, result);
            var product = TestService.GetProductById("test-laptop-1");
            Assert.AreEqual(originalRatings.Length, product.Ratings.Length);
        }

        /// <summary>
        /// Test UpdateCategory returns false for non-existent product
        /// </summary>
        [Test]
        public void UpdateCategory_Invalid_ProductId_Should_Return_False()
        {
            // Arrange
            var updatedProduct = new ProductModel
            {
                Id = "non-existent-id",
                Brand = "Test"
            };

            // Act
            var result = TestService.UpdateCategory(updatedProduct);

            // Assert
            Assert.AreEqual(false, result);
        }

        /// <summary>
        /// Test UpdateCategory handles exception and returns false
        /// </summary>
        [Test]
        public void UpdateCategory_Exception_Should_Return_False()
        {
            // Arrange - Delete JSON file to cause exception
            File.Delete(TestJsonFilePath);
            var updatedProduct = new ProductModel
            {
                Id = "test-keyboard-1",
                Brand = "Test"
            };

            // Act
            var result = TestService.UpdateCategory(updatedProduct);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion UpdateCategory

        #region DeleteCategory

        /// <summary>
        /// Test DeleteCategory removes product successfully
        /// </summary>
        [Test]
        public void DeleteCategory_Valid_ProductId_Should_Delete_Return_True()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            var result = TestService.DeleteCategory(productId);

            // Assert
            Assert.AreEqual(true, result);
            var products = TestService.GetProducts();
            Assert.AreEqual(1, products.Count());
            var deletedProduct = TestService.GetProductById(productId);
            Assert.AreEqual(null, deletedProduct);
        }

        /// <summary>
        /// Test DeleteCategory with case insensitive product ID
        /// </summary>
        [Test]
        public void DeleteCategory_Valid_Case_Insensitive_ProductId_Should_Delete_Return_True()
        {
            // Arrange
            var productId = "TEST-KEYBOARD-1";

            // Act
            var result = TestService.DeleteCategory(productId);

            // Assert
            Assert.AreEqual(true, result);
            var product = TestService.GetProductById("test-keyboard-1");
            Assert.AreEqual(null, product);
        }

        /// <summary>
        /// Test DeleteCategory returns false for non-existent product
        /// </summary>
        [Test]
        public void DeleteCategory_Invalid_ProductId_Should_Return_False()
        {
            // Arrange
            var productId = "non-existent-id";

            // Act
            var result = TestService.DeleteCategory(productId);

            // Assert
            Assert.AreEqual(false, result);
            var products = TestService.GetProducts();
            Assert.AreEqual(2, products.Count());
        }

        /// <summary>
        /// Test DeleteCategory handles exception and returns false
        /// </summary>
        [Test]
        public void DeleteCategory_Exception_Should_Return_False()
        {
            // Arrange - Delete JSON file to cause exception
            File.Delete(TestJsonFilePath);
            var productId = "test-laptop-1";

            // Act
            var result = TestService.DeleteCategory(productId);

            // Assert
            Assert.AreEqual(false, result);
        }

        #endregion DeleteCategory

        #region SaveUploadedFileAsync

        /// <summary>
        /// Test SaveUploadedFileAsync saves file and returns correct path
        /// </summary>
        [Test]
        public async Task SaveUploadedFileAsync_Valid_File_Should_Save_Return_Path()
        {
            // Arrange
            var fileName = "test-image.png";
            var content = "fake image content";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(stream.Length);
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream target, CancellationToken token) =>
                {
                    stream.CopyTo(target);
                    return Task.CompletedTask;
                });

            // Act
            var result = await TestService.SaveUploadedFileAsync(mockFile.Object);

            // Assert
            Assert.IsTrue(result.StartsWith("/assets/"));
            Assert.IsTrue(result.EndsWith(".png"));
        }

        /// <summary>
        /// Test SaveUploadedFileAsync creates assets directory if not exists
        /// </summary>
        [Test]
        public async Task SaveUploadedFileAsync_Valid_No_Assets_Directory_Should_Create_Directory()
        {
            // Arrange
            var assetsPath = Path.Combine(MockWebHostEnvironment.Object.WebRootPath, "assets");
            if (Directory.Exists(assetsPath))
            {
                Directory.Delete(assetsPath, true);
            }

            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.png");
            mockFile.Setup(f => f.Length).Returns(100);
            mockFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await TestService.SaveUploadedFileAsync(mockFile.Object);

            // Assert
            Assert.IsTrue(Directory.Exists(assetsPath));
            Assert.IsTrue(result.StartsWith("/assets/"));
        }

        /// <summary>
        /// Test SaveUploadedFileAsync returns null for null file
        /// </summary>
        [Test]
        public async Task SaveUploadedFileAsync_Invalid_Null_File_Should_Return_Null()
        {
            // Arrange
            IFormFile nullFile = null;

            // Act
            var result = await TestService.SaveUploadedFileAsync(nullFile);

            // Assert
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// Test SaveUploadedFileAsync returns null for empty file
        /// </summary>
        [Test]
        public async Task SaveUploadedFileAsync_Invalid_Empty_File_Should_Return_Null()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0);

            // Act
            var result = await TestService.SaveUploadedFileAsync(mockFile.Object);

            // Assert
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// Test SaveUploadedFileAsync generates unique filename with GUID
        /// </summary>
        [Test]
        public async Task SaveUploadedFileAsync_Valid_Multiple_Files_Should_Generate_Unique_Names()
        {
            // Arrange
            var mockFile1 = new Mock<IFormFile>();
            mockFile1.Setup(f => f.FileName).Returns("test.png");
            mockFile1.Setup(f => f.Length).Returns(100);
            mockFile1.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var mockFile2 = new Mock<IFormFile>();
            mockFile2.Setup(f => f.FileName).Returns("test.png");
            mockFile2.Setup(f => f.Length).Returns(100);
            mockFile2.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result1 = await TestService.SaveUploadedFileAsync(mockFile1.Object);
            var result2 = await TestService.SaveUploadedFileAsync(mockFile2.Object);

            // Assert
            Assert.AreNotEqual(result1, result2);
        }

        /// <summary>
        /// Test SaveUploadedFileAsync preserves file extension
        /// </summary>
        [Test]
        public async Task SaveUploadedFileAsync_Valid_Different_Extensions_Should_Preserve_Extension()
        {
            // Arrange
            var mockFileJpg = new Mock<IFormFile>();
            mockFileJpg.Setup(f => f.FileName).Returns("test.jpg");
            mockFileJpg.Setup(f => f.Length).Returns(100);
            mockFileJpg.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await TestService.SaveUploadedFileAsync(mockFileJpg.Object);

            // Assert
            Assert.IsTrue(result.EndsWith(".jpg"));
        }

        #endregion SaveUploadedFileAsync

        #region WebHostEnvironment

        /// <summary>
        /// Test WebHostEnvironment property returns correct instance
        /// </summary>
        [Test]
        public void WebHostEnvironment_Valid_Should_Return_Correct_Instance()
        {
            // Arrange - Service initialized in SetUp

            // Act
            var result = TestService.WebHostEnvironment;

            // Assert
            Assert.AreEqual(MockWebHostEnvironment.Object, result);
        }

        #endregion WebHostEnvironment
    }
}