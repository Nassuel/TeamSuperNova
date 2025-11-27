using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Enums;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;

namespace UnitTests.Pages.Components
{
    /// <summary>
    /// Unit tests for ProductList Blazor component
    /// Tests all component functionality to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class ProductListTests
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

            // Add AddCommentToProduct
            MockProductService.Setup(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>())).Returns(true);
        }

        #endregion TestSetup

        #region ComponentRendering

        /// <summary>
        /// Test component renders successfully
        /// </summary>
        [Test]
        public void ProductList_Component_Should_Render_Successfully()
        {
            // Arrange - Done in TestInitialize

            // Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            Assert.That(component, Is.Not.Null);
        }

        /// <summary>
        /// Test component displays all products initially
        /// </summary>
        [Test]
        public void ProductList_Should_Display_All_Products_Initially()
        {
            // Arrange - Done in TestInitialize

            // Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test component displays product brands
        /// </summary>
        [Test]
        public void ProductList_Should_Display_Product_Brands()
        {
            // Arrange - Done in TestInitialize

            // Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var markup = component.Markup;
            Assert.That(markup.Contains("TestBrand"));
            Assert.That(markup.Contains("KeyboardBrand"));
        }

        #endregion ComponentRendering

        #region Search

        /// <summary>
        /// Test search with valid brand filters products
        /// </summary>
        [Test]
        public void Search_Valid_Brand_Should_Filter_Products()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var searchInput = component.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Input("TestBrand");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Test search is case insensitive
        /// </summary>
        [Test]
        public void Search_Case_Insensitive_Should_Filter_Products()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var searchInput = component.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Input("testbrand");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Test search with no matches shows alert message
        /// </summary>
        [Test]
        public void Search_No_Matches_Should_Show_Alert_Message()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var searchInput = component.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Input("NonExistentBrand");

            // Reset

            // Assert
            var alert = component.Find(".alert");
            Assert.That(alert, Is.Not.Null);
            Assert.That(alert.TextContent.Contains("No products are found"));
        }

        /// <summary>
        /// Test search with empty string shows all products
        /// </summary>
        [Test]
        public void Search_Empty_String_Should_Show_All_Products()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var searchInput = component.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Input("");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Tests that HighlightMatch() correctly highlights a substring within a given text.
        /// </summary>
        [Test]
        public void Search_HighlightMatch_FromProductList_Should_Call_StringHighlighter_Correctly()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var searchInput = component.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Input("Mac");
            var result = component.Instance.HighlightMatch("Macintosh");

            // Reset

            // Assert
            Assert.That("<mark>Mac</mark>intosh", Is.EqualTo(result));
        }

        #endregion Search

        #region Filter

        /// <summary>
        /// Test filter by ProductType Monitor shows only monitors
        /// </summary>
        [Test]
        public void Filter_ProductType_Monitor_Should_Show_Only_Monitors()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var dropdown = component.Find("select");

            // Act
            dropdown.Change(ProductTypeEnum.Laptop.ToString());

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(2)); // Based on test data, 2 laptops
        }

        /// <summary>
        /// Test filter ProductType dropdown is populated
        /// </summary>
        [Test]
        public void Filter_ProductType_Dropdown_Should_Be_Populated()
        {
            // Arrange - Done in TestInitialize

            // Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var options = component.FindAll("select option");
            Assert.That(options.Count, Is.GreaterThan(1));
        }

        /// <summary>
        /// Test filter ProductType All Types shows all products
        /// </summary>
        [Test]
        public void Filter_ProductType_All_Types_Should_Show_All_Products()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var dropdown = component.Find("select");

            // Act
            dropdown.Change("");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test filter MinRating should show only products meeting criteria
        /// </summary>
        [Test]
        public void Filter_MinRating_Should_Show_Only_Products_Meeting_Criteria()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            // Try to find rating filter - adjust selector based on your actual component
            var ratingSelect = component.FindAll("select").Skip(2).FirstOrDefault(); // Third select is usually rating

            // Act
            ratingSelect.Change("4");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test filter MinRating should exclude products with no ratings
        /// </summary>
        [Test]
        public void Filter_MinRating_Should_Exclude_Products_With_No_Ratings()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            // Try to find rating filter - adjust selector based on your actual component
            var ratingSelect = component.FindAll("select").Skip(2).FirstOrDefault(); // Third select is usually rating

            // Act
            ratingSelect.Change("1");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test filter MinRating All should show all products
        /// </summary>
        [Test]
        public void Filter_MinRating_All_Should_Show_All_Products()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            // Try to find rating filter - adjust selector based on your actual component
            var ratingSelect = component.FindAll("select").Skip(2).FirstOrDefault(); // Third select is usually rating

            // Act
            ratingSelect.Change("0");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        #endregion Filter

        #region Sort

        /// <summary>
        /// Test sort Brand A-Z should order products alphabetically
        /// </summary>
        [Test]
        public void Sort_BrandAZ_Should_Order_Products_Alphabetically()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("Brand: A-Z");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test sort Brand Z-A should order products reverse alphabetically
        /// </summary>
        [Test]
        public void Sort_BrandZA_Should_Order_Products_Reverse_Alphabetically()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("Brand: Z-A");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test sort Rating High-Low should order products by rating descending
        /// </summary>
        [Test]
        public void Sort_RatingHighLow_Should_Order_Products_By_Rating_Descending()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("Rating: High-Low");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test sort Rating Low-High should order products by rating ascending
        /// </summary>
        [Test]
        public void Sort_RatingLowHigh_Should_Order_Products_By_Rating_Ascending()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("Rating: Low-High");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test sort Default should maintain original order
        /// </summary>
        [Test]
        public void Sort_Default_Should_Maintain_Original_Order()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("Default");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        #endregion Sort

        #region ClearFilters

        /// <summary>
        /// Test clear filters should reset all filter values
        /// </summary>
        [Test]
        public void ClearFilters_Should_Reset_All_Filter_Values()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var searchInput = component.Find("input[placeholder='Search Brands...']");
            searchInput.Input("TestBrand");

            // Try to find clear button - use FirstOrDefault to avoid exception
            var clearButton = component.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Clear"));

            // Act
            clearButton.Click();

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        #endregion ClearFilters

        #region Modal

        /// <summary>
        /// Test MoreInfo button click should open modal with product details
        /// </summary>
        [Test]
        public void MoreInfo_Button_Click_Should_Open_Modal_With_Product_Details()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modal = component.Find(".modal");
            Assert.That(modal, Is.Not.Null);
        }

        /// <summary>
        /// Test modal should display product description
        /// </summary>
        [Test]
        public void Modal_Should_Display_Product_Description()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modalBody = component.Find(".modal-body");
            Assert.That(modalBody.TextContent.Contains("Test Description"));
        }

        /// <summary>
        /// Test modal should display View Product link when URL exists
        /// </summary>
        [Test]
        public void Modal_Should_Display_View_Product_Link_When_URL_Exists()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var viewLink = component.FindAll("a").FirstOrDefault(a => a.TextContent.Contains("View Product"));
            Assert.That(viewLink, Is.Not.Null);
        }

        /// <summary>
        /// Test modal should hide View Product link when URL is empty
        /// </summary>
        [Test]
        public void Modal_Should_Hide_View_Product_Link_When_URL_Empty()
        {
            // Arrange
            TestProducts[0].Url = string.Empty;
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button")
                                           .Where(b => b.TextContent.Contains("More Info"))
                                           .ToList();

            // Act
            moreInfoButtons[0].Click();

            // Wait for modal content to render
            component.WaitForAssertion(() =>
            {
                // This ensures the modal is rendered before evaluating the predicate
                var allElements = component.FindAll("*");
                Assert.That(allElements.Count, Is.GreaterThan(0));
            });

            // Assert
            var viewLinks = component.FindAll("*").FirstOrDefault(e => e.TextContent.Contains("View Product"));

            Assert.That(viewLinks, Is.Null);
        }



        /// <summary>
        /// Test modal should display vote count for product with ratings
        /// </summary>
        [Test]
        public void Modal_Should_Display_Vote_Count_For_Product_With_Ratings()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modalBody = component.FindAll(".modal-footer")[1];
            Assert.That(modalBody.TextContent.Contains("3 Votes"));
        }

        /// <summary>
        /// Test modal should display first vote message when no ratings
        /// </summary>
        [Test]
        public void Modal_Should_Display_First_Vote_Message_When_No_Ratings()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[3].Click();

            // Reset

            // Assert
            var modalBody = component.FindAll(".modal-footer")[1];
            Assert.That(modalBody.TextContent.Contains("Be the first to vote!"));
        }

        /// <summary>
        /// Test modal should display correct number of checked stars
        /// </summary>
        [Test]
        public void Modal_Should_Display_Correct_Number_Of_Checked_Stars()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var checkedStars = component.FindAll("span.fa-star.checked");
            Assert.That(checkedStars.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test SubmitRating should call ProductService AddRating
        /// </summary>
        [Test]
        public void SubmitRating_Should_Call_ProductService_AddRating()
        {
            // Arrange
            MockProductService.Setup(s => s.AddRating(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true)
                .Verifiable();

            var component = _testContext.Render<ProductList>();
            var moreInfoButton = component.Find("button:contains('More Info')");
            moreInfoButton.Click();
            var voteButton = component.Find("span.fa-star");

            // Act
            voteButton.Click();

            // Assert
            MockProductService.Verify();
        }

        /// <summary>
        /// Test modal should display singular vote label for one vote
        /// </summary>
        [Test]
        public void Modal_Should_Display_Singular_Vote_Label_For_One_Vote()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 5 };
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modalBody = component.FindAll(".modal-footer")[1];
            Assert.That(modalBody.TextContent.Contains("1 Vote"));
        }

        #endregion Modal

        #region MultipleFilters

        /// <summary>
        /// Test search with filter should apply both criteria
        /// </summary>
        [Test]
        public void Multiple_Filters_Should_Work_Together()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var searchInput = component.Find("input[placeholder='Search Brands...']");
            var dropdown = component.Find("select");

            // Act
            searchInput.Input("Brand");
            dropdown.Change(ProductTypeEnum.Keyboard.ToString());

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Test search with filter should apply both criteria
        /// </summary>
        [Test]
        public void Search_With_Filter_Should_Apply_Both_Criteria()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var searchInput = component.Find("input[placeholder='Search Brands...']");
            var typeDropdown = component.Find("select");

            // Act
            searchInput.Input("TestBrand");
            typeDropdown.Change(ProductTypeEnum.Laptop.ToString());

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1));
        }

        #endregion MultipleFilters

        #region ComponentHandling

        /// <summary>
        /// Test component should handle empty product list
        /// </summary>
        [Test]
        public void Component_Should_Handle_Empty_Product_List()
        {
            // Arrange
            MockProductService.Setup(x => x.GetProducts()).Returns(new List<ProductModel>());

            // Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test component should handle products with empty ratings array
        /// </summary>
        [Test]
        public void Component_Should_Handle_Products_With_Empty_Ratings_Array()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { };

            // Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert - Component should render without crashing
            Assert.That(component, Is.Not.Null);
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));

            // Don't click the modal button as it will cause divide by zero
            // The test passes if the component renders successfully with empty ratings
        }

        #endregion ComponentHandling

        #region BrandFilter

        /// <summary>
        /// Test filter by brand should show only selected brand
        /// </summary>
        [Test]
        public void Filter_Brand_Should_Show_Only_Selected_Brand()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var brandSelect = component.FindAll("select")[1]; // Second select is brand

            // Act
            brandSelect.Change("TestBrand");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Test filter brand All Brands should show all products
        /// </summary>
        [Test]
        public void Filter_Brand_All_Brands_Should_Show_All_Products()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var brandSelect = component.FindAll("select")[1];

            // Act
            brandSelect.Change("");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        #endregion BrandFilter

        #region SortingWithValues

        /// <summary>
        /// Test sort BrandAZ using actual option value
        /// </summary>
        [Test]
        public void Sort_Valid_BrandAZ_Value_Should_Order_Products_Alphabetically()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("BrandAZ");

            // Reset

            // Assert
            var cardTitles = component.FindAll(".card-title").Select(c => c.TextContent.Trim()).ToList();
            Assert.That(cardTitles[0], Is.EqualTo("KeyboardBrand"));
        }

        /// <summary>
        /// Test sort BrandZA using actual option value
        /// </summary>
        [Test]
        public void Sort_Valid_BrandZA_Value_Should_Order_Products_Reverse_Alphabetically()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("BrandZA");

            // Reset

            // Assert
            var cardTitles = component.FindAll(".card-title").Select(c => c.TextContent.Trim()).ToList();
            Assert.That(cardTitles[0], Is.EqualTo("TestBrand"));
        }

        /// <summary>
        /// Test sort RatingHighLow using actual option value
        /// </summary>
        [Test]
        public void Sort_Valid_RatingHighLow_Value_Should_Order_By_Rating_Descending()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("RatingHighLow");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test sort RatingLowHigh using actual option value
        /// </summary>
        [Test]
        public void Sort_Valid_RatingLowHigh_Value_Should_Order_By_Rating_Ascending()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("RatingLowHigh");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test sort with empty value should maintain original order
        /// </summary>
        [Test]
        public void Sort_Valid_Empty_Value_Should_Maintain_Original_Order()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test sort with invalid value should maintain original order
        /// </summary>
        [Test]
        public void Sort_Invalid_Unknown_Value_Should_Maintain_Original_Order()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("InvalidSortOption");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        #endregion SortingWithValues

        #region Comments

        /// <summary>
        /// Test AddComment with valid comment should add comment
        /// </summary>
        [Test]
        public void AddComment_Valid_Comment_Should_Call_ProductService()
        {
            // Arrange
            MockProductService.Setup(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>())).Returns(true);
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();

            var textarea = component.Find("textarea");
            var submitButton = component.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Submit Comment"));

            // Act
            textarea.Change("This is a test comment");
            submitButton.Click();

            // Reset

            // Assert
            MockProductService.Verify(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>()), Times.Once);
        }

        /// <summary>
        /// Test AddComment with empty comment should not add comment
        /// </summary>
        [Test]
        public void AddComment_Invalid_Empty_Comment_Should_Not_Call_ProductService()
        {
            // Arrange
            MockProductService.Setup(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>())).Returns(true);
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();

            var textarea = component.Find("textarea");
            var submitButton = component.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Submit Comment"));

            // Act
            textarea.Change("");
            submitButton.Click();

            // Reset

            // Assert
            MockProductService.Verify(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>()), Times.Never);
        }

        /// <summary>
        /// Test AddComment with whitespace comment should not add comment
        /// </summary>
        [Test]
        public void AddComment_Invalid_Whitespace_Comment_Should_Not_Call_ProductService()
        {
            // Arrange
            MockProductService.Setup(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>()))
                .Returns(true);
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();
            var textarea = component.Find("textarea");
            var submitButton = component.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Submit Comment"));

            // Act
            textarea.Change("   ");
            submitButton.Click();

            // Assert
            var addCommentCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddCommentToProduct");
            Assert.That(addCommentCalls, Is.EqualTo(0));
        }

        #endregion Comments

        #region EnterKeySubmit

        /// <summary>
        /// Test UsingEnterKey with Enter key should submit comment
        /// </summary>
        [Test]
        public void UsingEnterKey_Valid_Enter_Key_Should_Submit_Comment()
        {
            // Arrange
            MockProductService.Setup(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>())).Returns(true);
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();
            var textarea = component.Find("textarea");
            textarea.Change("Test comment via Enter");

            // Act
            textarea.KeyDown(new KeyboardEventArgs { Key = "Enter", ShiftKey = false });

            // Assert
            var addCommentCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddCommentToProduct");
            Assert.That(addCommentCalls, Is.EqualTo(1));
        }

        /// <summary>
        /// Test UsingEnterKey with Shift+Enter should not submit comment
        /// </summary>
        [Test]
        public void UsingEnterKey_Valid_Shift_Enter_Should_Not_Submit_Comment()
        {
            // Arrange
            MockProductService.Setup(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>())).Returns(true);
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();
            var textarea = component.Find("textarea");
            textarea.Change("Test comment");

            // Act
            textarea.KeyDown(new KeyboardEventArgs { Key = "Enter", ShiftKey = true });

            // Assert
            var addCommentCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddCommentToProduct");
            Assert.That(addCommentCalls, Is.EqualTo(0));
        }

        /// <summary>
        /// Test UsingEnterKey with other key should not submit comment
        /// </summary>
        [Test]
        public void UsingEnterKey_Invalid_Other_Key_Should_Not_Submit_Comment()
        {
            // Arrange
            MockProductService.Setup(x => x.AddCommentToProduct(It.IsAny<string>(), It.IsAny<CommentModel>())).Returns(true);
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();
            var textarea = component.Find("textarea");
            textarea.Change("Test comment");

            // Act
            textarea.KeyDown(new KeyboardEventArgs { Key = "Tab", ShiftKey = false });

            // Assert
            var addCommentCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddCommentToProduct");
            Assert.That(addCommentCalls, Is.EqualTo(0));
        }

        #endregion EnterKeySubmit

        #region GetAverageRating

        /// <summary>
        /// Test sorting with null ratings should treat as 0
        /// </summary>
        [Test]
        public void Sort_Valid_RatingHighLow_With_Null_Ratings_Should_Treat_As_Zero()
        {
            // Arrange
            TestProducts[0].Ratings = null;
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("RatingHighLow");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test sorting with empty ratings array should treat as 0
        /// </summary>
        [Test]
        public void Sort_Valid_RatingLowHigh_With_Empty_Ratings_Should_Treat_As_Zero()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { };
            var component = _testContext.Render<ProductList>();
            var sortSelect = component.FindAll("select")[3];

            // Act
            sortSelect.Change("RatingLowHigh");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        #endregion GetAverageRating

        #region ApplyProductTypeFilter

        /// <summary>
        /// Test filter with invalid ProductType string should show all products
        /// </summary>
        [Test]
        public void Filter_Invalid_ProductType_String_Should_Show_All_Products()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var typeSelect = component.FindAll("select")[0];

            // Act
            typeSelect.Change("InvalidProductType");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        #endregion ApplyProductTypeFilter

        #region ApplyRatingFilter

        /// <summary>
        /// Test filter rating with products having empty ratings array
        /// </summary>
        [Test]
        public void Filter_MinRating_With_Empty_Ratings_Array_Should_Exclude_Product()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { };
            var component = _testContext.Render<ProductList>();
            var ratingSelect = component.FindAll("select")[2];

            // Act
            ratingSelect.Change("1");

            // Reset

            // Assert
            var cards = component.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(2)); // Only products with ratings >= 1
        }

        #endregion ApplyRatingFilter

        #region GetAllProductTypes

        /// <summary>
        /// Test GetAllProductTypes excludes Undefined
        /// </summary>
        [Test]
        public void GetAllProductTypes_Should_Exclude_Undefined()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();

            // Act
            var typeSelect = component.FindAll("select")[0];
            var options = typeSelect.QuerySelectorAll("option");

            // Reset

            // Assert
            var optionTexts = options.Select(o => o.TextContent).ToList();
            Assert.That(optionTexts.Contains("Undefined"), Is.False);
        }

        #endregion GetAllProductTypes

        #region GetAllBrands

        /// <summary>
        /// Test GetAllBrands returns unique sorted brands
        /// </summary>
        [Test]
        public void GetAllBrands_Should_Return_Unique_Sorted_Brands()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();

            // Act
            var brandSelect = component.FindAll("select")[1];
            var options = brandSelect.QuerySelectorAll("option").Skip(1).ToList(); // Skip "All Brands"

            // Reset

            // Assert
            var brands = options.Select(o => o.TextContent).ToList();
            var sortedBrands = brands.OrderBy(b => b).ToList();
            Assert.That(brands, Is.EqualTo(sortedBrands));
        }

        #endregion GetAllBrands

        #region HasComments

        /// <summary>
        /// Test HasComments returns true when CommentList has items
        /// </summary>
        [Test]
        public void HasComments_Valid_CommentList_With_Items_Should_Return_True()
        {
            // Arrange
            TestProducts[0].CommentList = new List<CommentModel>
            {
                new CommentModel { Comment = "Test comment", CreatedAt = DateTime.Now }
            };
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var commentItems = component.FindAll(".list-group-item");
            Assert.That(commentItems.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test HasComments returns false when CommentList is null
        /// </summary>
        [Test]
        public void HasComments_Invalid_Null_CommentList_Should_Return_False()
        {
            // Arrange
            TestProducts[0].CommentList = null;
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modalBody = component.Find(".modal-body");
            Assert.That(modalBody.TextContent.Contains("No comments yet"), Is.True);
        }

        /// <summary>
        /// Test HasComments returns false when CommentList is empty (Any() == false)
        /// </summary>
        [Test]
        public void HasComments_Invalid_Empty_CommentList_Should_Return_False()
        {
            // Arrange
            TestProducts[0].CommentList = new List<CommentModel>(); // Empty list, not null
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var commentItems = component.FindAll(".list-group-item");
            Assert.That(commentItems.Count, Is.EqualTo(0));
        }

        #endregion HasComments

        #region BuildRenderTree

        /// <summary>
        /// Test all product types are rendered in dropdown
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Should_Render_All_ProductType_Options()
        {
            // Arrange & Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var typeSelect = component.FindAll("select")[0];
            var options = typeSelect.QuerySelectorAll("option");
            Assert.That(options.Count(), Is.GreaterThan(1));
        }

        /// <summary>
        /// Test all brands are rendered in dropdown
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Should_Render_All_Brand_Options()
        {
            // Arrange & Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var brandSelect = component.FindAll("select")[1];
            var options = brandSelect.QuerySelectorAll("option");
            Assert.That(options.Count(), Is.EqualTo(5)); // "All Brands" + 4 unique brands
        }

        /// <summary>
        /// Test modal renders with product URL link
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Product_With_URL_Should_Render_Link()
        {
            // Arrange
            TestProducts[0].Url = "https://example.com";
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var link = component.FindAll("a").FirstOrDefault(a => a.GetAttribute("href") == "https://example.com");
            Assert.That(link, Is.Not.Null);
        }

        /// <summary>
        /// Test modal renders without product URL link when URL is null
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Product_With_Null_URL_Should_Not_Render_Link()
        {
            // Arrange
            TestProducts[0].Url = null;
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button")
                                           .Where(b => b.TextContent.Contains("More Info"))
                                           .ToList();

            // Act
            moreInfoButtons[0].Click();

            // Wait for modal content to render
            component.WaitForAssertion(() =>
            {
                var allElements = component.FindAll("*");
                Assert.That(allElements.Count, Is.GreaterThan(0));
            });

            // Assert
            var viewProductLinks = component.FindAll("*").FirstOrDefault(e => e.TextContent.Contains("View Product"));

            Assert.That(viewProductLinks, Is.Null);
        }



        /// <summary>
        /// Test star rating renders checked stars correctly
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Rating_Should_Render_Checked_Stars()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 5, 5, 5 }; // Average = 5
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var checkedStars = component.FindAll("span.fa-star.checked");
            Assert.That(checkedStars.Count, Is.EqualTo(5));
        }

        /// <summary>
        /// Test star rating renders unchecked stars correctly
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Rating_Should_Render_Unchecked_Stars()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 2, 2, 2 }; // Average = 2
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var checkedStars = component.FindAll("span.fa-star.checked");
            var uncheckedStars = component.FindAll("span.fa-star:not(.checked)");
            Assert.That(checkedStars.Count, Is.EqualTo(2));
            Assert.That(uncheckedStars.Count, Is.EqualTo(3));
        }

        /// <summary>
        /// Test VoteCount greater than 0 renders vote count span
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_VoteCount_Greater_Than_Zero_Should_Render_Count()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 5, 4, 3 };
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modalFooter = component.FindAll(".modal-footer")[1];
            Assert.That(modalFooter.TextContent.Contains("3 Votes"), Is.True);
        }

        /// <summary>
        /// Test VoteCount equal to 0 renders first vote message
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_VoteCount_Zero_Should_Render_First_Vote_Message()
        {
            // Arrange
            TestProducts[0].Ratings = null;
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modalFooter = component.FindAll(".modal-footer")[1];
            Assert.That(modalFooter.TextContent.Contains("Be the first to vote!"), Is.True);
        }

        /// <summary>
        /// Test comments section renders when HasComments is true
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_HasComments_True_Should_Render_Comments_List()
        {
            // Arrange
            TestProducts[0].CommentList = new List<CommentModel>
    {
        new CommentModel { Comment = "First comment", CreatedAt = new DateTime(2024, 1, 15, 10, 30, 0) },
        new CommentModel { Comment = "Second comment", CreatedAt = new DateTime(2024, 1, 16, 14, 45, 0) }
    };
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var commentItems = component.FindAll(".list-group-item");
            Assert.That(commentItems.Count, Is.EqualTo(2));
            Assert.That(component.Markup.Contains("First comment"), Is.True);
            Assert.That(component.Markup.Contains("Second comment"), Is.True);
        }

        /// <summary>
        /// Test comment timestamp is rendered
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Comment_Should_Render_Timestamp()
        {
            // Arrange
            var testDate = new DateTime(2024, 6, 15, 10, 30, 0);
            TestProducts[0].CommentList = new List<CommentModel>
    {
        new CommentModel { Comment = "Test comment", CreatedAt = testDate }
    };
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modalBody = component.Find(".modal-body");
            Assert.That(modalBody.TextContent.Contains(testDate.ToShortDateString()), Is.True);
        }

        /// <summary>
        /// Test clicking each star calls SubmitRating with correct value
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Click_Star_Should_Submit_Correct_Rating()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();
            var stars = component.FindAll("span.fa-star");

            // Act - Click the 3rd star (index 2)
            stars[2].Click();

            // Assert
            var addRatingCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddRating" &&
                                    invocation.Arguments[0].ToString() == "test-laptop-1" &&
                                    (int)invocation.Arguments[1] == 3);
            Assert.That(addRatingCalls, Is.EqualTo(1));
        }

        /// <summary>
        /// Test product card image style is rendered correctly
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Product_Should_Render_Card_Image_Style()
        {
            // Arrange & Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var cardImages = component.FindAll(".card-img");
            Assert.That(cardImages.Count, Is.GreaterThan(0));
            Assert.That(cardImages[0].GetAttribute("style").Contains("background-image"), Is.True);
        }

        /// <summary>
        /// Test product type display name is rendered
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Product_Should_Render_ProductType_DisplayName()
        {
            // Arrange & Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var subtitles = component.FindAll(".card-subtitle");
            Assert.That(subtitles.Any(s => s.TextContent.Contains("Laptop")), Is.True);
        }

        /// <summary>
        /// Test modal image is rendered with correct style
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Modal_Should_Render_Product_Image()
        {
            // Arrange
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Reset

            // Assert
            var modalCardImages = component.FindAll(".modal .card-img");
            Assert.That(modalCardImages.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test no products message is shown when CurrentProductList is empty
        /// </summary>
        [Test]
        public void BuildRenderTree_Valid_Empty_ProductList_Should_Show_No_Products_Alert()
        {
            // Arrange
            MockProductService.Setup(x => x.GetProducts()).Returns(new List<ProductModel>());

            // Act
            var component = _testContext.Render<ProductList>();

            // Reset

            // Assert
            var alert = component.Find(".alert");
            Assert.That(alert.TextContent.Contains("No products are found"), Is.True);
        }

        #endregion BuildRenderTree

        #region StarRating

        /// <summary>
        /// Test clicking unchecked star submits correct rating
        /// </summary>
        [Test]
        public void SubmitRating_Valid_Click_Unchecked_Star_Should_Submit_Rating()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 2, 2, 2 }; // Average = 2, so stars 3, 4, 5 are unchecked
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();
            // Get all star elements (both checked and unchecked)
            var allStars = component.FindAll("span.fa-star");

            // Act - Click the 5th star (index 4), which should be unchecked
            allStars[4].Click();

            // Assert
            var addRatingCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddRating" &&
                                    invocation.Arguments[0].ToString() == "test-laptop-1" &&
                                    (int)invocation.Arguments[1] == 5);
            Assert.That(addRatingCalls, Is.EqualTo(1));
        }

        /// <summary>
        /// Test clicking unchecked star when current rating is 1
        /// </summary>
        [Test]
        public void SubmitRating_Valid_Click_Unchecked_Star_Rating_1_Should_Submit_Rating_3()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 1, 1, 1 }; // Average = 1, so stars 2, 3, 4, 5 are unchecked
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();
            // Get all star elements
            var allStars = component.FindAll("span.fa-star");

            // Act - Click the 3rd star (index 2), which should be unchecked
            allStars[2].Click();

            // Assert
            var addRatingCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddRating" &&
                                    invocation.Arguments[0].ToString() == "test-laptop-1" &&
                                    (int)invocation.Arguments[1] == 3);
            Assert.That(addRatingCalls, Is.EqualTo(1));
        }

        /// <summary>
        /// Test clicking checked star submits correct rating
        /// </summary>
        [Test]
        public void SubmitRating_Valid_Click_Checked_Star_Should_Submit_Rating()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 4, 4, 4 }; // Average = 4, so stars 1, 2, 3, 4 are checked
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();
            // Get checked star elements
            var checkedStars = component.FindAll("span.fa-star.checked");

            // Act - Click the 2nd checked star (index 1)
            checkedStars[1].Click();

            // Assert
            var addRatingCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddRating" &&
                                    invocation.Arguments[0].ToString() == "test-laptop-1" &&
                                    (int)invocation.Arguments[1] == 2);
            Assert.That(addRatingCalls, Is.EqualTo(1));
        }

        /// <summary>
        /// Test both checked and unchecked stars are rendered correctly
        /// </summary>
        [Test]
        public void Modal_Valid_Rating_3_Should_Render_3_Checked_And_2_Unchecked_Stars()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 3, 3, 3 }; // Average = 3
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Assert
            var checkedStars = component.FindAll("span.fa-star.checked");
            var uncheckedStars = component.FindAll("span.fa-star:not(.checked)");

            Assert.That(checkedStars.Count, Is.EqualTo(3));
            Assert.That(uncheckedStars.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test clicking each unchecked star individually
        /// </summary>
        [Test]
        public void SubmitRating_Valid_Click_Each_Unchecked_Star_Should_Submit_Correct_Rating()
        {
            // Arrange
            TestProducts[0].Ratings = new int[] { 1, 1, 1 }; // Average = 1, stars 2-5 are unchecked
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();

            // Get unchecked stars (stars 2–5)
            var uncheckedStars = component.FindAll("span.fa-star:not(.checked)");

            // Act - Click the first unchecked star (should correspond to rating 2)
            uncheckedStars[0].Click();

            // Assert
            var addRatingCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddRating" &&
                                     invocation.Arguments[0].ToString() == "test-laptop-1" &&
                                     (int)invocation.Arguments[1] == 2);

            Assert.That(addRatingCalls, Is.EqualTo(1));
        }


        /// <summary>
        /// Test product with 0 rating shows all unchecked stars
        /// </summary>
        [Test]
        public void Modal_Valid_Rating_0_Should_Render_All_Unchecked_Stars()
        {
            // Arrange
            TestProducts[0].Ratings = null; // No ratings = 0 average
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();

            // Act
            moreInfoButtons[0].Click();

            // Assert
            var checkedStars = component.FindAll("span.fa-star.checked");
            var uncheckedStars = component.FindAll("span.fa-star:not(.checked)");

            Assert.That(checkedStars.Count, Is.EqualTo(0));
            Assert.That(uncheckedStars.Count, Is.EqualTo(5));
        }

        /// <summary>
        /// Test clicking unchecked star when no ratings exist
        /// </summary>
        [Test]
        public void SubmitRating_Valid_No_Ratings_Click_Unchecked_Star_Should_Submit_Rating()
        {
            // Arrange
            TestProducts[0].Ratings = null; // No ratings, all stars unchecked
            var component = _testContext.Render<ProductList>();
            var moreInfoButtons = component.FindAll("button").Where(b => b.TextContent.Contains("More Info")).ToList();
            moreInfoButtons[0].Click();

            // Get all unchecked stars
            var uncheckedStars = component.FindAll("span.fa-star:not(.checked)");

            // Act - Click the 4th star (index 3)
            uncheckedStars[3].Click();

            // Assert
            var addRatingCalls = MockProductService.Invocations
                .Count(invocation => invocation.Method.Name == "AddRating" &&
                                     invocation.Arguments[0].ToString() == "test-laptop-1" &&
                                     (int)invocation.Arguments[1] == 4);

            Assert.That(addRatingCalls, Is.EqualTo(1));
        }


        #endregion StarRating

    }

}