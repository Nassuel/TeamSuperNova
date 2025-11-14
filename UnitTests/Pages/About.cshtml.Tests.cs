using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for AboutModel class
    /// Tests all methods and properties to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class AboutModelTests
    {
        #region TestSetup

        public static AboutModel pageModel;

        /// <summary>
        /// Initialize test environment before each test
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            pageModel = new AboutModel();
        }

        #endregion TestSetup

        #region Constructor

        /// <summary>
        /// Test constructor creates valid instance
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Create_Valid_Instance()
        {
            // Arrange - Done in TestInitialize

            // Act - PageModel created in TestInitialize

            // Assert
            Assert.That(pageModel, Is.Not.Null);
        }

        #endregion Constructor

        #region OnGet

        /// <summary>
        /// Test OnGet executes without error
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Execute_Without_Error()
        {
            // Arrange - Done in TestInitialize

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.True);
        }

        /// <summary>
        /// Test OnGet can be called multiple times
        /// </summary>
        [Test]
        public void OnGet_Valid_Multiple_Calls_Should_Execute_Without_Error()
        {
            // Arrange - Done in TestInitialize

            // Act
            pageModel.OnGet();
            pageModel.OnGet();
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid,Is.True);
        }

        #endregion OnGet
    }
}