using ContosoCrafts.WebSite.Enums;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for CreateModel class
    /// Tests all methods and properties to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class CreateTests
    {
        #region TestSetup

        public static CreateModel pageModel;

        /// <summary>
        /// Initialize test environment before each test
        /// Creates page model with TestHelper service
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new CreateModel(TestHelper.ProductService)
            {
            };

            pageModel.OnGet();
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
            var result = pageModel.ProductService;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(TestHelper.ProductService));
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
            Assert.That(pageModel, Is.Not.Null);
            Assert.That(pageModel.ProductService, Is.Not.Null);
        }

        #endregion Constructor

        #region OnGet

        /// <summary>
        /// Test OnGet returns Page result
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Page_Result()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnGet initializes Product property
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Initialize_Product_Property()
        {
            // Arrange - Done in TestInitialize

            // Act

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.Product, Is.Not.Null);
        }

        /// <summary>
        /// Test OnGet initializes Product with GUID Id
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Initialize_Product_With_Guid_Id()
        {
            // Arrange - Done in TestInitialize

            // Act

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.Product.Id, Is.Not.Null);
            Assert.That(pageModel.FormModel.Product.Id, Is.Not.Empty);
            // Verify it's a valid GUID format
            Assert.That(Guid.TryParse(pageModel.FormModel.Product.Id, out _), Is.True);
        }

        /// <summary>
        /// Test OnGet initializes Product with all default properties
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Initialize_Product_With_Default_Properties()
        {
            // Arrange - Done in TestInitialize

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.Product.Id, Is.Not.Null);
            Assert.That(pageModel.FormModel.Product.Brand, Is.EqualTo(""));
            Assert.That(pageModel.FormModel.Product.ProductName, Is.EqualTo(""));
            Assert.That(pageModel.FormModel.Product.ProductType, Is.EqualTo(ProductTypeEnum.Undefined));
            Assert.That(pageModel.FormModel.Product.Url, Is.EqualTo(""));
            Assert.That(pageModel.FormModel.Product.ProductDescription, Is.EqualTo(""));
            Assert.That(pageModel.FormModel.Product.Image, Is.EqualTo(""));
            Assert.That(pageModel.FormModel.Product.Ratings, Is.Null);
        }

        /// <summary>
        /// Test OnGet generates unique GUID for each call
        /// </summary>
        [Test]
        public void OnGet_Valid_Multiple_Calls_Should_Generate_Unique_Guids()
        {
            // Arrange - Done in TestInitialize

            // Act
            pageModel.OnGet();
            var firstId = pageModel.FormModel.Product.Id;

            pageModel.OnGet();
            var secondId = pageModel.FormModel.Product.Id;

            // Reset

            // Assert
            Assert.That(firstId, Is.Not.EqualTo(secondId));
            Assert.That(Guid.TryParse(firstId, out _), Is.True);
            Assert.That(Guid.TryParse(secondId, out _), Is.True);
        }

        /// <summary>
        /// Test OnGet can be called multiple times
        /// </summary>
        [Test]
        public void OnGet_Valid_Multiple_Calls_Should_Initialize_Product_Each_Time()
        {
            // Arrange - Done in TestInitialize

            // Act
            pageModel.OnGet();
            var firstProduct = pageModel.FormModel.Product;

            pageModel.OnGet();
            var secondProduct = pageModel.FormModel.Product;

            // Reset

            // Assert
            Assert.That(firstProduct, Is.Not.Null);
            Assert.That(secondProduct, Is.Not.Null);
            Assert.That(firstProduct, Is.Not.SameAs(secondProduct));
        }

        /// <summary>
        /// Test OnGet initializes all ProductType enum values correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Initialize_ProductType_As_Undefined()
        {
            // Arrange - Done in TestInitialize

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.That((int)pageModel.FormModel.Product.ProductType, Is.EqualTo(0));
            Assert.That(pageModel.FormModel.Product.ProductType, Is.EqualTo(ProductTypeEnum.Undefined));
        }

        #endregion OnGet

        #region OnPostAsync

        /// <summary>
        /// Test OnPostAsync with null FormModel returns RedirectToPageResult
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_Null_FormModel_Should_Return_RedirectToPageResult()
        {
            // Arrange
            pageModel.FormModel = null;

            // Act
            var result = await pageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnPostAsync with null FormModel redirects to Index
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_Null_FormModel_Should_Redirect_To_Index()
        {
            // Arrange
            pageModel.FormModel = null;

            // Act
            var result = await pageModel.OnPostAsync();

            var redirectResult = result as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That("/Product/Index", Is.EqualTo(redirectResult.PageName));
        }

        /// <summary>
        /// Test OnPostAsync with valid data creates product and redirects to Read
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Product_Should_Create_And_Redirect_To_Read()
        {
            // Arrange
            pageModel.FormModel.Product = new ProductModel
            {
                Id = Guid.NewGuid().ToString(),
                Brand = "NewBrand",
                ProductName = "New Product",
                ProductType = ProductTypeEnum.Laptop,
                Url = "https://new.com",
                ProductDescription = "New Description"
            };
            pageModel.ModelState.Clear();

            // Act
            var result = await pageModel.OnPostAsync() as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(result.PageName, Is.EqualTo("/Product/Read"));
        }

        /// <summary>
        /// Test OnPostAsync redirects to Read page with product Id
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Should_Redirect_With_Product_Id()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            pageModel.FormModel.Product = new ProductModel
            {
                Id = productId,
                Brand = "TestBrand",
                ProductName = "Test Product"
            };
            pageModel.ModelState.Clear();

            // Act
            var result = await pageModel.OnPostAsync() as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result.RouteValues, Is.Not.Null);
            Assert.That(result.RouteValues["id"], Is.EqualTo(productId));
        }

        /// <summary>
        /// Test OnPostAsync sets Product Image from SaveUploadedFileAsync
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Should_Set_Product_Image_From_Upload()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            pageModel.FormModel.Product = new ProductModel
            {
                Id = productId,
                Brand = "TestBrand",
                ProductName = "Test Product"
            };

            var mockImageFile = new Mock<IFormFile>();
            mockImageFile.Setup(f => f.Length).Returns(100);
            mockImageFile.Setup(f => f.FileName).Returns("test.png");
            pageModel.FormModel.ImageFile = mockImageFile.Object;
            pageModel.ModelState.Clear();

            // Act
            await pageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.Product.Image, Is.Not.Null);
        }

        /// <summary>
        /// Test OnPostAsync adds product to service
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Should_Add_Product_To_Service()
        {
            // Arrange
            var initialCount = TestHelper.ProductService.GetProducts().Count();
            var productId = Guid.NewGuid().ToString();
            pageModel.FormModel.Product= new ProductModel
            {
                Id = productId,
                Brand = "TestBrand",
                ProductName = "Test Product",
                ProductType = ProductTypeEnum.Laptop
            };
            pageModel.ModelState.Clear();

            // Act
            await pageModel.OnPostAsync();

            // Reset

            // Assert
            var updatedCount = TestHelper.ProductService.GetProducts().Count();
            Assert.That(updatedCount, Is.EqualTo(initialCount + 1));
        }

        /// <summary>
        /// Test OnPostAsync with null Product returns RedirectToPageResult
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_Null_Product_Should_Return_RedirectToPageResult()
        {
            // Arrange
            pageModel.FormModel.Product = null;

            // Act
            var result = await pageModel.OnPostAsync();

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
            pageModel.FormModel.Product= null;

            // Act
            var result = await pageModel.OnPostAsync() as RedirectToPageResult;

            // Reset

            // Assert
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
        }

        /// <summary>
        /// Test OnPostAsync with invalid ModelState returns Page result
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_ModelState_Should_Return_Page_Result()
        {
            // Arrange
            pageModel.FormModel.Product= new ProductModel
            {
                Id = Guid.NewGuid().ToString(),
                Brand = "TestBrand"
            };
            pageModel.ModelState.AddModelError("TestError", "Test error message");

            // Act
            var result = await pageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnPostAsync with invalid ModelState does not add product
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_ModelState_Should_Not_Add_Product()
        {
            // Arrange
            var initialCount = TestHelper.ProductService.GetProducts().Count();
            pageModel.FormModel.Product= new ProductModel
            {
                Id = Guid.NewGuid().ToString(),
                Brand = "TestBrand"
            };
            pageModel.ModelState.AddModelError("TestError", "Test error message");

            // Act
            await pageModel.OnPostAsync();

            // Reset

            // Assert
            var updatedCount = TestHelper.ProductService.GetProducts().Count();
            Assert.That(updatedCount, Is.EqualTo(initialCount));
        }

        /// <summary>
        /// Test OnPostAsync with multiple ModelState errors returns Page result
        /// </summary>
        [Test]
        public async Task OnPostAsync_Invalid_Multiple_ModelState_Errors_Should_Return_Page_Result()
        {
            // Arrange
            pageModel.FormModel.Product= new ProductModel
            {
                Id = Guid.NewGuid().ToString(),
                Brand = "TestBrand"
            };
            pageModel.ModelState.AddModelError("Error1", "First error");
            pageModel.ModelState.AddModelError("Error2", "Second error");

            // Act
            var result = await pageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Test OnPostAsync with valid data and null image path
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_With_Null_Image_Should_Create_Product()
        {
            // Arrange
            pageModel.FormModel.Product= new ProductModel
            {
                Id = Guid.NewGuid().ToString(),
                Brand = "TestBrand",
                ProductName = "Test Product"
            };
            pageModel.FormModel.ImageFile = null;
            pageModel.ModelState.Clear();

            // Act
            var result = await pageModel.OnPostAsync();

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnPostAsync creates product with complete data
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Complete_Product_Should_Create_All_Properties()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            pageModel.FormModel.Product= new ProductModel
            {
                Id = productId,
                Brand = "NewBrand",
                ProductName = "New Product",
                ProductType = ProductTypeEnum.Laptop,
                Url = "https://new.com",
                ProductDescription = "New Description",
                Ratings = null
            };
            pageModel.ModelState.Clear();

            // Act
            await pageModel.OnPostAsync();

            // Reset

            // Assert
            var createdProduct = TestHelper.ProductService.GetProductById(productId);
            Assert.That(createdProduct, Is.Not.Null);
            Assert.That(createdProduct.Brand, Is.EqualTo("NewBrand"));
            Assert.That(createdProduct.ProductName, Is.EqualTo("New Product"));
        }

        /// <summary>
        /// Test OnPostAsync with different ProductType values
        /// </summary>
        [Test]
        public async Task OnPostAsync_Valid_Different_ProductTypes_Should_Create_Correctly()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            pageModel.FormModel.Product= new ProductModel
            {
                Id = productId,
                Brand = "KeyboardBrand",
                ProductName = "Test Keyboard",
                ProductType = ProductTypeEnum.Keyboard
            };
            pageModel.ModelState.Clear();

            // Act
            await pageModel.OnPostAsync();

            // Reset

            // Assert
            var createdProduct = TestHelper.ProductService.GetProductById(productId);
            Assert.That(createdProduct, Is.Not.Null);
            Assert.That(createdProduct.ProductType, Is.EqualTo(ProductTypeEnum.Keyboard));
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
            var result = pageModel.ProductService;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(TestHelper.ProductService));
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
            var result = pageModel.ProductService;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Test ProductService property can be set
        /// </summary>
        [Test]
        public void ProductService_Valid_Should_Be_Settable()
        {
            // Arrange
            var newService = TestHelper.ProductService;

            // Act
            pageModel.ProductService = newService;

            // Reset

            // Assert
            Assert.That(pageModel.ProductService, Is.EqualTo(newService));
        }

        #endregion ProductService

        #region Product

        /// <summary>
        /// Test FormModel is null before OnGet is called
        /// </summary>
        [Test]
        public void FormModel_Initial_Should_Be_Null()
        {
            // Arrange
            var newPageModel = new CreateModel(TestHelper.ProductService);

            // Act
            var result = newPageModel.FormModel;

            // Reset

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test Product property is set after OnGet
        /// </summary>
        [Test]
        public void Product_After_OnGet_Should_Be_Set()
        {
            // Arrange - Done in TestInitialize

            // Act
            pageModel.OnGet();
            var result = pageModel.FormModel.Product;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.Empty);
        }

        /// <summary>
        /// Test FormModel property has BindProperty attribute
        /// </summary>
        [Test]
        public void FormModel_Should_Have_BindProperty_Attribute()
        {
            // Arrange
            var propertyInfo = typeof(CreateModel).GetProperty("FormModel");

            // Act
            var attributes = propertyInfo.GetCustomAttributes(typeof(BindPropertyAttribute), false);

            // Reset

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(1));
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
            var newPageModel = new CreateModel(TestHelper.ProductService);

            // Act
            newPageModel.OnGet();
            var result = newPageModel.FormModel.ImageFile;

            // Reset

            // Assert
            Assert.That(result, Is.Null);
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

            pageModel.FormModel.ImageFile = mockFile.Object;

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.ImageFile, Is.EqualTo(mockFile.Object));
        }

        /// <summary>
        /// Test ImageFile property on FormModel does not have BindProperty attribute
        /// </summary>
        [Test]
        public void FormModel_ImageFile_Should_Not_Have_BindProperty_Attribute()
        {
            // Arrange
            var propertyInfo = typeof(ProductFormModel).GetProperty("ImageFile");

            // Act
            var attributes = propertyInfo.GetCustomAttributes(typeof(BindPropertyAttribute), false);

            // Reset

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(0));
        }

        #endregion ImageFile

        #region FormModel Placeholders

        /// <summary>
        /// Test OnGet sets BrandPlaceholder correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Set_BrandPlaceholder()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.BrandPlaceholder, Is.EqualTo("Enter the brand of new product"));
        }

        /// <summary>
        /// Test OnGet sets ProductNamePlaceholder correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Set_ProductNamePlaceholder()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.ProductNamePlaceholder, Is.EqualTo("Enter the new product name"));
        }

        /// <summary>
        /// Test OnGet sets DescriptionPlaceholder correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Set_DescriptionPlaceholder()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.DescriptionPlaceholder, Is.EqualTo("Enter Description Here"));
        }

        /// <summary>
        /// Test OnGet sets UrlPlaceholder correctly
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Set_UrlPlaceholder()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(pageModel.FormModel.UrlPlaceholder, Is.EqualTo("Enter URL of Product Website"));
        }

        #endregion FormModel Placeholders

    }

}