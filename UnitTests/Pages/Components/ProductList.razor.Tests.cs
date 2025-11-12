using System.Diagnostics;
using Bunit;

using Microsoft.Extensions.Logging;

using NUnit.Framework;

using Moq;

using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace UnitTests.Pages.Components
{
    /// <summary>
    /// Unit tests for ProductList Razor component
    /// Tests search, filter, sort, modal, and rating functionality
    /// </summary>
    public class ProductListTests : BunitContext
    {
        #region TestSetup
        private Mock<JsonFileProductService> mockProductService;
        private List<ProductModel> testProducts;

        [SetUp]
        public void TestInitialize()
        {
            // Create test data
            testProducts = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = "1",
                    Brand = "Alpha Brand",
                    ProductType = ProductTypeEnum.Mice,
                    ProductDescription = "Test shampoo",
                    Image = "test1.jpg",
                    Url = "http://test1.com",
                    Ratings = new int[] { 5, 4, 5 }
                },
                new ProductModel
                {
                    Id = "2",
                    Brand = "Beta Brand",
                    ProductType = ProductTypeEnum.Keyboard,
                    ProductDescription = "Test conditioner",
                    Image = "test2.jpg",
                    Url = "http://test2.com",
                    Ratings = new int[] { 3, 3, 4 }
                },
                new ProductModel
                {
                    Id = "3",
                    Brand = "Gamma Brand",
                    ProductType = ProductTypeEnum.Laptop,
                    ProductDescription = "Another shampoo",
                    Image = "test3.jpg",
                    Url = "http://test3.com",
                    Ratings = null
                },
                new ProductModel
                {
                    Id = "4",
                    Brand = "Delta Brand",
                    ProductType = ProductTypeEnum.Laptop,
                    ProductDescription = "Hair oil product",
                    Image = "test4.jpg",
                    Url = "",
                    Ratings = new int[] { 2, 2, 1 }
                }
            };

            // Setup mock service
            mockProductService = new Mock<JsonFileProductService>(MockBehavior.Strict, (IWebHostEnvironment)null);
            mockProductService.Setup(s => s.GetProducts()).Returns(testProducts);

            // Register mock service
            Services.AddSingleton(mockProductService.Object);
        }
        #endregion TestSetup

        #region ComponentRendering
        /// <summary>
        /// Test that component renders without errors
        /// </summary>
        [Test]
        public void ProductList_Component_Should_Render_Successfully()
        {
            // Arrange & Act
            var cut = Render<ProductList>();

            // Assert
            Assert.That(cut, Is.Not.Null);
            Assert.That(cut.Markup, Does.Contain("<div"));
        }

        [Test]
        public void ProductList_Default_Should_Return_Content()
        {
            // Arrange
            
            // Act
            var page = Render<ProductList>();

            // Get the Cards returned
            var result = page.Markup;

            Console.WriteLine(result);

            // Assert
            Assert.That(result.Contains("Dell 16 Plus"));
        }

        /// <summary>
        /// Test that all products are displayed initially
        /// </summary>
        [Test]
        public void ProductList_Should_Display_All_Products_Initially()
        {
            // Arrange & Act
            var cut = Render<ProductList>();

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test that product brands are displayed correctly
        /// </summary>
        [Test]
        public void ProductList_Should_Display_Product_Brands()
        {
            // Arrange & Act
            var cut = Render<ProductList>();

            // Assert
            var cardTitles = cut.FindAll(".card-title");
            Assert.That(cardTitles.Any(ct => ct.TextContent.Contains("Alpha Brand")));
            Assert.That(cardTitles.Any(ct => ct.TextContent.Contains("Beta Brand")));
        }
        #endregion ComponentRendering

        #region SearchFunctionality
        /// <summary>
        /// Test search functionality filters products by brand
        /// </summary>
        [Test]
        public void Search_Valid_Brand_Should_Filter_Products()
        {
            // Arrange
            var cut = Render<ProductList>();
            var searchInput = cut.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Change("Alpha");
            cut.WaitForState(() => cut.FindAll(".card").Count == 1);

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1));
            Assert.That(cut.Markup.Contains("Alpha Brand"));
        }

        /// <summary>
        /// Test search is case-insensitive
        /// </summary>
        [Test]
        public void Search_Case_Insensitive_Should_Filter_Products()
        {
            // Arrange
            var cut = Render<ProductList>();
            var searchInput = cut.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Change("beta");
            cut.WaitForState(() => cut.FindAll(".card").Count == 1);

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1));
            Assert.That(cut.Markup.Contains("Beta Brand"));
        }

        /// <summary>
        /// Test search with no matches shows alert message
        /// </summary>
        [Test]
        public void Search_No_Matches_Should_Show_Alert_Message()
        {
            // Arrange
            var cut = Render<ProductList>();
            var searchInput = cut.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Change("NonExistentBrand");
            cut.WaitForState(() => cut.FindAll(".alert-info").Count == 1);

            // Assert
            var alert = cut.Find(".alert-info");
            Assert.That(alert.TextContent.Contains("No products are found"));
        }

        /// <summary>
        /// Test empty search shows all products
        /// </summary>
        [Test]
        public void Search_Empty_String_Should_Show_All_Products()
        {
            // Arrange
            var cut = Render<ProductList>();
            var searchInput = cut.Find("input[placeholder='Search Brands...']");

            // Act
            searchInput.Change("");

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }
        #endregion SearchFunctionality

        #region ProductTypeFilter
        /// <summary>
        /// Test product type filter shows only shampoos
        /// </summary>
        [Test]
        public void Filter_ProductType_Shampoo_Should_Show_Only_Shampoos()
        {
            // Arrange
            var cut = Render<ProductList>();
            var productTypeSelect = cut.FindAll("select").First();

            // Act
            productTypeSelect.Change(ProductTypeEnum.Mice.ToString());
            cut.WaitForState(() => cut.FindAll(".card").Count == 2);

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Test product type filter dropdown is populated dynamically
        /// </summary>
        [Test]
        public void Filter_ProductType_Dropdown_Should_Be_Populated()
        {
            // Arrange & Act
            var cut = Render<ProductList>();
            var productTypeSelect = cut.FindAll("select").First();
            var options = productTypeSelect.QuerySelectorAll("option");

            // Assert
            Assert.That(options.Count(), Is.GreaterThan(1)); // "All Types" + actual types
            Assert.That(options.Any(o => o.TextContent == "All Types"));
        }

        /// <summary>
        /// Test selecting "All Types" shows all products
        /// </summary>
        [Test]
        public void Filter_ProductType_All_Types_Should_Show_All_Products()
        {
            // Arrange
            var cut = Render<ProductList>();
            var productTypeSelect = cut.FindAll("select").First();

            // Act - First filter, then reset
            productTypeSelect.Change(ProductTypeEnum.Mice.ToString());
            productTypeSelect.Change("");

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }
        #endregion ProductTypeFilter

        #region BrandFilter
        /// <summary>
        /// Test brand filter shows only selected brand
        /// </summary>
        [Test]
        public void Filter_Brand_Should_Show_Only_Selected_Brand()
        {
            // Arrange
            var cut = Render<ProductList>();
            var brandSelect = cut.FindAll("select")[1]; // Second select is brand

            // Act
            brandSelect.Change("Alpha Brand");
            cut.WaitForState(() => cut.FindAll(".card").Count == 1);

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1));
            Assert.That(cut.Markup.Contains("Alpha Brand"));
        }

        /// <summary>
        /// Test brand dropdown is populated with unique brands
        /// </summary>
        [Test]
        public void Filter_Brand_Dropdown_Should_Be_Populated_With_Unique_Brands()
        {
            // Arrange & Act
            var cut = Render<ProductList>();
            var brandSelect = cut.FindAll("select")[1];
            var options = brandSelect.QuerySelectorAll("option");

            // Assert
            Assert.That(options.Count(), Is.EqualTo(5)); // "All Brands" + 4 unique brands
            Assert.That(options.Any(o => o.TextContent == "All Brands"));
        }

        /// <summary>
        /// Test brands are sorted alphabetically
        /// </summary>
        [Test]
        public void Filter_Brand_Dropdown_Should_Be_Sorted_Alphabetically()
        {
            // Arrange & Act
            var cut = Render<ProductList>();
            var brandSelect = cut.FindAll("select")[1];
            var options = brandSelect.QuerySelectorAll("option").Skip(1).ToList(); // Skip "All Brands"

            // Assert
            var brands = options.Select(o => o.TextContent).ToList();
            var sortedBrands = brands.OrderBy(b => b).ToList();
            Assert.That(brands, Is.EqualTo(sortedBrands));
        }
        #endregion BrandFilter

        #region RatingFilter
        /// <summary>
        /// Test rating filter shows only products with minimum rating
        /// </summary>
        [Test]
        public void Filter_MinRating_Should_Show_Only_Products_Meeting_Criteria()
        {
            // Arrange
            var cut = Render<ProductList>();
            var ratingSelect = cut.FindAll("select")[2]; // Third select is min rating

            // Act
            ratingSelect.Change("4");
            cut.WaitForState(() => cut.FindAll(".card").Count == 1);

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1)); // Only Alpha Brand has avg rating >= 4
        }

        /// <summary>
        /// Test rating filter excludes products with no ratings
        /// </summary>
        [Test]
        public void Filter_MinRating_Should_Exclude_Products_With_No_Ratings()
        {
            // Arrange
            var cut = Render<ProductList>();
            var ratingSelect = cut.FindAll("select")[2];

            // Act
            ratingSelect.Change("1");

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(3)); // Gamma Brand has null ratings, should be excluded
            Assert.That(cut.Markup.Contains("Gamma Brand"), Is.False);
        }

        /// <summary>
        /// Test "All Ratings" shows all products
        /// </summary>
        [Test]
        public void Filter_MinRating_All_Should_Show_All_Products()
        {
            // Arrange
            var cut = Render<ProductList>();
            var ratingSelect = cut.FindAll("select")[2];

            // Act
            ratingSelect.Change("0");

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4));
        }
        #endregion RatingFilter

        #region SortingFunctionality
        /// <summary>
        /// Test sorting by brand A-Z
        /// </summary>
        [Test]
        public void Sort_BrandAZ_Should_Order_Products_Alphabetically()
        {
            // Arrange
            var cut = Render<ProductList>();
            var sortSelect = cut.FindAll("select")[3]; // Fourth select is sort

            // Act
            sortSelect.Change("BrandAZ");

            // Assert
            var cardTitles = cut.FindAll(".card-title");
            var brands = cardTitles.Select(ct => ct.TextContent.Trim()).ToList();
            
            Assert.That(brands[0], Is.EqualTo("Alpha Brand"));
            Assert.That(brands[3], Is.EqualTo("Delta Brand"));
        }

        /// <summary>
        /// Test sorting by brand Z-A
        /// </summary>
        [Test]
        public void Sort_BrandZA_Should_Order_Products_Reverse_Alphabetically()
        {
            // Arrange
            var cut = Render<ProductList>();
            var sortSelect = cut.FindAll("select")[3];

            // Act
            sortSelect.Change("BrandZA");

            // Assert
            var cardTitles = cut.FindAll(".card-title");
            var brands = cardTitles.Select(ct => ct.TextContent.Trim()).ToList();
            
            Assert.That(brands[0], Is.EqualTo("Gamma Brand"));
            Assert.That(brands[3], Is.EqualTo("Alpha Brand"));
        }

        /// <summary>
        /// Test sorting by rating high to low
        /// </summary>
        [Test]
        public void Sort_RatingHighLow_Should_Order_Products_By_Rating_Descending()
        {
            // Arrange
            var cut = Render<ProductList>();
            var sortSelect = cut.FindAll("select")[3];

            // Act
            sortSelect.Change("RatingHighLow");

            // Assert
            var cardTitles = cut.FindAll(".card-title");
            var brands = cardTitles.Select(ct => ct.TextContent.Trim()).ToList();
            
            // Alpha Brand (avg 4.67) should be first
            Assert.That(brands[0], Is.EqualTo("Alpha Brand"));
        }

        /// <summary>
        /// Test sorting by rating low to high
        /// </summary>
        [Test]
        public void Sort_RatingLowHigh_Should_Order_Products_By_Rating_Ascending()
        {
            // Arrange
            var cut = Render<ProductList>();
            var sortSelect = cut.FindAll("select")[3];

            // Act
            sortSelect.Change("RatingLowHigh");

            // Assert
            var cardTitles = cut.FindAll(".card-title");
            var brands = cardTitles.Select(ct => ct.TextContent.Trim()).ToList();
            
            // Products with no ratings (0) or Delta Brand (avg 1.67) should be first
            Assert.That(brands[0] == "Gamma Brand" || brands[0] == "Delta Brand");
        }

        /// <summary>
        /// Test default sorting maintains original order
        /// </summary>
        [Test]
        public void Sort_Default_Should_Maintain_Original_Order()
        {
            // Arrange
            var cut = Render<ProductList>();
            var sortSelect = cut.FindAll("select")[3];

            // Act
            sortSelect.Change("");

            // Assert
            var cardTitles = cut.FindAll(".card-title");
            var brands = cardTitles.Select(ct => ct.TextContent.Trim()).ToList();
            
            Assert.That(brands[0], Is.EqualTo("Alpha Brand"));
            Assert.That(brands[1], Is.EqualTo("Beta Brand"));
        }
        #endregion SortingFunctionality

        #region ClearFilters
        /// <summary>
        /// Test clear filters button resets all filters
        /// </summary>
        [Test]
        public void ClearFilters_Should_Reset_All_Filter_Values()
        {
            // Arrange
            var cut = Render<ProductList>();
            var searchInput = cut.Find("input[placeholder='Search Brands...']");
            var productTypeSelect = cut.FindAll("select")[0];
            var brandSelect = cut.FindAll("select")[1];

            // Set some filters
            searchInput.Change("Alpha");
            productTypeSelect.Change(ProductTypeEnum.Mice.ToString());
            brandSelect.Change("Alpha Brand");

            // Act
            cut.InvokeAsync(() => cut.Find("button.btn-secondary").Click());

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(4)); // All products should be visible
        }
        #endregion ClearFilters

        #region ModalFunctionality
        /// <summary>
        /// Test clicking More Info button selects product
        /// </summary>
        [Test]
        public void MoreInfo_Button_Click_Should_Open_Modal_With_Product_Details()
        {
            // Arrange
            var cut = Render<ProductList>();
            var moreInfoButton = cut.Find(".btn-primary");

            // Act
            moreInfoButton.Click();

            // Assert
            var modal = cut.Find("#productModal");
            Assert.That(modal, Is.Not.Null);
            Assert.That(cut.Markup.Contains("Alpha Brand"));
        }

        /// <summary>
        /// Test modal displays product description
        /// </summary>
        [Test]
        public void Modal_Should_Display_Product_Description()
        {
            // Arrange
            var cut = Render<ProductList>();
            var moreInfoButton = cut.Find(".btn-primary");

            // Act
            moreInfoButton.Click();

            // Assert
            Assert.That(cut.Markup.Contains("Test shampoo"));
        }

        /// <summary>
        /// Test modal displays View Product link when URL exists
        /// </summary>
        [Test]
        public void Modal_Should_Display_View_Product_Link_When_URL_Exists()
        {
            // Arrange
            var cut = Render<ProductList>();
            var moreInfoButton = cut.Find(".btn-primary");

            // Act
            moreInfoButton.Click();

            // Assert
            var viewProductLink = cut.Find("a[href='http://test1.com']");
            Assert.That(viewProductLink, Is.Not.Null);
            Assert.That(viewProductLink.TextContent, Is.EqualTo("View Product"));
        }

        /// <summary>
        /// Test modal hides View Product link when URL is empty
        /// </summary>
        [Test]
        public void Modal_Should_Hide_View_Product_Link_When_URL_Empty()
        {
            // Arrange
            var cut = Render<ProductList>();
            var cards = cut.FindAll(".btn-primary");
            
            // Act - Click the 4th product (Delta Brand with empty URL)
            cards[3].Click();

            // Assert
            var viewProductLinks = cut.FindAll("a.btn-primary");
            Assert.That(viewProductLinks.Count, Is.EqualTo(0));
        }
        #endregion ModalFunctionality

        #region RatingFunctionality
        /// <summary>
        /// Test product with ratings displays vote count
        /// </summary>
        [Test]
        public void Modal_Should_Display_Vote_Count_For_Product_With_Ratings()
        {
            // Arrange
            var cut = Render<ProductList>();
            var moreInfoButton = cut.Find(".btn-primary");

            // Act
            moreInfoButton.Click();

            // Assert
            Assert.That(cut.Markup.Contains("3 Votes"));
        }

        /// <summary>
        /// Test product with no ratings displays appropriate message
        /// </summary>
        [Test]
        public void Modal_Should_Display_First_Vote_Message_When_No_Ratings()
        {
            // Arrange
            var cut = Render<ProductList>();
            var cards = cut.FindAll(".btn-primary");
            
            // Act - Click product with null ratings (Gamma Brand)
            cards[2].Click();

            // Assert
            Assert.That(cut.Markup.Contains("Be the first to vote!"));
        }

        /// <summary>
        /// Test correct star display based on current rating
        /// </summary>
        [Test]
        public void Modal_Should_Display_Correct_Number_Of_Checked_Stars()
        {
            // Arrange
            var cut = Render<ProductList>();
            var moreInfoButton = cut.Find(".btn-primary");

            // Act
            moreInfoButton.Click();

            // Assert
            var checkedStars = cut.FindAll(".fa-star.checked");
            // Alpha Brand has ratings [5, 4, 5], avg = 4.67, should show 4 checked stars
            Assert.That(checkedStars.Count, Is.EqualTo(4));
        }

        /// <summary>
        /// Test submitting rating calls service method
        /// </summary>
        [Test]
        public void SubmitRating_Should_Call_ProductService_AddRating()
        {
            // Arrange
            mockProductService.Setup(s => s.AddRating(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(true);

            var cut = Render<ProductList>();
            var moreInfoButton = cut.Find(".btn-primary");
            moreInfoButton.Click();

            // Act
            var stars = cut.FindAll(".fa-star");
            stars[4].Click(); // Click 5th star

            // Assert
            mockProductService.Verify(s => s.AddRating("1", 5), Times.Once);
        }

        /// <summary>
        /// Test vote label is singular for single vote
        /// </summary>
        [Test]
        public void Modal_Should_Display_Singular_Vote_Label_For_One_Vote()
        {
            // Arrange
            testProducts[0].Ratings = new int[] { 5 }; // Single rating
            var cut = Render<ProductList>();
            var moreInfoButton = cut.Find(".btn-primary");

            // Act
            moreInfoButton.Click();

            // Assert
            Assert.That(cut.Markup.Contains("1 Vote"));
        }
        #endregion RatingFunctionality

        #region CombinedFilters
        /// <summary>
        /// Test multiple filters work together
        /// </summary>
        [Test]
        public void Multiple_Filters_Should_Work_Together()
        {
            // Arrange
            var cut = Render<ProductList>();
            var productTypeSelect = cut.FindAll("select")[0];
            var ratingSelect = cut.FindAll("select")[2];

            // Act
            productTypeSelect.Change(ProductTypeEnum.Mice.ToString());
            ratingSelect.Change("4");

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(1)); // Only Alpha Brand is Shampoo with rating >= 4
            Assert.That(cut.Markup.Contains("Alpha Brand"));
        }

        /// <summary>
        /// Test search with other filters
        /// </summary>
        [Test]
        public void Search_With_Filter_Should_Apply_Both_Criteria()
        {
            // Arrange
            var cut = Render<ProductList>();
            var searchInput = cut.Find("input[placeholder='Search Brands...']");
            var productTypeSelect = cut.FindAll("select")[0];

            // Act
            searchInput.Change("Brand");
            productTypeSelect.Change(ProductTypeEnum.Mice.ToString());

            // Assert
            var cards = cut.FindAll(".card");
            Assert.That(cards.Count, Is.EqualTo(2)); // Alpha and Gamma are shampoos
        }
        #endregion CombinedFilters

        #region EdgeCases
        /// <summary>
        /// Test component handles empty product list
        /// </summary>
        [Test]
        public void Component_Should_Handle_Empty_Product_List()
        {
            // Arrange
            mockProductService.Setup(s => s.GetProducts()).Returns(new List<ProductModel>());
            
            // Act
            var cut = Render<ProductList>();

            // Assert
            var alert = cut.Find(".alert-info");
            Assert.That(alert.TextContent.Contains("No products are found"));
        }

        /// <summary>
        /// Test component handles products with empty ratings array
        /// </summary>
        [Test]
        public void Component_Should_Handle_Products_With_Empty_Ratings_Array()
        {
            // Arrange
            testProducts[0].Ratings = new int[] { };
            var cut = Render<ProductList>();
            var moreInfoButton = cut.Find(".btn-primary");

            // Act
            moreInfoButton.Click();

            // Assert
            Assert.That(cut.Markup.Contains("Be the first to vote!"));
        }
        #endregion EdgeCases
    }
}