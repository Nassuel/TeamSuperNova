using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for ProductsController class
    /// Tests all methods and properties to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class ProductsControllerTests
    {
        #region TestSetup

        private Mock<JsonFileProductService> mockProductService;
        private ProductsController controller;
        private List<ProductModel> testProducts;

        /// <summary>
        /// Initialize test environment before each test
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Arrange - Create test data
            testProducts = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = "1",
                    Brand = "Test Brand 1",
                    ProductName = "Product 1",
                    Ratings = new int[] { 5, 4 }
                },
                new ProductModel
                {
                    Id = "2",
                    Brand = "Test Brand 2",
                    ProductName = "Product 2",
                    Ratings = new int[] { 3, 4, 5 }
                },
                new ProductModel
                {
                    Id = "3",
                    Brand = "Test Brand 3",
                    ProductName = "Product 3",
                    Ratings = null
                }
            };

            // Setup mock service - Use Loose behavior instead of Strict
            mockProductService = new Mock<JsonFileProductService>((Microsoft.AspNetCore.Hosting.IWebHostEnvironment)null);
            mockProductService.Setup(s => s.GetProducts()).Returns(testProducts);

            // Create controller with mock service
            controller = new ProductsController(mockProductService.Object);

            // Reset
        }

        #endregion TestSetup

        #region Constructor

        /// <summary>
        /// Test constructor creates valid instance with service
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Create_Valid_Instance()
        {
            // Arrange - Done in TestInitialize

            // Act
            var testController = new ProductsController(mockProductService.Object);

            // Reset

            // Assert
            Assert.That(testController, Is.Not.Null);
        }

        /// <summary>
        /// Test constructor initializes ProductService property
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Initialize_ProductService_Property()
        {
            // Arrange - Done in TestInitialize

            // Act
            var testController = new ProductsController(mockProductService.Object);

            // Reset

            // Assert
            Assert.That(testController.ProductService, Is.Not.Null);
            Assert.That(testController.ProductService, Is.EqualTo(mockProductService.Object));
        }

        /// <summary>
        /// Test controller has ApiController attribute
        /// </summary>
        [Test]
        public void Controller_Should_Have_ApiController_Attribute()
        {
            // Arrange
            var type = typeof(ProductsController);

            // Act
            var attributes = type.GetCustomAttributes(typeof(ApiControllerAttribute), false);

            // Reset

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(1));
        }

        /// <summary>
        /// Test controller has Route attribute with correct template
        /// </summary>
        [Test]
        public void Controller_Should_Have_Route_Attribute()
        {
            // Arrange
            var type = typeof(ProductsController);

            // Act
            var attributes = type.GetCustomAttributes(typeof(RouteAttribute), false);

            // Reset

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(1));
            var routeAttr = (RouteAttribute)attributes[0];
            Assert.That(routeAttr.Template, Is.EqualTo("[controller]"));
        }

        #endregion Constructor

        #region ProductService

        /// <summary>
        /// Test ProductService property returns injected service
        /// </summary>
        [Test]
        public void ProductService_Property_Should_Return_Injected_Service()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = controller.ProductService;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(mockProductService.Object));
        }

        #endregion ProductService

        #region Get

        /// <summary>
        /// Test Get method returns all products
        /// </summary>
        [Test]
        public void Get_Valid_Should_Return_All_Products()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = controller.Get();

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        /// <summary>
        /// Test Get method returns correct product data
        /// </summary>
        [Test]
        public void Get_Valid_Should_Return_Correct_Product_Data()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = controller.Get().ToList();

            // Reset

            // Assert
            Assert.That(result[0].Id, Is.EqualTo("1"));
            Assert.That(result[0].Brand, Is.EqualTo("Test Brand 1"));
            Assert.That(result[1].Id, Is.EqualTo("2"));
            Assert.That(result[2].Id, Is.EqualTo("3"));
        }

        /// <summary>
        /// Test Get method calls ProductService.GetProducts
        /// </summary>
        [Test]
        public void Get_Valid_Should_Call_ProductService_GetProducts()
        {
            // Arrange - Done in TestInitialize

            // Act
            controller.Get();

            // Reset

            // Assert
            mockProductService.Verify(s => s.GetProducts(), Times.Once);
        }

        /// <summary>
        /// Test Get method returns empty collection when no products
        /// </summary>
        [Test]
        public void Get_Empty_Collection_Should_Return_Empty_Enumerable()
        {
            // Arrange
            mockProductService.Setup(s => s.GetProducts()).Returns(new List<ProductModel>());

            // Act
            var result = controller.Get();

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Test Get method has HttpGet attribute
        /// </summary>
        [Test]
        public void Get_Method_Should_Have_HttpGet_Attribute()
        {
            // Arrange
            var methodInfo = typeof(ProductsController).GetMethod("Get");

            // Act
            var attributes = methodInfo.GetCustomAttributes(typeof(HttpGetAttribute), false);

            // Reset

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(1));
        }

        /// <summary>
        /// Test Get method returns IEnumerable of ProductModel
        /// </summary>
        [Test]
        public void Get_Valid_Should_Return_IEnumerable_ProductModel()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = controller.Get();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<IEnumerable<ProductModel>>());
        }

        #endregion Get

        #region Patch

        /// <summary>
        /// Test Patch method with valid request returns Ok result
        /// </summary>
        [Test]
        public void Patch_Valid_Request_Should_Return_Ok_Result()
        {
            // Arrange
            mockProductService.Setup(s => s.AddRating(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            var request = new RatingRequest
            {
                ProductId = "1",
                Rating = 5
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Test Patch method calls ProductService.AddRating with correct parameters
        /// </summary>
        [Test]
        public void Patch_Valid_Request_Should_Call_AddRating_With_Correct_Parameters()
        {
            // Arrange
            mockProductService.Setup(s => s.AddRating("1", 5))
                .Returns(true);

            var request = new RatingRequest
            {
                ProductId = "1",
                Rating = 5
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            mockProductService.Verify(s => s.AddRating("1", 5), Times.Once);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Test Patch method with different rating values
        /// </summary>
        [Test]
        public void Patch_Valid_Different_Ratings_Should_Call_AddRating()
        {
            // Arrange
            mockProductService.Setup(s => s.AddRating("1", 1))
                .Returns(true);
            mockProductService.Setup(s => s.AddRating("2", 5))
                .Returns(true);

            var request1 = new RatingRequest
            {
                ProductId = "1",
                Rating = 1
            };

            var request2 = new RatingRequest
            {
                ProductId = "2",
                Rating = 5
            };

            // Act
            var result1 = controller.Patch(request1);
            var result2 = controller.Patch(request2);

            // Reset

            // Assert
            mockProductService.Verify(s => s.AddRating("1", 1), Times.Once);
            mockProductService.Verify(s => s.AddRating("2", 5), Times.Once);
            Assert.That(result1, Is.InstanceOf<OkResult>());
            Assert.That(result2, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Test Patch method with null ProductId returns BadRequest
        /// </summary>
        [Test]
        public void Patch_Null_ProductId_Should_Return_BadRequest()
        {
            // Arrange
            var request = new RatingRequest
            {
                ProductId = null,
                Rating = 5
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Test Patch method with empty ProductId returns BadRequest
        /// </summary>
        [Test]
        public void Patch_Empty_ProductId_Should_Return_BadRequest()
        {
            // Arrange
            var request = new RatingRequest
            {
                ProductId = string.Empty,
                Rating = 5
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Test Patch method with invalid rating (less than 0) returns BadRequest
        /// </summary>
        [Test]
        public void Patch_Rating_Less_Than_Zero_Should_Return_BadRequest()
        {
            // Arrange
            var request = new RatingRequest
            {
                ProductId = "1",
                Rating = -1
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Test Patch method with invalid rating (greater than 5) returns BadRequest
        /// </summary>
        [Test]
        public void Patch_Rating_Greater_Than_Five_Should_Return_BadRequest()
        {
            // Arrange
            var request = new RatingRequest
            {
                ProductId = "1",
                Rating = 6
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Test Patch method with rating = 0 returns BadRequest
        /// </summary>
        [Test]
        public void Patch_Rating_Zero_Should_Return_BadRequest()
        {
            // Arrange
            var request = new RatingRequest
            {
                ProductId = "1",
                Rating = 0
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Test Patch method when AddRating is called with valid parameters returns Ok
        /// </summary>
        [Test]
        public void Patch_Valid_Request_Always_Returns_Ok()
        {
            // Arrange
            mockProductService.Setup(s => s.AddRating("1", 4))
                .Returns(true);

            var request = new RatingRequest
            {
                ProductId = "1",
                Rating = 4
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
            mockProductService.Verify(s => s.AddRating("1", 4), Times.Once);
        }

        /// <summary>
        /// Test Patch method still returns Ok even when AddRating returns false
        /// </summary>
        [Test]
        public void Patch_Returns_Ok_Even_When_AddRating_Returns_False()
        {
            // Arrange
            mockProductService.Setup(s => s.AddRating("nonexistent", 3))
                .Returns(false);

            var request = new RatingRequest
            {
                ProductId = "nonexistent",
                Rating = 3
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
            mockProductService.Verify(s => s.AddRating("nonexistent", 3), Times.Once);
        }

        /// <summary>
        /// Test Patch method with rating boundary values (1 and 5 should work)
        /// </summary>
        [Test]
        public void Patch_Valid_Boundary_Ratings_Should_Work()
        {
            // Arrange
            mockProductService.Setup(s => s.AddRating("1", 1))
                .Returns(true);
            mockProductService.Setup(s => s.AddRating("1", 5))
                .Returns(true);

            var minRatingRequest = new RatingRequest
            {
                ProductId = "1",
                Rating = 1
            };

            var maxRatingRequest = new RatingRequest
            {
                ProductId = "1",
                Rating = 5
            };

            // Act
            var minResult = controller.Patch(minRatingRequest);
            var maxResult = controller.Patch(maxRatingRequest);

            // Reset

            // Assert
            mockProductService.Verify(s => s.AddRating("1", 1), Times.Once);
            mockProductService.Verify(s => s.AddRating("1", 5), Times.Once);
            Assert.That(minResult, Is.InstanceOf<OkResult>());
            Assert.That(maxResult, Is.InstanceOf<OkResult>());
        }

        /// <summary>
        /// Test Patch method has HttpPatch attribute
        /// </summary>
        [Test]
        public void Patch_Method_Should_Have_HttpPatch_Attribute()
        {
            // Arrange
            var methodInfo = typeof(ProductsController).GetMethod("Patch");

            // Act
            var attributes = methodInfo.GetCustomAttributes(typeof(HttpPatchAttribute), false);

            // Reset

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(1));
        }

        /// <summary>
        /// Test Patch method parameter has FromBody attribute
        /// </summary>
        [Test]
        public void Patch_Parameter_Should_Have_FromBody_Attribute()
        {
            // Arrange
            var methodInfo = typeof(ProductsController).GetMethod("Patch");
            var parameter = methodInfo.GetParameters()[0];

            // Act
            var attributes = parameter.GetCustomAttributes(typeof(FromBodyAttribute), false);

            // Reset

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(1));
        }

        /// <summary>
        /// Test Patch method returns ActionResult type
        /// </summary>
        [Test]
        public void Patch_Valid_Should_Return_ActionResult()
        {
            // Arrange
            mockProductService.Setup(s => s.AddRating(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            var request = new RatingRequest
            {
                ProductId = "1",
                Rating = 5
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult>());
        }

        #endregion Patch

        #region RatingRequest

        /// <summary>
        /// Test RatingRequest class exists
        /// </summary>
        [Test]
        public void RatingRequest_Class_Should_Exist()
        {
            // Arrange - No setup needed

            // Act
            var request = new RatingRequest();

            // Reset

            // Assert
            Assert.That(request, Is.Not.Null);
        }

        /// <summary>
        /// Test RatingRequest ProductId property can be set and retrieved
        /// </summary>
        [Test]
        public void RatingRequest_ProductId_Should_Be_Settable()
        {
            // Arrange
            var request = new RatingRequest();

            // Act
            request.ProductId = "test-id";

            // Reset

            // Assert
            Assert.That(request.ProductId, Is.EqualTo("test-id"));
        }

        /// <summary>
        /// Test RatingRequest Rating property can be set and retrieved
        /// </summary>
        [Test]
        public void RatingRequest_Rating_Should_Be_Settable()
        {
            // Arrange
            var request = new RatingRequest();

            // Act
            request.Rating = 5;

            // Reset

            // Assert
            Assert.That(request.Rating, Is.EqualTo(5));
        }

        /// <summary>
        /// Test RatingRequest properties are initially default values
        /// </summary>
        [Test]
        public void RatingRequest_Initial_Properties_Should_Be_Default()
        {
            // Arrange - No setup needed

            // Act
            var request = new RatingRequest();

            // Reset

            // Assert
            Assert.That(request.ProductId, Is.Null);
            Assert.That(request.Rating, Is.EqualTo(0));
        }

        /// <summary>
        /// Test RatingRequest with object initializer
        /// </summary>
        [Test]
        public void RatingRequest_Object_Initializer_Should_Work()
        {
            // Arrange - No setup needed

            // Act
            var request = new RatingRequest
            {
                ProductId = "init-id",
                Rating = 4
            };

            // Reset

            // Assert
            Assert.That(request.ProductId, Is.EqualTo("init-id"));
            Assert.That(request.Rating, Is.EqualTo(4));
        }

        /// <summary>
        /// Test RatingRequest ProductId accepts null
        /// </summary>
        [Test]
        public void RatingRequest_ProductId_Should_Accept_Null()
        {
            // Arrange
            var request = new RatingRequest
            {
                ProductId = "some-id"
            };

            // Act
            request.ProductId = null;

            // Reset

            // Assert
            Assert.That(request.ProductId, Is.Null);
        }

        /// <summary>
        /// Test RatingRequest ProductId accepts empty string
        /// </summary>
        [Test]
        public void RatingRequest_ProductId_Should_Accept_Empty_String()
        {
            // Arrange
            var request = new RatingRequest();

            // Act
            request.ProductId = string.Empty;

            // Reset

            // Assert
            Assert.That(request.ProductId, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Test RatingRequest Rating accepts various integer values
        /// </summary>
        [Test]
        public void RatingRequest_Rating_Should_Accept_Various_Integers()
        {
            // Arrange
            var request = new RatingRequest();

            // Act & Assert
            request.Rating = -1;
            Assert.That(request.Rating, Is.EqualTo(-1));

            request.Rating = 0;
            Assert.That(request.Rating, Is.EqualTo(0));

            request.Rating = 3;
            Assert.That(request.Rating, Is.EqualTo(3));

            request.Rating = 100;
            Assert.That(request.Rating, Is.EqualTo(100));

            // Reset
        }

        #endregion RatingRequest

        #region Integration

        #endregion Integration

        #region AdditionalCoverage

        /// <summary>
        /// Test Patch with null request should return BadRequest
        /// </summary>
        [Test]
        public void Patch_Null_Request_Should_Return_BadRequest()
        {
            // Arrange
            RatingRequest request = null;

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        /// <summary>
        /// Test Patch with whitespace ProductId should return BadRequest
        /// </summary>
        [Test]
        public void Patch_Whitespace_ProductId_Should_Return_BadRequest()
        {
            // Arrange
            var request = new RatingRequest
            {
                ProductId = "   ",
                Rating = 3
            };

            // Act
            var result = controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        #endregion AdditionalCoverage
    }
}