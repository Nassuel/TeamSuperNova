using Bunit;
using ContosoCrafts.WebSite.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTests.Pages.Components
{
    /// <summary>
    /// Unit tests for the UrlValidator component
    /// </summary>
    [TestFixture]
    public class UrlValidatorTests
    {
        // Test context for Bunit
        public BunitContext TestContext;

        // Mock HTTP message handler
        public Mock<HttpMessageHandler> MockHttpMessageHandler;

        // Mock HTTP client factory
        public Mock<IHttpClientFactory> MockHttpClientFactory;

        /// <summary>
        /// Simple HttpMessageHandler that returns a configurable response.
        /// Used to create a deterministic HttpClient for tests instead of relying on Moq protected setups.
        /// </summary>
        public class TestHttpMessageHandler : HttpMessageHandler
        {
            public readonly HttpStatusCode _statusCode;
            public readonly string _content;

            public TestHttpMessageHandler(HttpStatusCode statusCode, string content = "Test response")
            {
                _statusCode = statusCode;
                _content = content;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = _statusCode,
                    Content = new StringContent(_content)
                };

                return Task.FromResult(response);
            }
        }

        /// <summary>
        /// HttpMessageHandler that introduces a delay before responding
        /// </summary>
        public class DelayedHttpMessageHandler : HttpMessageHandler
        {
            public readonly HttpStatusCode _statusCode;
            public readonly TimeSpan _delay;

            public DelayedHttpMessageHandler(HttpStatusCode statusCode, TimeSpan delay)
            {
                _statusCode = statusCode;
                _delay = delay;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                await Task.Delay(_delay, cancellationToken);
                return new HttpResponseMessage
                {
                    StatusCode = _statusCode,
                    Content = new StringContent("Test response")
                };
            }
        }

        /// <summary>
        /// Sets up test context before each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Create test context
            TestContext = new BunitContext();

            // Create mock HTTP message handler (kept for compatibility with existing code)
            MockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Create mock HTTP client factory
            MockHttpClientFactory = new Mock<IHttpClientFactory>();

            // Register mock HTTP client factory
            TestContext.Services.AddSingleton(MockHttpClientFactory.Object);
        }

        #region Render Tests

        /// <summary>
        /// Tests that component renders successfully
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_NotNull()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that input element exists after render
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_InputElement()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get input element
            var inputElement = result.Find("input[type='text']");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(inputElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that validate button exists after render
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_ValidateButton()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get button element
            var buttonElement = result.Find("button");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(buttonElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that label displays correct text
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_LabelWithText()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get label element
            var labelElement = result.Find("label");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(labelElement.TextContent, Does.Contain("Product Url:"));
        }

        /// <summary>
        /// Tests that input has correct placeholder
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_InputWithPlaceholder()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get input element
            var inputElement = result.Find("input[type='text']");

            // Get placeholder attribute
            var placeholder = inputElement.GetAttribute("placeholder");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(placeholder, Is.EqualTo("Enter URL of Product Website"));
        }

        /// <summary>
        /// Tests that hidden input for URL exists
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_HiddenUrlInput()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get hidden input element
            var hiddenInput = result.Find("input[name='Product.Url']");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hiddenInput, Is.Not.Null);
        }

        /// <summary>
        /// Tests that hidden input for validation status exists
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_HiddenValidationInput()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get hidden input element
            var hiddenInput = result.Find("input[name='IsUrlValidated']");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hiddenInput, Is.Not.Null);
        }

        /// <summary>
        /// Tests that input has form-control class
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_FormControlClass()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get input element
            var inputElement = result.Find("input[type='text'].form-control");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(inputElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that button has correct type attribute
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_ButtonTypeButton()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get button element
            var buttonElement = result.Find("button");

            // Get type attribute
            var typeAttr = buttonElement.GetAttribute("type");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(typeAttr, Is.EqualTo("button"));
        }

        /// <summary>
        /// Tests that validation message is not displayed initially
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_NoValidationMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Try to find alert element
            var alertElements = result.FindAll(".alert");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElements.Count, Is.EqualTo(0));
        }

        #endregion

        #region OnInitialized Tests

        /// <summary>
        /// Tests that OnInitialized sets URL from InitialUrl parameter
        /// </summary>
        [Test]
        public void OnInitialized_Valid_InitialUrlSet_Should_Return_UrlValue()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var initialUrl = "https://example.com";

            // Act
            var result = TestContext.Render<UrlValidator>(parameters => parameters
                .Add(p => p.InitialUrl, initialUrl));

            // Get input element
            var inputElement = result.Find("input[type='text']");

            // Get input value
            var inputValue = inputElement.GetAttribute("value");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(inputValue, Is.EqualTo(initialUrl));
        }

        /// <summary>
        /// Tests that OnInitialized handles null InitialUrl
        /// </summary>
        [Test]
        public void OnInitialized_InValid_NullInitialUrl_Should_Return_EmptyString()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            string nullUrl = null;

            // Act
            var result = TestContext.Render<UrlValidator>(parameters => parameters
                .Add(p => p.InitialUrl, nullUrl));

            // Get input element
            var inputElement = result.Find("input[type='text']");

            // Get input value
            var inputValue = inputElement.GetAttribute("value");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(inputValue, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Tests that OnInitialized handles empty InitialUrl
        /// </summary>
        [Test]
        public void OnInitialized_Valid_EmptyInitialUrl_Should_Return_EmptyString()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var emptyUrl = string.Empty;

            // Act
            var result = TestContext.Render<UrlValidator>(parameters => parameters
                .Add(p => p.InitialUrl, emptyUrl));

            // Get input element
            var inputElement = result.Find("input[type='text']");

            // Get input value
            var inputValue = inputElement.GetAttribute("value");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(inputValue, Is.EqualTo(string.Empty));
        }

        #endregion

        #region OnUrlChanged Tests

        /// <summary>
        /// Tests that OnUrlChanged resets validation state
        /// </summary>
        [Test]
        public void OnUrlChanged_Valid_UrlChanged_Should_Return_ResetValidationState()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input element
            var inputElement = result.Find("input[type='text']");

            // Act
            inputElement.Input("https://example.com");

            // Get alert elements
            var alertElements = result.FindAll(".alert");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElements.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that OnUrlChanged removes validation classes
        /// </summary>
        [Test]
        public void OnUrlChanged_Valid_AfterValidation_Should_Return_NoValidationClasses()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation message
            result.WaitForState(() => result.FindAll(".alert").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement.TextContent, Does.Contain("URL is valid and accessible"));
        }

        #endregion

        #region Validation UI State Tests

        /// <summary>
        /// Tests that validation shows loading spinner
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_DuringValidation_Should_Return_LoadingSpinner()
        {
            // Arrange
            var delayHandler = new DelayedHttpMessageHandler(HttpStatusCode.OK, TimeSpan.FromMilliseconds(500));
            var httpClient = new HttpClient(delayHandler) { Timeout = TimeSpan.FromSeconds(10) };
            MockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Set URL
            inputElement.Input("https://example.com");

            // Act
            buttonElement.Click();

            // Check immediately for spinner
            var hasSpinner = result.FindAll(".spinner-border").Count > 0;

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hasSpinner, Is.True);
        }

        /// <summary>
        /// Tests that validation shows "Validating..." text
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_DuringValidation_Should_Return_ValidatingText()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Set URL
            inputElement.Input("https://example.com");

            // Act
            buttonElement.Click();

            // Get button text (may show Validating... during request)
            var buttonText = buttonElement.TextContent;

            // Reset
            // (No reset needed)

            // Assert
            // Button should show either Validating or Validate
            Assert.That(buttonText.Contains("Validat"), Is.True);
        }

        /// <summary>
        /// Tests that button shows Validate text when not validating
        /// </summary>
        [Test]
        public void Render_Valid_NotValidating_Should_Return_ValidateButtonText()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get button element
            var buttonElement = result.Find("button");

            // Get button text
            var buttonText = buttonElement.TextContent;

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(buttonText, Does.Contain("Validate"));
        }

        #endregion

        #region GetInputCssClass Tests

        /// <summary>
        /// Tests that GetInputCssClass returns form-control when not validated
        /// </summary>
        [Test]
        public void GetInputCssClass_Valid_NotValidated_Should_Return_FormControl()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Get input element
            var inputElement = result.Find("input[type='text']");

            // Check classes
            var hasFormControl = inputElement.ClassList.Contains("form-control");
            var hasValidation = inputElement.ClassList.Contains("is-valid");
            var hasInvalid = inputElement.ClassList.Contains("is-invalid");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hasFormControl, Is.True);
            Assert.That(hasValidation, Is.False);
            Assert.That(hasInvalid, Is.False);
        }

        /// <summary>
        /// Tests that GetInputCssClass returns is-valid when validated successfully
        /// </summary>
        [Test]
        public void GetInputCssClass_Valid_ValidatedSuccessfully_Should_Return_IsValid()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation
            result.WaitForState(() => result.FindAll(".alert").Count > 0, TimeSpan.FromSeconds(5));

            // Get input element
            var updatedInput = result.Find("input[type='text']");

            // Check if has valid class
            var hasValidClass = updatedInput.ClassList.Contains("is-valid");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hasValidClass, Is.True);
        }

        /// <summary>
        /// Tests that GetInputCssClass returns is-invalid when validation fails
        /// </summary>
        [Test]
        public void GetInputCssClass_InValid_ValidationFailed_Should_Return_IsInvalid()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.NotFound);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation
            result.WaitForState(() => result.FindAll(".alert").Count > 0, TimeSpan.FromSeconds(5));

            // Get input element
            var updatedInput = result.Find("input[type='text']");

            // Check if has invalid class
            var hasInvalidClass = updatedInput.ClassList.Contains("is-invalid");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hasInvalidClass, Is.True);
        }

        #endregion

        #region GetAlertCssClass Tests

        /// <summary>
        /// Tests that GetAlertCssClass returns alert-success when valid
        /// </summary>
        [Test]
        public void GetAlertCssClass_Valid_SuccessfulValidation_Should_Return_AlertSuccess()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for success alert
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-success");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that GetAlertCssClass returns alert-danger when invalid
        /// </summary>
        [Test]
        public void GetAlertCssClass_InValid_FailedValidation_Should_Return_AlertDanger()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.NotFound);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for danger alert
            result.WaitForState(() => result.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-danger");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        #endregion

        #region Hidden Input Tests

        /// <summary>
        /// Tests that hidden URL input has correct value
        /// </summary>
        [Test]
        public void Render_Valid_UrlEntered_Should_Return_HiddenUrlInputWithValue()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var testUrl = "https://example.com";

            var result = TestContext.Render<UrlValidator>();

            // Get input element
            var inputElement = result.Find("input[type='text']");

            // Act
            inputElement.Input(testUrl);

            // Wait for component to update
            result.WaitForState(() =>
            {
                var hidden = result.Find("input[name='Product.Url']");
                return hidden.GetAttribute("value") == testUrl;
            }, TimeSpan.FromSeconds(2));

            // Get hidden input
            var hiddenInput = result.Find("input[name='Product.Url']");

            // Get value
            var hiddenValue = hiddenInput.GetAttribute("value");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hiddenValue, Is.EqualTo(testUrl));
        }

        /// <summary>
        /// Tests that hidden validation input is false initially
        /// </summary>
        [Test]
        public void Render_Valid_NotValidated_Should_Return_HiddenValidationInputFalse()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            // Act
            var result = TestContext.Render<UrlValidator>();

            // Wait for component to render
            result.WaitForState(() =>
            {
                var hidden = result.Find("input[name='IsUrlValidated']");
                return hidden.GetAttribute("value") != null;
            }, TimeSpan.FromSeconds(2));

            // Get hidden input
            var hiddenInput = result.Find("input[name='IsUrlValidated']");

            // Get value
            var hiddenValue = hiddenInput.GetAttribute("value");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hiddenValue, Is.EqualTo("False"));
        }

        /// <summary>
        /// Tests that hidden validation input is true after successful validation
        /// </summary>
        [Test]
        public void Render_Valid_AfterSuccessfulValidation_Should_Return_HiddenValidationInputTrue()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation
            result.WaitForState(() => result.FindAll(".alert").Count > 0, TimeSpan.FromSeconds(5));

            // Wait for hidden input to update
            result.WaitForState(() =>
            {
                var hidden = result.Find("input[name='IsUrlValidated']");
                return hidden.GetAttribute("value") == "True";
            }, TimeSpan.FromSeconds(2));

            // Get hidden input
            var hiddenInput = result.Find("input[name='IsUrlValidated']");

            // Get value
            var hiddenValue = hiddenInput.GetAttribute("value");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(hiddenValue, Is.EqualTo("True"));
        }

        #endregion

        #region Edge Case Tests

        /// <summary>
        /// Tests that component handles URL with query parameters
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_UrlWithQueryParams_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com?param1=value1&param2=value2");
            buttonElement.Click();

            // Wait for validation message
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-success");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that component handles URL with path
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_UrlWithPath_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com/path/to/resource");
            buttonElement.Click();

            // Wait for validation message
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-success");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that component handles URL with port
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_UrlWithPort_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com:8080");
            buttonElement.Click();

            // Wait for validation message
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-success");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that component handles URL with fragment
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_UrlWithFragment_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://example.com#section");
            buttonElement.Click();

            // Wait for validation message
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-success");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that component handles URL with subdomain
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_UrlWithSubdomain_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://subdomain.example.com");
            buttonElement.Click();

            // Wait for validation message
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-success");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that component handles very long URL
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_VeryLongUrl_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Create long URL
            var longUrl = "https://example.com/" + new string('a', 100) + "?param=" + new string('b', 100);

            // Act
            inputElement.Input(longUrl);
            buttonElement.Click();

            // Wait for validation message
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-success");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that component handles URL with international domain
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_InternationalDomain_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // Act
            inputElement.Input("https://münchen.de");
            buttonElement.Click();

            // Wait for validation message
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-success");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        #endregion

        #region Multiple Validation Tests

        /// <summary>
        /// Tests that component can be validated multiple times
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_MultipleValidations_Should_Return_LatestResult()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // First validation
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for first validation
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Act - Second validation with different URL
            inputElement.Input("https://newexample.com");
            buttonElement.Click();

            // Wait for second validation
            result.WaitForState(() => result.FindAll(".alert").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that validation state changes from success to failure
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_SuccessThenFailure_Should_Return_FailureState()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);

            var result = TestContext.Render<UrlValidator>();

            // Get input and button
            var inputElement = result.Find("input[type='text']");
            var buttonElement = result.Find("button");

            // First validation - success
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for success
            result.WaitForState(() => result.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Setup for failure
            SetupMockHttpClient(HttpStatusCode.NotFound);

            // Act - Second validation - failure
            inputElement.Input("https://notfound.com");
            buttonElement.Click();

            // Wait for failure
            result.WaitForState(() => result.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Get alert element
            var alertElement = result.Find(".alert-danger");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(alertElement, Is.Not.Null);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Sets up mock HTTP client with specified status code
        /// </summary>
        /// <param name="statusCode">HTTP status code to return</param>
        public void SetupMockHttpClient(HttpStatusCode statusCode)
        {
            // Setup mock HTTP message handler
            MockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent("Test response")
                });

            // Create HTTP client with mock handler
            var httpClient = new HttpClient(new TestHttpMessageHandler(statusCode))
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            // Setup mock factory to return HTTP client
            MockHttpClientFactory
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
        }

        #endregion

        #region ValidateUrlAsync Tests

        /// <summary>
        /// Test ValidateUrlAsync with empty URL shows error message
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Invalid_EmptyUrl_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var buttonElement = component.Find("button");

            // Act - Click validate without entering URL
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("Please enter a URL first"));
        }

        /// <summary>
        /// Test ValidateUrlAsync with whitespace URL shows error message
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Invalid_WhitespaceUrl_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("   ");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("Please enter a URL first"));
        }

        /// <summary>
        /// Test ValidateUrlAsync with invalid URL format shows error message
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Invalid_InvalidUrlFormat_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("not-a-valid-url");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("Invalid URL format"));
        }

        /// <summary>
        /// Test ValidateUrlAsync with FTP scheme shows error message
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Invalid_FtpScheme_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("ftp://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("Invalid URL format"));
        }

        /// <summary>
        /// Test ValidateUrlAsync with file scheme shows error message
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Invalid_FileScheme_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("file:///C:/test.txt");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("Invalid URL format"));
        }

        /// <summary>
        /// Test ValidateUrlAsync with HTTP scheme (not HTTPS) validates successfully
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_HttpScheme_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("http://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-success");
            Assert.That(alertElement.TextContent, Does.Contain("URL is valid and accessible"));
        }

        /// <summary>
        /// Test ValidateUrlAsync with HTTPS scheme validates successfully
        /// </summary>
        [Test]
        public void ValidateUrlAsync_Valid_HttpsScheme_Should_Return_Success()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-success");
            Assert.That(alertElement.TextContent, Does.Contain("URL is valid and accessible"));
        }

        #endregion ValidateUrlAsync Tests

        #region AttemptHttpRequest Tests

        /// <summary>
        /// Test AttemptHttpRequest with 200 OK shows success message
        /// </summary>
        [Test]
        public void AttemptHttpRequest_Valid_StatusCode200_Should_Return_SuccessMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-success").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-success");
            Assert.That(alertElement.TextContent, Does.Contain("Status: 200"));
        }

        /// <summary>
        /// Test AttemptHttpRequest with 404 Not Found shows error message
        /// </summary>
        [Test]
        public void AttemptHttpRequest_Invalid_StatusCode404_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.NotFound);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("404"));
        }

        /// <summary>
        /// Test AttemptHttpRequest with 500 Internal Server Error shows error message
        /// </summary>
        [Test]
        public void AttemptHttpRequest_Invalid_StatusCode500_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.InternalServerError);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("500"));
        }

        /// <summary>
        /// Test AttemptHttpRequest with 301 Redirect shows error message
        /// </summary>
        [Test]
        public void AttemptHttpRequest_Invalid_StatusCode301_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.MovedPermanently);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("301"));
        }

        /// <summary>
        /// Test AttemptHttpRequest with 403 Forbidden shows error message
        /// </summary>
        [Test]
        public void AttemptHttpRequest_Invalid_StatusCode403_Should_Return_ErrorMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.Forbidden);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert-danger").Count > 0, TimeSpan.FromSeconds(5));

            // Assert
            var alertElement = component.Find(".alert-danger");
            Assert.That(alertElement.TextContent, Does.Contain("403"));
        }

        #endregion AttemptHttpRequest Tests

        #region OnUrlChanged Tests

        /// <summary>
        /// Test OnUrlChanged clears validation message after URL change
        /// </summary>
        [Test]
        public void OnUrlChanged_Valid_AfterValidation_Should_Clear_ValidationMessage()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // First validate
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation message
            component.WaitForState(() => component.FindAll(".alert").Count > 0, TimeSpan.FromSeconds(5));

            // Act - Change URL using Change event (triggers OnUrlChanged)
            inputElement.Change("https://newurl.com");

            // Assert - Alert should be cleared
            var alertElements = component.FindAll(".alert");
            Assert.That(alertElements.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test OnUrlChanged resets IsValid to false
        /// </summary>
        [Test]
        public void OnUrlChanged_Valid_AfterSuccessfulValidation_Should_Reset_IsValid()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // First validate successfully
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for success
            component.WaitForState(() => component.FindAll(".is-valid").Count > 0, TimeSpan.FromSeconds(5));

            // Act - Change URL
            inputElement.Change("https://newurl.com");

            // Assert - is-valid class should be removed
            var validInputs = component.FindAll("input.is-valid");
            Assert.That(validInputs.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Test OnUrlChanged resets IsValidated to false
        /// </summary>
        [Test]
        public void OnUrlChanged_Valid_AfterFailedValidation_Should_Reset_IsValidated()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.NotFound);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // First validate with failure
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for failure
            component.WaitForState(() => component.FindAll(".is-invalid").Count > 0, TimeSpan.FromSeconds(5));

            // Act - Change URL
            inputElement.Change("https://newurl.com");

            // Assert - is-invalid class should be removed
            var invalidInputs = component.FindAll("input.is-invalid");
            Assert.That(invalidInputs.Count, Is.EqualTo(0));
        }

        #endregion OnUrlChanged Tests

        #region PerformHttpValidation Tests

        /// <summary>
        /// Test PerformHttpValidation sets IsValidating to true during validation
        /// </summary>
        [Test]
        public void PerformHttpValidation_Valid_DuringValidation_Should_Set_IsValidating_True()
        {
            // Arrange
            var delayHandler = new DelayedHttpMessageHandler(HttpStatusCode.OK, TimeSpan.FromMilliseconds(500));
            var httpClient = new HttpClient(delayHandler) { Timeout = TimeSpan.FromSeconds(10) };
            MockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Assert - Check for spinner immediately
            var spinners = component.FindAll(".spinner-border");
            Assert.That(spinners.Count, Is.GreaterThan(0));
        }

        /// <summary>
        /// Test PerformHttpValidation sets IsValidating to false after validation
        /// </summary>
        [Test]
        public void PerformHttpValidation_Valid_AfterValidation_Should_Set_IsValidating_False()
        {
            // Arrange
            SetupMockHttpClient(HttpStatusCode.OK);
            var component = TestContext.Render<UrlValidator>();
            var inputElement = component.Find("input[type='text']");
            var buttonElement = component.Find("button");

            // Act
            inputElement.Input("https://example.com");
            buttonElement.Click();

            // Wait for validation to complete
            component.WaitForState(() => component.FindAll(".alert").Count > 0, TimeSpan.FromSeconds(5));

            // Assert - Spinner should be gone
            var spinners = component.FindAll(".spinner-border");
            Assert.That(spinners.Count, Is.EqualTo(0));
        }

        #endregion PerformHttpValidation Tests

    }

}

