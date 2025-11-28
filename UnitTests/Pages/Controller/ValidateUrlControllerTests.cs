using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace UnitTests.Controllers
{
    /// <summary>
    /// Unit tests for ValidateUrlController class
    /// Tests all methods to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class ValidateUrlControllerTests
    {
        #region TestSetup

        // Controller instance for testing
        private ValidateUrlController Controller;

        // Mock HTTP client factory
        private Mock<IHttpClientFactory> MockHttpClientFactory;

        // Mock HTTP message handler
        private Mock<HttpMessageHandler> MockHttpMessageHandler;

        /// <summary>
        /// Initialize test environment before each test
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create mock HTTP message handler
            MockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Create HTTP client with mock handler
            var httpClient = new HttpClient(MockHttpMessageHandler.Object);

            // Create mock HTTP client factory
            MockHttpClientFactory = new Mock<IHttpClientFactory>();

            MockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            // Create controller with mock factory
            Controller = new ValidateUrlController(MockHttpClientFactory.Object);
        }

        #endregion TestSetup

        #region Constructor

        /// <summary>
        /// Test constructor initializes controller correctly
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Create_Instance()
        {
            // Arrange

            // Act
            var result = new ValidateUrlController(MockHttpClientFactory.Object);

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        #endregion Constructor

        #region OnGet

        /// <summary>
        /// Test OnGet with null URL returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_Null_Url_Should_Return_Invalid_Result()
        {
            // Arrange
            string url = null;

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(0));
            Assert.That(validationResult.Message, Is.EqualTo("URL is required"));
        }

        /// <summary>
        /// Test OnGet with empty URL returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_Empty_Url_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = string.Empty;

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(0));
            Assert.That(validationResult.Message, Is.EqualTo("URL is required"));
        }

        /// <summary>
        /// Test OnGet with invalid URL format returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_Url_Format_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "not-a-valid-url";

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(0));
            Assert.That(validationResult.Message, Is.EqualTo("Invalid URL format"));
        }

        /// <summary>
        /// Test OnGet with non-HTTP scheme returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_Non_Http_Scheme_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "ftp://example.com";

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(0));
            Assert.That(validationResult.Message, Is.EqualTo("URL must use HTTP or HTTPS"));
        }

        /// <summary>
        /// Test OnGet with file scheme returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_File_Scheme_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "file:///C:/test.txt";

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("URL must use HTTP or HTTPS"));
        }

        /// <summary>
        /// Test OnGet with valid URL returning 200 status returns valid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_200_Status_Should_Return_Valid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.True);
            Assert.That(validationResult.StatusCode, Is.EqualTo(200));
            Assert.That(validationResult.Message, Is.EqualTo("URL is valid"));
        }

        /// <summary>
        /// Test OnGet with valid URL returning 201 status returns valid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_201_Status_Should_Return_Valid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.True);
            Assert.That(validationResult.StatusCode, Is.EqualTo(201));
        }

        /// <summary>
        /// Test OnGet with valid URL returning 204 status returns valid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_204_Status_Should_Return_Valid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.True);
            Assert.That(validationResult.StatusCode, Is.EqualTo(204));
        }

        /// <summary>
        /// Test OnGet with HTTP URL returning 200 status returns valid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Http_Url_With_200_Status_Should_Return_Valid_Result()
        {
            // Arrange
            var url = "http://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.True);
            Assert.That(validationResult.StatusCode, Is.EqualTo(200));
        }

        /// <summary>
        /// Test OnGet with URL returning 404 status returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_404_Status_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "https://example.com/notfound";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(404));
            Assert.That(validationResult.Message, Is.EqualTo("URL returned non-success status"));
        }

        /// <summary>
        /// Test OnGet with URL returning 500 status returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_500_Status_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(500));
        }

        /// <summary>
        /// Test OnGet with URL returning 301 redirect returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_301_Status_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.MovedPermanently
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(301));
        }

        /// <summary>
        /// Test OnGet with URL returning 403 forbidden returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_403_Status_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Forbidden
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(403));
        }

        /// <summary>
        /// Test OnGet with timeout returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_Timeout_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("Request timed out"));

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(0));
            Assert.That(validationResult.Message, Is.EqualTo("Request timed out"));
        }

        /// <summary>
        /// Test OnGet with HTTP request exception returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_HttpRequestException_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Unable to reach URL"));

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(0));
            Assert.That(validationResult.Message, Is.EqualTo("Unable to reach URL"));
        }

        /// <summary>
        /// Test OnGet with whitespace URL returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_Whitespace_Url_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "   ";

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(validationResult.IsValid, Is.False);
        }

        /// <summary>
        /// Test OnGet uses HEAD method for request
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_Should_Use_Head_Method()
        {
            // Arrange
            var url = "https://example.com";

            HttpMethod capturedMethod = null;

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
                {
                    capturedMethod = request.Method;
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            await Controller.OnGet(url);

            // Reset

            // Assert
            Assert.That(capturedMethod, Is.EqualTo(HttpMethod.Head));
        }

        #endregion OnGet

        #region UrlValidationResultModel

        /// <summary>
        /// Test UrlValidationResultModel IsValid property can be set
        /// </summary>
        [Test]
        public void UrlValidationResultModel_IsValid_Should_Be_Settable()
        {
            // Arrange
            var model = new UrlValidationResultModel();

            // Act
            model.IsValid = true;

            // Reset

            // Assert
            Assert.That(model.IsValid, Is.True);
        }

        /// <summary>
        /// Test UrlValidationResultModel StatusCode property can be set
        /// </summary>
        [Test]
        public void UrlValidationResultModel_StatusCode_Should_Be_Settable()
        {
            // Arrange
            var model = new UrlValidationResultModel();

            // Act
            model.StatusCode = 200;

            // Reset

            // Assert
            Assert.That(model.StatusCode, Is.EqualTo(200));
        }

        /// <summary>
        /// Test UrlValidationResultModel Message property can be set
        /// </summary>
        [Test]
        public void UrlValidationResultModel_Message_Should_Be_Settable()
        {
            // Arrange
            var model = new UrlValidationResultModel();

            var data = "Test message";

            // Act
            model.Message = data;

            // Reset

            // Assert
            Assert.That(model.Message, Is.EqualTo(data));
        }

        /// <summary>
        /// Test UrlValidationResultModel default values
        /// </summary>
        [Test]
        public void UrlValidationResultModel_Default_Values_Should_Be_Correct()
        {
            // Arrange

            // Act
            var model = new UrlValidationResultModel();

            // Reset

            // Assert
            Assert.That(model.IsValid, Is.False);
            Assert.That(model.StatusCode, Is.EqualTo(0));
            Assert.That(model.Message, Is.Null);
        }

        #endregion UrlValidationResultModel

        #region IsSuccessStatusCode Coverage

        /// <summary>
        /// Test OnGet with URL returning 200 status hits success path
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_200_Status_Should_Return_Valid_Message()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.True);
            Assert.That(validationResult.Message, Is.EqualTo("URL is valid"));
        }

        /// <summary>
        /// Test OnGet with URL returning 199 status fails less than 200 check
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_199_Status_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode)199
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(199));
            Assert.That(validationResult.Message, Is.EqualTo("URL returned non-success status"));
        }

        /// <summary>
        /// Test OnGet with URL returning 299 status is still valid
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_299_Status_Should_Return_Valid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode)299
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.True);
            Assert.That(validationResult.StatusCode, Is.EqualTo(299));
        }

        /// <summary>
        /// Test OnGet with URL returning 300 status fails greater than or equal 300 check
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_With_300_Status_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.MultipleChoices
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(300));
            Assert.That(validationResult.Message, Is.EqualTo("URL returned non-success status"));
        }

        #endregion IsSuccessStatusCode Coverage

        #region HTTP and HTTPS Scheme Coverage

        /// <summary>
        /// Test OnGet with HTTP scheme URL returns valid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Http_Scheme_Url_Should_Call_ValidateUrlAsync()
        {
            // Arrange
            var url = "http://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.True);
            Assert.That(validationResult.StatusCode, Is.EqualTo(200));
        }

        /// <summary>
        /// Test OnGet with HTTPS scheme URL returns valid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Https_Scheme_Url_Should_Call_ValidateUrlAsync()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.True);
            Assert.That(validationResult.StatusCode, Is.EqualTo(200));
        }

        /// <summary>
        /// Test OnGet with HTTP scheme URL that fails returns invalid result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Http_Scheme_Url_With_Error_Should_Return_Invalid_Result()
        {
            // Arrange
            var url = "http://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.StatusCode, Is.EqualTo(404));
        }

        /// <summary>
        /// Test OnGet with HTTP scheme URL that times out returns timeout result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Http_Scheme_Url_With_Timeout_Should_Return_Timeout_Result()
        {
            // Arrange
            var url = "http://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("Request timed out"));

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("Request timed out"));
        }

        /// <summary>
        /// Test OnGet with HTTP scheme URL that throws HttpRequestException returns unable to reach result
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Http_Scheme_Url_With_HttpRequestException_Should_Return_Unable_To_Reach_Result()
        {
            // Arrange
            var url = "http://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Connection refused"));

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("Unable to reach URL"));
        }

        #endregion HTTP and HTTPS Scheme Coverage

        #region GetValidationMessage Coverage

        /// <summary>
        /// Test OnGet with valid URL returns URL is valid message
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_Success_Should_Return_Url_Is_Valid_Message()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.Message, Is.EqualTo("URL is valid"));
        }

        /// <summary>
        /// Test OnGet with invalid URL returns non-success status message
        /// </summary>
        [Test]
        public async Task OnGet_Valid_Url_Failure_Should_Return_Non_Success_Status_Message()
        {
            // Arrange
            var url = "https://example.com";

            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.Message, Is.EqualTo("URL returned non-success status"));
        }

        #endregion GetValidationMessage Coverage

        #region Edge Cases

        /// <summary>
        /// Test OnGet with FTP scheme returns invalid scheme message
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_Ftp_Scheme_Should_Return_Invalid_Scheme_Message()
        {
            // Arrange
            var url = "ftp://example.com";

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("URL must use HTTP or HTTPS"));
        }

        /// <summary>
        /// Test OnGet with file scheme returns invalid scheme message
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_File_Scheme_Should_Return_Invalid_Scheme_Message()
        {
            // Arrange
            var url = "file:///C:/test.txt";

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("URL must use HTTP or HTTPS"));
        }

        /// <summary>
        /// Test OnGet with mailto scheme returns invalid scheme message
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_Mailto_Scheme_Should_Return_Invalid_Scheme_Message()
        {
            // Arrange
            var url = "mailto:test@example.com";

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("URL must use HTTP or HTTPS"));
        }

        /// <summary>
        /// Test OnGet with invalid URL format returns invalid format message
        /// </summary>
        [Test]
        public async Task OnGet_Invalid_Url_Format_Should_Return_Invalid_Format_Message()
        {
            // Arrange
            var url = "not-a-valid-url";

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("Invalid URL format"));
        }

        /// <summary>
        /// Test OnGet with null URL returns URL required message
        /// </summary>
        [Test]
        public async Task OnGet_Null_Url_Should_Return_Url_Required_Message()
        {
            // Arrange
            string url = null;

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("URL is required"));
        }

        /// <summary>
        /// Test OnGet with empty URL returns URL required message
        /// </summary>
        [Test]
        public async Task OnGet_Empty_Url_Should_Return_Url_Required_Message()
        {
            // Arrange
            var url = string.Empty;

            // Act
            var result = await Controller.OnGet(url);

            var okResult = result as OkObjectResult;

            var validationResult = okResult.Value as UrlValidationResultModel;

            // Reset

            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Message, Is.EqualTo("URL is required"));
        }

        #endregion Edge Cases

    }

}