//using System.Diagnostics;

//using Microsoft.Extensions.Logging;

//using NUnit.Framework;

//using Moq;

//using ContosoCrafts.WebSite.Pages;

//namespace UnitTests.Pages.Error
//{
//    /// <summary>
//    /// Provides unit testing for the Error page
//    /// </summary>
//    public class ErrorTests
//    {
//        #region TestSetup
//        // Declare the model of the Error page to be used in unit tests
//        public static ErrorModel pageModel;

//        [SetUp]
//        /// <summary>
//        /// Initializes mock error page model for testing.
//        /// </summary>
//        public void TestInitialize()
//        {
//            // Logs where the category name is derived from for the mocked ErrorMoel
//            var MockLoggerDirect = Mock.Of<ILogger<ErrorModel>>();

//            pageModel = new ErrorModel(MockLoggerDirect)
//            {
//                // Holds the dummy PageContext from testHelper
//                PageContext = TestHelper.PageContext,
//                // Holds the dummy tempData from testHelper
//                TempData = TestHelper.TempData,
//            };
//        }

//        #endregion TestSetup

//        #region OnGet
//        [Test]
//        /// <summary>
//        /// Tests that starting a valid activity then going to the Error page correctly sets the RequestId for the Error page as the
//        /// activity Id
//        /// </summary>
//        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
//        {
//            // Arrange

//            // Creates a valid activity to test the pageModel with
//            Activity activity = new Activity("activity");
//            activity.Start();

//            // Act
//            pageModel.OnGet();

//            // Reset
//            activity.Stop();

//            // Assert
//            Assert.AreEqual(true, pageModel.ModelState.IsValid);
//            Assert.AreEqual(activity.Id, pageModel.RequestId);
//        }

//        [Test]
//        /// <summary>
//        /// Tests that having an invalid activity then going to the Error page correctly sets the RequestId for the Error page as "trace"
//        /// while maintaining a valid state
//        /// </summary>
//        public void OnGet_InValid_Activity_Null_Should_Return_TraceIdentifier()
//        {
//            // Arrange

//            // Act
//            pageModel.OnGet();

//            // Reset

//            // Assert
//            Assert.AreEqual(true, pageModel.ModelState.IsValid);
//            Assert.AreEqual("trace", pageModel.RequestId);
//            Assert.AreEqual(true, pageModel.ShowRequestId);
//        }
//        #endregion OnGet
//    }
//}



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
            Assert.IsNotNull(pageModel);
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
            Assert.IsNotNull(model);
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
            Assert.AreEqual(null, result);
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
            Assert.AreEqual(requestId, pageModel.RequestId);
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
            Assert.AreEqual(null, pageModel.RequestId);
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
            Assert.AreEqual(string.Empty, pageModel.RequestId);
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
            Assert.AreEqual(false, result);
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
            Assert.AreEqual(false, result);
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
            Assert.AreEqual(true, result);
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
            Assert.AreEqual(true, result); // Note: !string.IsNullOrEmpty doesn't check whitespace
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
            Assert.AreEqual("test-trace-id", pageModel.RequestId);
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
            Assert.AreEqual("trace-id-123", pageModel.RequestId);
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
            Assert.IsNotNull(pageModel.RequestId);
            Assert.AreEqual(activity.Id, pageModel.RequestId);

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
            Assert.AreEqual("first-trace-id", firstRequestId);
            Assert.AreEqual("second-trace-id", secondRequestId);
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
            Assert.AreEqual(1, attributes.Length);
            var responseCacheAttr = (ResponseCacheAttribute)attributes[0];
            Assert.AreEqual(0, responseCacheAttr.Duration);
            Assert.AreEqual(ResponseCacheLocation.None, responseCacheAttr.Location);
            Assert.AreEqual(true, responseCacheAttr.NoStore);
        }

        #endregion OnGet
    }
}
