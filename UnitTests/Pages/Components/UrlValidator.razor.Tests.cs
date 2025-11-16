using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages.Components
{
    /// <summary>
    /// Unit tests for ProductList Blazor component
    /// Tests all component functionality to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class UrlValidatorTests
    {
        #region TestSetup

        // Bunit test context - created fresh for each test
        private BunitContext _testContext;

        // Mock product service
        private Mock<JsonFileProductService> MockProductService;

        // Test product data
        private List<ProductModel> TestProducts;

        /// <summary>
        /// Initialize test environment before each test
        /// Creates mock service and test data
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create a fresh TestContext for each test
            _testContext = new BunitContext();

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
                    Ratings = new int[] { 4, 5 }
                },
                new ProductModel
                {
                    Id = "test-monitor-1",
                    Brand = "MonitorNature",
                    ProductName = "Gaming Monitor",
                    ProductType = ProductTypeEnum.Laptop,
                    Url = "https://monitor.com",
                    ProductDescription = "Gaming monitor",
                    Image = "/assets/monitor.png",
                    Ratings = new int[] { 3, 4, 5 }
                },
                new ProductModel
                {
                    Id = "test-mouse-1",
                    Brand = "MouseBrand",
                    ProductName = "Gaming Mouse",
                    ProductType = ProductTypeEnum.Mice,
                    Url = "https://mouse.com",
                    ProductDescription = "Gaming mouse",
                    Image = "/assets/mouse.png",
                    Ratings = null
                }
            };

            // Create mock product service
            MockProductService = new Mock<JsonFileProductService>(MockBehavior.Strict, null);

            // Setup GetProducts to return test data
            MockProductService.Setup(x => x.GetProducts()).Returns(TestProducts);

            // Setup AddRating to return true
            MockProductService.Setup(x => x.AddRating(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            // Register mock service with bUnit test context BEFORE rendering any component
            _testContext.Services.AddSingleton<JsonFileProductService>(MockProductService.Object);
        }

        /// <summary>
        /// Clean up after each test
        /// </summary>
        [TearDown]
        public void TestCleanup()
        {
            _testContext?.Dispose();
        }

        #endregion TestSetup

    }
}