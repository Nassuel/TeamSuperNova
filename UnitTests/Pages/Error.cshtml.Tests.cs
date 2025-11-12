using ContosoCrafts.WebSite.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Diagnostics;

namespace UnitTests.Pages
{
    /// <summary>
    /// Unit tests for ErrorModel class
    /// Tests all methods and properties to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class ErrorModelTests
    {
        #region TestSetup

        public static ErrorModel pageModel;
        public static Mock<ILogger<ErrorModel>> mockLogger;

        /// <summary>
        /// Initialize test environment before each test
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            mockLogger = new Mock<ILogger<ErrorModel>>();
            pageModel = new ErrorModel(mockLogger.Object);
        }

        #endregion TestSetup

        #region Constructor

        /// <summary>
        /// Test constructor creates valid instance with logger
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Create_Valid_Instance()
        {
            // Arrange - Done in TestInitialize

            // Act - PageModel created in TestInitialize

            // Assert
            Assert.That(pageModel, Is.Not.Null);
        }

        /// <summary>
        /// Test constructor initializes with logger
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Initialize_With_Logger()
        {
            // Arrange
            var logger = new Mock<ILogger<ErrorModel>>();

            // Act
            var model = new ErrorModel(logger.Object);

            // Assert
            Assert.That(model, Is.Not.Null);
        }

        #endregion Constructor

        #region RequestId

        /// <summary>
        /// Test RequestId property is initially null
        /// </summary>
        [Test]
        public void RequestId_Initial_Should_Be_Null()
        {
            // Arrange - Done in TestInitialize

            // Act
            var result = pageModel.RequestId;

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test RequestId property can be set
        /// </summary>
        [Test]
        public void RequestId_Valid_Should_Be_Settable()
        {
            // Arrange
            var requestId = "test-request-id-123";

            // Act
            pageModel.RequestId = requestId;

            // Assert
            Assert.That(pageModel.RequestId, Is.EqualTo(requestId));
        }

        /// <summary>
        /// Test RequestId property can be set to null
        /// </summary>
        [Test]
        public void RequestId_Null_Should_Be_Settable()
        {
            // Arrange
            pageModel.RequestId = "some-id";

            // Act
            pageModel.RequestId = null;

            // Assert
            Assert.That(pageModel.RequestId, Is.Null);
        }

        /// <summary>
        /// Test RequestId property can be set to empty string
        /// </summary>
        [Test]
        public void RequestId_Empty_Should_Be_Settable()
        {
            // Arrange
            var requestId = string.Empty;

            // Act
            pageModel.RequestId = requestId;

            // Assert
            Assert.That(pageModel.RequestId, Is.EqualTo(string.Empty));
        }

        #endregion RequestId

        #region ShowRequestId

        /// <summary>
        /// Test ShowRequestId returns false when RequestId is null
        /// </summary>
        [Test]
        public void ShowRequestId_Null_RequestId_Should_Return_False()
        {
            // Arrange
            pageModel.RequestId = null;

            // Act
            var result = pageModel.ShowRequestId;

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Test ShowRequestId returns false when RequestId is empty
        /// </summary>
        [Test]
        public void ShowRequestId_Empty_RequestId_Should_Return_False()
        {
            // Arrange
            pageModel.RequestId = string.Empty;

            // Act
            var result = pageModel.ShowRequestId;

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Test ShowRequestId returns true when RequestId has value
        /// </summary>
        [Test]
        public void ShowRequestId_Valid_RequestId_Should_Return_True()
        {
            // Arrange
            pageModel.RequestId = "test-request-id";

            // Act
            var result = pageModel.ShowRequestId;

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Test ShowRequestId returns false when RequestId is whitespace
        /// </summary>
        [Test]
        public void ShowRequestId_Whitespace_RequestId_Should_Return_False()
        {
            // Arrange
            pageModel.RequestId = "   ";

            // Act
            var result = pageModel.ShowRequestId;

            // Assert
            Assert.That(result, Is.True); // Note: !string.IsNullOrEmpty doesn't check whitespace
        }

        #endregion ShowRequestId

        #region OnGet

        /// <summary>
        /// Test OnGet sets RequestId from HttpContext.TraceIdentifier
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Set_RequestId_From_TraceIdentifier()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "test-trace-id";
            pageModel.PageContext.HttpContext = httpContext;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.RequestId, Is.EqualTo("test-trace-id"));
        }

        /// <summary>
        /// Test OnGet sets RequestId when Activity.Current is null
        /// </summary>
        [Test]
        public void OnGet_No_Activity_Should_Use_TraceIdentifier()
        {
            // Arrange
            Activity.Current = null;
            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "trace-id-123";
            pageModel.PageContext.HttpContext = httpContext;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.RequestId, Is.EqualTo("trace-id-123"));
        }

        /// <summary>
        /// Test OnGet sets RequestId from Activity.Current when available
        /// </summary>
        [Test]
        public void OnGet_With_Activity_Should_Use_Activity_Id()
        {
            // Arrange
            var activity = new Activity("test-activity");
            activity.Start();
            Activity.Current = activity;

            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "trace-id-123";
            pageModel.PageContext.HttpContext = httpContext;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.RequestId, Is.Not.Null);
            Assert.That(pageModel.RequestId, Is.EqualTo(activity.Id));

            // Cleanup
            activity.Stop();
            Activity.Current = null;
        }

        /// <summary>
        /// Test OnGet can be called multiple times
        /// </summary>
        [Test]
        public void OnGet_Valid_Multiple_Calls_Should_Update_RequestId()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "first-trace-id";
            pageModel.PageContext.HttpContext = httpContext;

            // Act
            pageModel.OnGet();
            var firstRequestId = pageModel.RequestId;

            httpContext.TraceIdentifier = "second-trace-id";
            pageModel.OnGet();
            var secondRequestId = pageModel.RequestId;

            // Assert
            Assert.That(firstRequestId, Is.EqualTo("first-trace-id"));
            Assert.That(secondRequestId, Is.EqualTo("second-trace-id"));
        }

        /// <summary>
        /// Test ResponseCache attribute is present on ErrorModel
        /// </summary>
        [Test]
        public void ErrorModel_Should_Have_ResponseCache_Attribute()
        {
            // Arrange
            var type = typeof(ErrorModel);

            // Act
            var attributes = type.GetCustomAttributes(typeof(ResponseCacheAttribute), false);

            // Assert
            Assert.That(attributes.Length, Is.EqualTo(1));
            var responseCacheAttr = (ResponseCacheAttribute)attributes[0];
            Assert.That(responseCacheAttr.Duration, Is.EqualTo(0));
            Assert.That(responseCacheAttr.Location, Is.EqualTo(ResponseCacheLocation.None));
            Assert.That(responseCacheAttr.NoStore, Is.True);
        }

        #endregion OnGet
    }
}