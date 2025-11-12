using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit tests for DeleteModel class
    /// Tests all methods and properties to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class DeleteModelTests
    {
        #region TestSetup

        // Page model instance for testing
        public static DeleteModel pageModel;

        /// <summary>
        /// Initialize test environment before each test
        /// Creates page model with TestHelper service
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new DeleteModel(TestHelper.ProductService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test OnGet with valid product ID returns Page result
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Return_Page_Result()
        {
            // Arrange
            var productId = TestHelper.ProductService.GetProducts().First().Id;

            // Act
            var result = pageModel.OnGet(productId);

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
            var productId = TestHelper.ProductService.GetProducts().First().Id;

            // Act
            pageModel.OnGet(productId);

            // Assert
            Assert.That(pageModel.Product, Is.Not.Null);
            Assert.That(pageModel.Product.Id, Is.EqualTo(productId));
        }

        /// <summary>
        /// Test OnGet with valid product ID loads all product properties
        /// </summary>
        [Test]
        public void OnGet_Valid_ProductId_Should_Load_All_Product_Properties()
        {
            // Arrange
            var productId = TestHelper.ProductService.GetProducts().First().Id;

            // Act
            pageModel.OnGet(productId);

            // Assert
            Assert.That(pageModel.Product.Id, Is.Not.Null);
            Assert.That(pageModel.Product.Brand, Is.Not.Null);
            Assert.That(pageModel.Product.ProductName, Is.Not.Null);
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
            var result = pageModel.OnGet(productId);

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
            string productId = null;

            // Act
            var result = pageModel.OnGet(productId) as RedirectToPageResult;

            // Assert
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
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
            var result = pageModel.OnGet(productId);

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
            var productId = string.Empty;

            // Act
            var result = pageModel.OnGet(productId) as RedirectToPageResult;

            // Assert
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
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
            var result = pageModel.OnGet(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnGet with whitespace product ID redirects to Index
        /// </summary>
        [Test]
        public void OnGet_Invalid_Whitespace_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            var productId = "   ";

            // Act
            var result = pageModel.OnGet(productId) as RedirectToPageResult;

            // Assert
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
        }

        /// <summary>
        /// Test OnGet with non-existent product ID returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnGet_Invalid_NonExistent_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            var productId = "non-existent-product-id-12345";

            // Act
            var result = pageModel.OnGet(productId);

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
            var productId = "non-existent-product-id-12345";

            // Act
            var result = pageModel.OnGet(productId) as RedirectToPageResult;

            // Assert
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
        }

        /// <summary>
        /// Test OnGet with non-existent product ID does not set Product
        /// </summary>
        [Test]
        public void OnGet_Invalid_NonExistent_ProductId_Should_Not_Set_Product()
        {
            // Arrange
            var productId = "non-existent-product-id-12345";

            // Act
            pageModel.OnGet(productId);

            // Assert
            Assert.That(pageModel.Product, Is.Null);
        }

        /// <summary>
        /// Test OnGet with different valid products
        /// </summary>
        [Test]
        public void OnGet_Valid_Different_Products_Should_Load_Correctly()
        {
            // Arrange
            var products = TestHelper.ProductService.GetProducts();
            var firstProductId = products.First().Id;
            var lastProductId = products.Last().Id;

            // Act - First product
            pageModel.OnGet(firstProductId);
            var firstProduct = pageModel.Product;

            // Reinitialize
            pageModel = new DeleteModel(TestHelper.ProductService);

            // Act - Last product
            pageModel.OnGet(lastProductId);
            var lastProduct = pageModel.Product;

            // Assert
            Assert.That(firstProduct, Is.Not.Null);
            Assert.That(lastProduct, Is.Not.Null);
            Assert.That(firstProduct.Id, Is.Not.EqualTo(lastProduct.Id));
        }

        #endregion OnGet

        #region OnPost

        /// <summary>
        /// Test OnPost with valid product deletes and redirects to Index
        /// </summary>
        [Test]
        public void OnPost_Valid_Product_Should_Delete_And_Redirect_To_Index()
        {
            // Arrange
            var productId = TestHelper.ProductService.GetProducts().First().Id;
            pageModel.OnGet(productId);

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
        }

        /// <summary>
        /// Test OnPost with valid product removes product from collection
        /// </summary>
        [Test]
        public void OnPost_Valid_Product_Should_Remove_Product()
        {
            // Arrange
            var products = TestHelper.ProductService.GetProducts();
            var initialCount = products.Count();
            var productId = products.First().Id;

            pageModel.OnGet(productId);

            // Act
            pageModel.OnPost();

            // Assert
            var updatedProducts = TestHelper.ProductService.GetProducts();
            Assert.That(updatedProducts.Count(), Is.EqualTo(initialCount - 1));
            Assert.That(updatedProducts.Any(p => p.Id == productId), Is.False);
        }

        /// <summary>
        /// Test OnPost with null Product returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnPost_Invalid_Null_Product_Should_Return_RedirectToPageResult()
        {
            // Arrange
            pageModel.Product = null;

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnPost with null Product redirects to Index
        /// </summary>
        [Test]
        public void OnPost_Invalid_Null_Product_Should_Redirect_To_Index()
        {
            // Arrange
            pageModel.Product = null;

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
        }

        /// <summary>
        /// Test OnPost with Product having null Id returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnPost_Invalid_Null_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            pageModel.Product = new ProductModel
            {
                Id = null,
                Brand = "TestBrand"
            };

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnPost with Product having null Id redirects to Index
        /// </summary>
        [Test]
        public void OnPost_Invalid_Null_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            pageModel.Product = new ProductModel
            {
                Id = null,
                Brand = "TestBrand"
            };

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
        }

        /// <summary>
        /// Test OnPost with Product having empty string Id returns RedirectToPageResult
        /// </summary>
        [Test]
        public void OnPost_Invalid_Empty_ProductId_Should_Return_RedirectToPageResult()
        {
            // Arrange
            pageModel.Product = new ProductModel
            {
                Id = string.Empty,
                Brand = "TestBrand"
            };

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        /// <summary>
        /// Test OnPost with Product having empty string Id redirects to Index
        /// </summary>
        [Test]
        public void OnPost_Invalid_Empty_ProductId_Should_Redirect_To_Index()
        {
            // Arrange
            pageModel.Product = new ProductModel
            {
                Id = string.Empty,
                Brand = "TestBrand"
            };

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
        }

        /// <summary>
        /// Test OnPost deletes specific product without affecting others
        /// </summary>
        [Test]
        public void OnPost_Valid_Should_Only_Delete_Target_Product()
        {
            // Arrange
            var products = TestHelper.ProductService.GetProducts();
            var productToDelete = products.First();
            var otherProduct = products.Last();

            pageModel.OnGet(productToDelete.Id);

            // Act
            pageModel.OnPost();

            // Assert
            var updatedProducts = TestHelper.ProductService.GetProducts();
            Assert.That(updatedProducts.Any(p => p.Id == productToDelete.Id), Is.False);
            Assert.That(updatedProducts.Any(p => p.Id == otherProduct.Id), Is.True);
        }

        #endregion OnPost

        #region Product

        /// <summary>
        /// Test Product property is initially null
        /// </summary>
        [Test]
        public void Product_Initial_Should_Be_Null()
        {
            // Arrange
            var newPageModel = new DeleteModel(TestHelper.ProductService);

            // Act
            var result = newPageModel.Product;

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
            var productId = TestHelper.ProductService.GetProducts().First().Id;

            // Act
            pageModel.OnGet(productId);
            var result = pageModel.Product;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(productId));
        }

        /// <summary>
        /// Test Product property remains null after OnGet with invalid ID
        /// </summary>
        [Test]
        public void Product_After_Invalid_OnGet_Should_Remain_Null()
        {
            // Arrange
            var productId = "non-existent-id-999";

            // Act
            pageModel.OnGet(productId);
            var result = pageModel.Product;

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test Product property has BindProperty attribute
        /// </summary>
        [Test]
        public void Product_Should_Have_BindProperty_Attribute()
        {
            // Arrange
            var propertyInfo = typeof(DeleteModel).GetProperty("Product");

            // Act
            var attributes = propertyInfo.GetCustomAttributes(typeof(BindPropertyAttribute), false);

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(1));
        }

        /// <summary>
        /// Test Product property can be set manually
        /// </summary>
        [Test]
        public void Product_Should_Be_Settable()
        {
            // Arrange
            var testProduct = new ProductModel
            {
                Id = "manual-id",
                Brand = "ManualBrand"
            };

            // Act
            pageModel.Product = testProduct;

            // Assert
            Assert.That(pageModel.Product.Id, Is.EqualTo("manual-id"));
            Assert.That(pageModel.Product.Brand, Is.EqualTo("ManualBrand"));
        }

        #endregion Product
    }
}