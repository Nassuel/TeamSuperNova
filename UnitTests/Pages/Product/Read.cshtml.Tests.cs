using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for ReadModel class
    /// Tests all methods and properties to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class ReadModelTests
    {
        #region TestSetup

        // Page model instance for testing
        public ReadModel PageModel;

        // Mock product service
        public Mock<JsonFileProductService> MockProductService;

        // Test product data
        public List<ProductModel> TestProducts;

        /// <summary>
        /// Initialize test environment before each test
        /// Creates mock service and test data
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Arrange - Create test products
            TestProducts = new List<ProductModel>
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
                },
                new ProductModel
                {
                    Id = "test-mice-1",
                    Brand = "MiceBrand",
                    ProductName = "Test Mouse",
                    ProductType = ProductTypeEnum.Mice,
                    Url = "https://mice.com",
                    ProductDescription = "Mouse Description",
                    Image = "/assets/mouse.png",
                    Ratings = new int[] { 3, 4 }
                }
            };

            // Create mock product service
            MockProductService = new Mock<JsonFileProductService>(MockBehavior.Strict, null);

            // Setup GetProducts to return test data
            MockProductService.Setup(x => x.GetProducts()).Returns(TestProducts);

            // Instantiate page model with mock service
            PageModel = new ReadModel(MockProductService.Object);
        }

        #endregion TestSetup

        #region Constructor

        /// <summary>
        /// Test constructor initializes ProductService correctly
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Initialize_ProductService()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = PageModel.ProductService;

            // Assert
            Assert.AreEqual(MockProductService.Object, result);
        }

        #endregion Constructor

        #region OnGet

        /// <summary>
        /// Test OnGet with valid product ID returns Page result
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Return_Page_Result()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            var result = PageModel.OnGet(productId);

            // Assert
            Assert.IsInstanceOf<PageResult>(result);
        }

        /// <summary>
        /// Test OnGet with valid product ID sets Product property
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Set_Product_Property()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual("test-laptop-1", PageModel.Product.Id);
            Assert.AreEqual("TestBrand", PageModel.Product.Brand);
            Assert.AreEqual("Test Laptop", PageModel.Product.ProductName);
        }

        /// <summary>
        /// Test OnGet with valid product ID loads correct product data
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Load_Correct_Product_Data()
        {
            // Arrange
            var productId = "test-keyboard-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual("test-keyboard-1", PageModel.Product.Id);
            Assert.AreEqual("KeyboardBrand", PageModel.Product.Brand);
            Assert.AreEqual(ProductTypeEnum.Keyboard, PageModel.Product.ProductType);
            Assert.AreEqual("/assets/keyboard.png", PageModel.Product.Image);
        }

        /// <summary>
        /// Test OnGet with valid product ID loads product with ratings
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_With_Ratings_Should_Load_Product()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual(3, PageModel.Product.Ratings.Length);
            Assert.AreEqual(5, PageModel.Product.Ratings[0]);
            Assert.AreEqual(4, PageModel.Product.Ratings[1]);
        }

        /// <summary>
        /// Test OnGet with valid product ID loads product with null ratings
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_With_Null_Ratings_Should_Load_Product()
        {
            // Arrange
            var productId = "test-keyboard-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual(null, PageModel.Product.Ratings);
        }

        /// <summary>
        /// Test OnGet with different valid product ID loads correct product
        /// </summary>
        [Test]
        public void OnGet_Valid_Different_ProductId_Should_Load_Correct_Product()
        {
            // Arrange
            var productId = "test-mice-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual("test-mice-1", PageModel.Product.Id);
            Assert.AreEqual("MiceBrand", PageModel.Product.Brand);
            Assert.AreEqual(ProductTypeEnum.Mice, PageModel.Product.ProductType);
        }

        /// <summary>
        /// Test OnGet with invalid product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            var productId = "non-existent-id";

            // Act
            var result = PageModel.OnGet(productId);

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        /// <summary>
        /// Test OnGet with invalid product ID redirects to Index page
        /// </summary>
        [Test]
        public void OnGet_Invalid_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            var productId = "non-existent-id";

            // Act
            var result = PageModel.OnGet(productId) as RedirectToPageResult;

            // Assert
            Assert.AreEqual("./Index", result.PageName);
        }

        /// <summary>
        /// Test OnGet with invalid product ID sets Product to null
        /// </summary>
        [Test]
        public void OnGet_Invalid_ProductId_Should_Set_Product_Null()
        {
            // Arrange
            var productId = "non-existent-id";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual(null, PageModel.Product);
        }

        /// <summary>
        /// Test OnGet with invalid product ID adds model error
        /// </summary>
        [Test]
        public void OnGet_Invalid_ProductId_Should_Add_ModelState_Error()
        {
            // Arrange
            var productId = "non-existent-id";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual(false, PageModel.ModelState.IsValid);
            Assert.IsTrue(PageModel.ModelState.ContainsKey("bogus"));
        }

        /// <summary>
        /// Test OnGet with invalid product ID adds correct error message
        /// </summary>
        [Test]
        public void OnGet_Invalid_ProductId_Should_Add_Correct_Error_Message()
        {
            // Arrange
            var productId = "non-existent-id";

            // Act
            PageModel.OnGet(productId);

            // Assert
            var errors = PageModel.ModelState["bogus"].Errors;
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("bogus error", errors[0].ErrorMessage);
        }

        /// <summary>
        /// Test OnGet with null product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_Null_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            string productId = null;

            // Act
            var result = PageModel.OnGet(productId);

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        /// <summary>
        /// Test OnGet with empty string product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_Empty_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            var productId = string.Empty;

            // Act
            var result = PageModel.OnGet(productId);

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        /// <summary>
        /// Test OnGet with whitespace product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_Whitespace_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            var productId = "   ";

            // Act
            var result = PageModel.OnGet(productId);

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
        }

        /// <summary>
        /// Test OnGet with case sensitive product ID match
        /// </summary>
        [Test]
        public void OnGet_Valid_Case_Sensitive_ProductId_Should_Match_Exact_Case()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual("test-laptop-1", PageModel.Product.Id);
        }

        /// <summary>
        /// Test OnGet with different case product ID does not match
        /// </summary>
        [Test]
        public void OnGet_Invalid_Different_Case_ProductId_Should_Not_Match()
        {
            // Arrange
            var productId = "TEST-LAPTOP-1";

            // Act
            var result = PageModel.OnGet(productId);

            // Assert
            Assert.IsInstanceOf<RedirectToPageResult>(result);
            Assert.AreEqual(null, PageModel.Product);
        }

        /// <summary>
        /// Test OnGet loads product with all properties correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Load_All_Product_Properties()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual("test-laptop-1", PageModel.Product.Id);
            Assert.AreEqual("TestBrand", PageModel.Product.Brand);
            Assert.AreEqual("Test Laptop", PageModel.Product.ProductName);
            Assert.AreEqual(ProductTypeEnum.Laptop, PageModel.Product.ProductType);
            Assert.AreEqual("https://test.com", PageModel.Product.Url);
            Assert.AreEqual("Test Description", PageModel.Product.ProductDescription);
            Assert.AreEqual("/assets/test.png", PageModel.Product.Image);
            Assert.IsNotNull(PageModel.Product.Ratings);
        }

        /// <summary>
        /// Test OnGet uses FirstOrDefault for product lookup
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Use_FirstOrDefault_For_Lookup()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            MockProductService.Verify(x => x.GetProducts(), Times.Once);
            Assert.AreEqual("test-laptop-1", PageModel.Product.Id);
        }

        /// <summary>
        /// Test OnGet with product ID matching first product in list
        /// </summary>
        [Test]
        public void OnGet_Valid_First_Product_Should_Return_First_Product()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual(TestProducts.First().Id, PageModel.Product.Id);
        }

        /// <summary>
        /// Test OnGet with product ID matching last product in list
        /// </summary>
        [Test]
        public void OnGet_Valid_Last_Product_Should_Return_Last_Product()
        {
            // Arrange
            var productId = "test-mice-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual(TestProducts.Last().Id, PageModel.Product.Id);
        }

        /// <summary>
        /// Test OnGet with product ID matching middle product in list
        /// </summary>
        [Test]
        public void OnGet_Valid_Middle_Product_Should_Return_Middle_Product()
        {
            // Arrange
            var productId = "test-keyboard-1";

            // Act
            PageModel.OnGet(productId);

            // Assert
            Assert.AreEqual(TestProducts[1].Id, PageModel.Product.Id);
        }

        #endregion OnGet

        #region ProductService

        /// <summary>
        /// Test ProductService property returns correct instance
        /// </summary>
        [Test]
        public void ProductService_Valid_Should_Return_Correct_Instance()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = PageModel.ProductService;

            // Assert
            Assert.AreEqual(MockProductService.Object, result);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test ProductService property is accessible
        /// </summary>
        [Test]
        public void ProductService_Valid_Should_Be_Accessible()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = PageModel.ProductService;

            // Assert
            Assert.IsInstanceOf<JsonFileProductService>(result);
        }

        #endregion ProductService

        #region Product

        /// <summary>
        /// Test Product property is initially null
        /// </summary>
        [Test]
        public void Product_Initial_Should_Be_Null()
        {
            // Arrange
            var newPageModel = new ReadModel(MockProductService.Object);

            // Act
            var result = newPageModel.Product;

            // Assert
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// Test Product property is set after OnGet with valid ID
        /// </summary>
        [Test]
        public void Product_After_Valid_OnGet_Should_Be_Set()
        {
            // Arrange
            var productId = "test-laptop-1";

            // Act
            PageModel.OnGet(productId);
            var result = PageModel.Product;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test-laptop-1", result.Id);
        }

        /// <summary>
        /// Test Product property remains null after OnGet with invalid ID
        /// </summary>
        [Test]
        public void Product_After_Invalid_OnGet_Should_Remain_Null()
        {
            // Arrange
            var productId = "non-existent-id";

            // Act
            PageModel.OnGet(productId);
            var result = PageModel.Product;

            // Assert
            Assert.AreEqual(null, result);
        }

        #endregion Product
    }
}