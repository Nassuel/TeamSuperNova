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

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(MockProductService.Object));
        }

        /// <summary>
        /// Test constructor creates valid instance
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Create_Valid_Instance()
        {
            // Arrange
            var mockService = new Mock<JsonFileProductService>(MockBehavior.Strict, null);

            // Act
            var result = new ReadModel(mockService.Object);

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ProductService, Is.EqualTo(mockService.Object));
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
            var data = "test-laptop-1";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Test OnGet with valid product ID sets Product property
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Set_Product_Property()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-laptop-1"));
            Assert.That(PageModel.Product.Brand, Is.EqualTo("TestBrand"));
            Assert.That(PageModel.Product.ProductName, Is.EqualTo("Test Laptop"));
        }

        /// <summary>
        /// Test OnGet with valid product ID loads correct product data
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Load_Correct_Product_Data()
        {
            // Arrange
            var data = "test-keyboard-1";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-keyboard-1"));
            Assert.That(PageModel.Product.Brand, Is.EqualTo("KeyboardBrand"));
            Assert.That(PageModel.Product.ProductType, Is.EqualTo(ProductTypeEnum.Keyboard));
            Assert.That(PageModel.Product.Image, Is.EqualTo("/assets/keyboard.png"));
        }

        /// <summary>
        /// Test OnGet with valid product ID loads product with ratings
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_With_Ratings_Should_Load_Product()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Ratings.Length, Is.EqualTo(3));
            Assert.That(PageModel.Product.Ratings[0], Is.EqualTo(5));
            Assert.That(PageModel.Product.Ratings[1], Is.EqualTo(4));
        }

        /// <summary>
        /// Test OnGet with valid product ID loads product with null ratings
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_With_Null_Ratings_Should_Load_Product()
        {
            // Arrange
            var data = "test-keyboard-1";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Ratings, Is.Null);
        }

        /// <summary>
        /// Test OnGet with different valid product ID loads correct product
        /// </summary>
        [Test]
        public void OnGet_Valid_Different_ProductId_Should_Load_Correct_Product()
        {
            // Arrange
            var data = "test-mice-1";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-mice-1"));
            Assert.That(PageModel.Product.Brand, Is.EqualTo("MiceBrand"));
            Assert.That(PageModel.Product.ProductType, Is.EqualTo(ProductTypeEnum.Mice));
        }

        /// <summary>
        /// Test OnGet with invalid product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            var data = "non-existent-id";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnGet with invalid product ID redirects to Index page
        /// </summary>
        [Test]
        public void OnGet_Invalid_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            var data = "non-existent-id";

            // Act
            var result = PageModel.OnGet(data) as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        /// <summary>
        /// Test OnGet with invalid product ID sets Product to null
        /// </summary>
        [Test]
        public void OnGet_Invalid_ProductId_Should_Set_Product_Null()
        {
            // Arrange
            var data = "non-existent-id";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product, Is.Null);
        }

        /// <summary>
        /// Test OnGet with null product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_Null_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            string data = null;

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnGet with null product ID redirects to Index page
        /// </summary>
        [Test]
        public void OnGet_Invalid_Null_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            string data = null;

            // Act
            var result = PageModel.OnGet(data) as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        /// <summary>
        /// Test OnGet with empty string product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_Empty_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            var data = string.Empty;

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnGet with empty string product ID redirects to Index page
        /// </summary>
        [Test]
        public void OnGet_Invalid_Empty_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            var data = string.Empty;

            // Act
            var result = PageModel.OnGet(data) as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        /// <summary>
        /// Test OnGet with whitespace product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_Whitespace_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            var data = "   ";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnGet with whitespace product ID redirects to Index page
        /// </summary>
        [Test]
        public void OnGet_Invalid_Whitespace_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            var data = "   ";

            // Act
            var result = PageModel.OnGet(data) as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        /// <summary>
        /// Test OnGet with case sensitive product ID match
        /// </summary>
        [Test]
        public void OnGet_Valid_Case_Sensitive_ProductId_Should_Match_Exact_Case()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-laptop-1"));
        }

        /// <summary>
        /// Test OnGet with different case product ID does not match
        /// </summary>
        [Test]
        public void OnGet_Invalid_Different_Case_ProductId_Should_Not_Match()
        {
            // Arrange
            var data = "TEST-LAPTOP-1";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(PageModel.Product, Is.Null);
        }

        /// <summary>
        /// Test OnGet loads product with all properties correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Load_All_Product_Properties()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-laptop-1"));
            Assert.That(PageModel.Product.Brand, Is.EqualTo("TestBrand"));
            Assert.That(PageModel.Product.ProductName, Is.EqualTo("Test Laptop"));
            Assert.That(PageModel.Product.ProductType, Is.EqualTo(ProductTypeEnum.Laptop));
            Assert.That(PageModel.Product.Url, Is.EqualTo("https://test.com"));
            Assert.That(PageModel.Product.ProductDescription, Is.EqualTo("Test Description"));
            Assert.That(PageModel.Product.Image, Is.EqualTo("/assets/test.png"));
            Assert.That(PageModel.Product.Ratings, Is.Not.Null);
        }

        /// <summary>
        /// Test OnGet uses FirstOrDefault for product lookup
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Use_FirstOrDefault_For_Lookup()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product, Is.Not.Null);
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-laptop-1"));
        }

        /// <summary>
        /// Test OnGet with valid product ID verifies Equals method is used
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Use_Equals_For_Comparison()
        {
            // Arrange
            var data = "test-mice-1";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product, Is.Not.Null);
            Assert.That(PageModel.Product.Id.Equals(data), Is.True);
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnGet executes lambda expression in FirstOrDefault
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Execute_Lambda_Expression()
        {
            // Arrange
            var data = "test-keyboard-1";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product, Is.Not.Null);
            Assert.That(PageModel.Product.Id, Is.EqualTo(data));
            Assert.That(result, Is.InstanceOf<PageResult>());
            MockProductService.Verify(x => x.GetProducts(), Times.Once);
        }

        /// <summary>
        /// Test OnGet returns PageResult with correct type
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Return_Correct_PageResult_Type()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            var result = PageModel.OnGet(data);
            var pageResult = result as PageResult;

            // Reset

            // Assert
            Assert.That(result, Is.TypeOf<PageResult>());
            Assert.That(pageResult, Is.Not.Null);
        }

        /// <summary>
        /// Test OnGet with each product verifies all IDs match correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Each_Product_Should_Match_Correctly()
        {
            // Arrange & Act & Assert for first product
            var result1 = PageModel.OnGet("test-laptop-1");
            Assert.That(result1, Is.InstanceOf<PageResult>());
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-laptop-1"));

            // Arrange & Act & Assert for second product
            var result2 = PageModel.OnGet("test-keyboard-1");
            Assert.That(result2, Is.InstanceOf<PageResult>());
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-keyboard-1"));

            // Arrange & Act & Assert for third product
            var result3 = PageModel.OnGet("test-mice-1");
            Assert.That(result3, Is.InstanceOf<PageResult>());
            Assert.That(PageModel.Product.Id, Is.EqualTo("test-mice-1"));

            // Reset
        }

        /// <summary>
        /// Test OnGet with product ID matching first product in list
        /// </summary>
        [Test]
        public void OnGet_Valid_First_Product_Should_Return_First_Product()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Id, Is.EqualTo(TestProducts.First().Id));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnGet with product ID matching last product in list
        /// </summary>
        [Test]
        public void OnGet_Valid_Last_Product_Should_Return_Last_Product()
        {
            // Arrange
            var data = "test-mice-1";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Id, Is.EqualTo(TestProducts.Last().Id));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnGet with product ID matching middle product in list
        /// </summary>
        [Test]
        public void OnGet_Valid_Middle_Product_Should_Return_Middle_Product()
        {
            // Arrange
            var data = "test-keyboard-1";

            // Act
            var result = PageModel.OnGet(data);

            // Reset

            // Assert
            Assert.That(PageModel.Product.Id, Is.EqualTo(TestProducts[1].Id));
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnGet returns Page result and sets Product correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Return_Page_And_Set_Product()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            var result = PageModel.OnGet(data);
            var pageResult = result as PageResult;

            // Reset

            // Assert
            Assert.That(pageResult, Is.Not.Null);
            Assert.That(PageModel.Product, Is.Not.Null);
            Assert.That(PageModel.Product.Id, Is.EqualTo(data));
        }

        /// <summary>
        /// Test OnGet with null ID does not call GetProducts
        /// </summary>
        [Test]
        public void OnGet_Invalid_Null_ProductId_Should_Not_Call_GetProducts()
        {
            // Arrange
            string data = null;
            var mockService = new Mock<JsonFileProductService>(MockBehavior.Strict, null);
            var model = new ReadModel(mockService.Object);

            // Act
            var result = model.OnGet(data);

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToPageResult>());
            var redirectResult = result as RedirectToPageResult;
            Assert.That(redirectResult.PageName, Is.EqualTo("./Index"));
        }

        /// <summary>
        /// Test OnGet with empty ID does not call GetProducts
        /// </summary>
        [Test]
        public void OnGet_Invalid_Empty_ProductId_Should_Not_Call_GetProducts()
        {
            // Arrange
            var data = string.Empty;
            var mockService = new Mock<JsonFileProductService>(MockBehavior.Strict, null);
            var model = new ReadModel(mockService.Object);

            // Act
            var result = model.OnGet(data);
            var redirectResult = result as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.PageName, Is.EqualTo("./Index"));
            mockService.Verify(x => x.GetProducts(), Times.Never);
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

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(MockProductService.Object));
            Assert.That(result, Is.Not.Null);
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

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<JsonFileProductService>());
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

            // Reset

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test Product property is set after OnGet with valid ID
        /// </summary>
        [Test]
        public void Product_After_Valid_OnGet_Should_Be_Set()
        {
            // Arrange
            var data = "test-laptop-1";

            // Act
            PageModel.OnGet(data);
            var result = PageModel.Product;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo("test-laptop-1"));
        }

        /// <summary>
        /// Test Product property remains null after OnGet with invalid ID
        /// </summary>
        [Test]
        public void Product_After_Invalid_OnGet_Should_Remain_Null()
        {
            // Arrange
            var data = "non-existent-id";

            // Act
            PageModel.OnGet(data);
            var result = PageModel.Product;

            // Reset

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test Product property can be accessed directly
        /// </summary>
        [Test]
        public void Product_Valid_Should_Be_Accessible()
        {
            // Arrange
            var data = "test-laptop-1";
            PageModel.OnGet(data);

            // Act
            var result = PageModel.Product;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<ProductModel>());
        }

        #endregion Product
    }
}