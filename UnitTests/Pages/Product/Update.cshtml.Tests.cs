using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for UpdateModel class
    /// Tests all methods and properties to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class UpdateModelTests
    {
        #region TestSetup

        // Page model instance for testing
        public UpdateModel PageModel;

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
            PageModel = new UpdateModel(MockProductService.Object);
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
            Assert.That(MockProductService.Object, Is.EqualTo(result));
        }

        /// <summary>
        /// Test constructor creates valid instance
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Create_Valid_Instance()
        {
            // Arrange - Done in TestInitialize

            // Act - PageModel created in TestInitialize

            // Reset

            // Assert
            Assert.That(PageModel, Is.Not.Null);
            Assert.That(PageModel.ProductService, Is.Not.Null);
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
            Assert.That("test-laptop-1", Is.EqualTo(PageModel.Product.Id));
            Assert.That("TestBrand", Is.EqualTo(PageModel.Product.Brand));
            Assert.That("Test Laptop", Is.EqualTo(PageModel.Product.ProductName));
        }

        /// <summary>
        /// Test OnGet with valid product ID loads all product properties
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
            Assert.That("test-laptop-1", Is.EqualTo(PageModel.Product.Id));
            Assert.That("TestBrand", Is.EqualTo(PageModel.Product.Brand));
            Assert.That("Test Laptop", Is.EqualTo(PageModel.Product.ProductName));
            Assert.That(ProductTypeEnum.Laptop, Is.EqualTo(PageModel.Product.ProductType));
            Assert.That("https://test.com", Is.EqualTo(PageModel.Product.Url));
            Assert.That("Test Description", Is.EqualTo(PageModel.Product.ProductDescription));
            Assert.That("/assets/test.png", Is.EqualTo(PageModel.Product.Image));
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
            Assert.That(3, Is.EqualTo(PageModel.Product.Ratings.Length));
            Assert.That(5, Is.EqualTo(PageModel.Product.Ratings[0]));
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
            Assert.That("test-mice-1", Is.EqualTo(PageModel.Product.Id));
            Assert.That("MiceBrand", Is.EqualTo(PageModel.Product.Brand));
            Assert.That(ProductTypeEnum.Mice, Is.EqualTo(PageModel.Product.ProductType));
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
            Assert.That("/Product/Index", Is.EqualTo(result.PageName));
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
        /// Test OnGet with empty string product ID redirects to Index
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
            Assert.That("/Product/Index", Is.EqualTo(result.PageName));
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
        /// Test OnGet with non-existent product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_NonExistent_ProductId_Should_Return_RedirectToPageResult()
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
        /// Test OnGet with non-existent product ID redirects to Index
        /// </summary>
        [Test]
        public void OnGet_Invalid_NonExistent_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            var data = "non-existent-id";

            // Act
            var result = PageModel.OnGet(data) as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That("/Product/Index", Is.EqualTo(result.PageName));
        }

        /// <summary>
        /// Test OnGet with non-existent product ID does not set Product
        /// </summary>
        [Test]
        public void OnGet_Invalid_NonExistent_ProductId_Should_Not_Set_Product()
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
            Assert.That("test-laptop-1", Is.EqualTo(PageModel.Product.Id));
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
            MockProductService.Verify(x => x.GetProducts(), Times.Once);
        }

        #endregion OnGet

        #region OnPostAsync

        /// <summary>
        /// Test OnPostAsync with valid data updates product and redirects
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Product_Should_Update_And_Redirect_To_Index()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "UpdatedBrand",
                ProductName = "Updated Laptop"
            };
            PageModel.ModelState.Clear();

            MockProductService.Setup(x => x.SaveUploadedFileAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync((string)null);
            MockProductService.Setup(x => x.UpdateData(It.IsAny<ProductModel>()))
                .Returns(true);

            // Act
            var result = await PageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var redirectResult = result as RedirectToPageResult;
            Assert.That("/Product/Index", Is.EqualTo(redirectResult.PageName));
        }

        /// <summary>
        /// Test OnPostAsync calls SaveUploadedFileAsync
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Should_Call_SaveUploadedFileAsync()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "TestBrand"
            };
            PageModel.ModelState.Clear();

            MockProductService.Setup(x => x.SaveUploadedFileAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("/assets/test.png");
            MockProductService.Setup(x => x.UpdateData(It.IsAny<ProductModel>()))
                .Returns(true);

            // Act
            var result = await PageModel.OnPostAsync();
            var redirectResult = result as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult, Is.InstanceOf<RedirectToPageResult>());
            MockProductService.Verify(x => x.SaveUploadedFileAsync(It.IsAny<IFormFile>()), Times.Once);
        }

        /// <summary>
        /// Test OnPostAsync sets Product Image from SaveUploadedFileAsync
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Should_Set_Product_Image_From_Upload()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "TestBrand"
            };
            PageModel.ModelState.Clear();

            var data = "/assets/uploaded-image.png";
            MockProductService.Setup(x => x.SaveUploadedFileAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync(data);
            MockProductService.Setup(x => x.UpdateData(It.IsAny<ProductModel>()))
                .Returns(true);

            // Act
            var result = await PageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(data, Is.EqualTo(PageModel.Product.Image));
        }

        /// <summary>
        /// Test OnPostAsync calls UpdateData service method
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Should_Call_UpdateData()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "TestBrand"
            };
            PageModel.ModelState.Clear();

            MockProductService.Setup(x => x.SaveUploadedFileAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync((string)null);
            MockProductService.Setup(x => x.UpdateData(It.IsAny<ProductModel>()))
                .Returns(true);

            // Act
            var result = await PageModel.OnPostAsync();
            var redirectResult = result as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult, Is.InstanceOf<RedirectToPageResult>());
            MockProductService.Verify(x => x.UpdateData(PageModel.Product), Times.Once);
        }

        /// <summary>
        /// Test OnPostAsync with null Product returns RedirectToPageResult
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_Null_Product_Should_Return_RedirectToPageResult()
        {
            // Arrange
            PageModel.Product = null;

            // Act
            var result = await PageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnPostAsync with null Product redirects to Index
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_Null_Product_Should_Redirect_To_Index()
        {
            // Arrange
            PageModel.Product = null;

            // Act
            var result = await PageModel.OnPostAsync();
            var redirectResult = result as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That("/Product/Index", Is.EqualTo(redirectResult.PageName));
        }

        /// <summary>
        /// Test OnPostAsync with invalid ModelState returns Page result
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_ModelState_Should_Return_Page_Result()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "TestBrand"
            };
            PageModel.ModelState.AddModelError("TestError", "Test error message");

            // Act
            var result = await PageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnPostAsync with invalid ModelState does not call UpdateData
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_ModelState_Should_Not_Call_UpdateData()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "TestBrand"
            };
            PageModel.ModelState.AddModelError("TestError", "Test error message");

            MockProductService.Setup(x => x.SaveUploadedFileAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync((string)null);
            MockProductService.Setup(x => x.UpdateData(It.IsAny<ProductModel>()))
                .Returns(true);

            // Act
            var result = await PageModel.OnPostAsync();

            // Reset

            // Assert
            MockProductService.Verify(x => x.UpdateData(It.IsAny<ProductModel>()), Times.Never);
        }

        /// <summary>
        /// Test OnPostAsync with multiple ModelState errors returns Page result
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_Multiple_ModelState_Errors_Should_Return_Page_Result()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "TestBrand"
            };
            PageModel.ModelState.AddModelError("Error1", "First error");
            PageModel.ModelState.AddModelError("Error2", "Second error");

            // Act
            var result = await PageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnPostAsync with valid data and null image path
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_With_Null_Image_Path_Should_Update_Product()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "TestBrand"
            };
            PageModel.ModelState.Clear();

            MockProductService.Setup(x => x.SaveUploadedFileAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync((string)null);
            MockProductService.Setup(x => x.UpdateData(It.IsAny<ProductModel>()))
                .Returns(true);

            // Act
            var result = await PageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(PageModel.Product.Image, Is.Null);
        }

        /// <summary>
        /// Test OnPostAsync updates product with complete data
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Complete_Product_Should_Update_All_Properties()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "UpdatedBrand",
                ProductName = "Updated Laptop",
                ProductType = ProductTypeEnum.Laptop,
                Url = "https://updated.com",
                ProductDescription = "Updated Description",
                Ratings = new int[] { 5 }
            };
            PageModel.ModelState.Clear();

            MockProductService.Setup(x => x.SaveUploadedFileAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync("/assets/updated.png");
            MockProductService.Setup(x => x.UpdateData(It.IsAny<ProductModel>()))
                .Returns(true);

            // Act
            var result = await PageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(PageModel.Product.Brand, Is.EqualTo("UpdatedBrand"));
            Assert.That(PageModel.Product.ProductName, Is.EqualTo("Updated Laptop"));
            Assert.That(PageModel.Product.Image, Is.EqualTo("/assets/updated.png"));
        }

        /// <summary>
        /// Test OnPostAsync with ImageFile property saves image file
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_With_ImageFile_Should_Save_Image()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test-laptop-1",
                Brand = "TestBrand"
            };

            // Mock image file
            var mockImageFile = new Mock<IFormFile>();
            mockImageFile.Setup(f => f.Length).Returns(100);
            PageModel.ImageFile = mockImageFile.Object;
            PageModel.ModelState.Clear();

            MockProductService.Setup(x => x.SaveUploadedFileAsync(mockImageFile.Object))
                .ReturnsAsync("/assets/new-image.png");
            MockProductService.Setup(x => x.UpdateData(It.IsAny<ProductModel>()))
                .Returns(true);

            // Act
            var result = await PageModel.OnPostAsync();
            var redirectResult = result as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult, Is.InstanceOf<RedirectToPageResult>());
            MockProductService.Verify(x => x.SaveUploadedFileAsync(mockImageFile.Object), Times.Once);
            Assert.That("/assets/new-image.png", Is.EqualTo(PageModel.Product.Image));
        }

        #endregion OnPostAsync

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
            Assert.That(MockProductService.Object, Is.EqualTo(result));
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
            var newPageModel = new UpdateModel(MockProductService.Object);

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
            Assert.That("test-laptop-1", Is.EqualTo(result.Id));
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
            Assert.That(null, Is.EqualTo(result));
        }

        /// <summary>
        /// Test Product property has BindProperty attribute
        /// </summary>
        [Test]
        public void Product_Should_Have_BindProperty_Attribute()
        {
            // Arrange
            var propertyInfo = typeof(UpdateModel).GetProperty("Product");

            // Act
            var attributes = propertyInfo.GetCustomAttributes(typeof(BindPropertyAttribute), false);

            // Reset

            // Assert
            Assert.That(1, Is.EqualTo(attributes.Length));
        }

        #endregion Product

        #region ImageFile

        /// <summary>
        /// Test ImageFile property is initially null
        /// </summary>
        [Test]
        public void ImageFile_Initial_Should_Be_Null()
        {
            // Arrange
            var newPageModel = new UpdateModel(MockProductService.Object);

            // Act
            var result = newPageModel.ImageFile;

            // Reset

            // Assert
            Assert.That(null, Is.EqualTo(result));
        }

        /// <summary>
        /// Test ImageFile property can be set
        /// </summary>
        [Test]
        public void ImageFile_Valid_Should_Be_Settable()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();

            // Act
            PageModel.ImageFile = mockFile.Object;

            // Reset

            // Assert
            Assert.That(mockFile.Object, Is.EqualTo(PageModel.ImageFile));
        }

        /// <summary>
        /// Test ImageFile property has BindProperty attribute
        /// </summary>
        [Test]
        public void ImageFile_Should_Have_BindProperty_Attribute()
        {
            // Arrange
            var propertyInfo = typeof(UpdateModel).GetProperty("ImageFile");

            // Act
            var attributes = propertyInfo.GetCustomAttributes(typeof(BindPropertyAttribute), false);

            // Reset

            // Assert
            Assert.That(1, Is.EqualTo(attributes.Length));
        }

        #endregion ImageFile
    }
}