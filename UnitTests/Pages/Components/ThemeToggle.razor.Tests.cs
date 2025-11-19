using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace UnitTests.Pages.Components
{
    /// <summary>
    /// Unit tests for the ThemeToggle component
    /// </summary>
    [TestFixture]
    public class ThemeToggleTests
    {
        // Test context for Bunit
        public BunitContext TestContext;

        // Mock for JavaScript runtime
        public Mock<IJSRuntime> MockJSRuntime;

        /// <summary>
        /// Sets up test context before each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Create test context
            TestContext = new BunitContext();

            // Create mock JS runtime
            MockJSRuntime = new Mock<IJSRuntime>();

            // Register mock JS runtime
            TestContext.Services.AddSingleton(MockJSRuntime.Object);

            // Register ThemeService
            TestContext.Services.AddSingleton<ThemeService>();
        }

        /// <summary>
        /// Cleans up test context after each test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Dispose test context
            TestContext?.Dispose();
        }

        /// <summary>
        /// Tests that component renders successfully with valid setup
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_WithMockJSRuntime_Should_Return_NotNull()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that button element exists after render
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_WithLightTheme_Should_Return_ButtonElement()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = result.Find("button");

            // Assert
            Assert.That(buttonElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that button has theme-toggle CSS class
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_WithLightTheme_Should_Return_ThemeToggleClass()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button with class
            var buttonElement = result.Find("button.theme-toggle");

            // Assert
            Assert.That(buttonElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that button type attribute is button
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_WithLightTheme_Should_Return_TypeButton()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = result.Find("button");

            // Get type attribute
            var typeAttribute = buttonElement.GetAttribute("type");

            // Assert
            Assert.That(typeAttribute, Is.EqualTo("button"));
        }

        /// <summary>
        /// Tests that button has title attribute
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_WithLightTheme_Should_Return_TitleAttribute()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = result.Find("button");

            // Get title attribute
            var titleAttribute = buttonElement.GetAttribute("title");

            // Assert
            Assert.That(titleAttribute, Is.Not.Null);
        }

        /// <summary>
        /// Tests that moon icon displays when theme is light
        /// </summary>
        [Test]
        public void Render_Valid_LightTheme_WithInitialization_Should_Return_MoonIcon()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Wait for component to initialize
            result.WaitForState(() => result.Find("i.fa-moon-o") != null);

            // Get icon element
            var iconElement = result.Find("i.fa-moon-o");

            // Assert
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that sun icon displays when theme is dark
        /// </summary>
        [Test]
        public void Render_Valid_DarkTheme_WithInitialization_Should_Return_SunIcon()
        {
            // Arrange
            SetupMockJSRuntime("dark");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Wait for component to initialize
            result.WaitForState(() => result.Find("i.fa-sun-o") != null);

            // Get icon element
            var iconElement = result.Find("i.fa-sun-o");

            // Assert
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that icon has theme-icon CSS class
        /// </summary>
        [Test]
        public void Render_Valid_LightTheme_WithInitialization_Should_Return_ThemeIconClass()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Wait for component to initialize
            result.WaitForState(() => result.Find("i.theme-icon") != null);

            // Get icon element
            var iconElement = result.Find("i.theme-icon");

            // Assert
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that clicking button invokes JavaScript setTheme function
        /// </summary>
        [Test]
        public void Click_Valid_ButtonClicked_WithLightTheme_Should_Invoke_JavaScriptSetTheme()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Render component
            var component = TestContext.Render<ThemeToggle>();

            // Wait for initialization
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Get button
            var buttonElement = component.Find("button");

            // Act
            buttonElement.Click();

            // Assert - Verify that InvokeAsync was called with setTheme (IJSVoidResult)
            MockJSRuntime.Verify(
                x => x.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                    "themeManager.setTheme",
                    It.Is<object[]>(args => args.Length == 1 && args[0].ToString() == "dark")),
                Times.Once);
        }

        /// <summary>
        /// Tests that icon changes from moon to sun when toggling from light to dark
        /// </summary>
        [Test]
        public void Click_Valid_ButtonClicked_FromLightToDark_Should_Return_SunIcon()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Render component
            var component = TestContext.Render<ThemeToggle>();

            // Wait for initialization
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Get button
            var buttonElement = component.Find("button");

            // Act
            buttonElement.Click();

            // Wait for icon to change
            component.WaitForState(() => component.Find("i.fa-sun-o") != null);

            // Get result icon
            var result = component.Find("i.fa-sun-o");

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that icon changes from sun to moon when toggling from dark to light
        /// </summary>
        [Test]
        public void Click_Valid_ButtonClicked_FromDarkToLight_Should_Return_MoonIcon()
        {
            // Arrange
            SetupMockJSRuntime("dark");

            // Render component
            var component = TestContext.Render<ThemeToggle>();

            // Wait for initial render
            component.WaitForState(() => component.Find("i.fa-sun-o") != null);

            // Get button
            var buttonElement = component.Find("button");

            // Act
            buttonElement.Click();

            // Wait for icon to change
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Get result icon
            var result = component.Find("i.fa-moon-o");

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that component calls getTheme on initialization
        /// </summary>
        [Test]
        public void Initialize_Valid_ComponentRendered_WithMockJSRuntime_Should_Invoke_GetTheme()
        {
            // Arrange
            SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Wait for initialization
            result.WaitForState(() => result.Find("i.fa-moon-o") != null);

            // Assert
            MockJSRuntime.Verify(x => x.InvokeAsync<string>("themeManager.getTheme", It.IsAny<object[]>()),
                Times.AtLeastOnce);
        }

        /// <summary>
        /// Helper to setup JSRuntime mock to return a theme value
        /// </summary>
        /// <param name="theme">Theme value to return ("light" or "dark")</param>
        private void SetupMockJSRuntime(string theme)
        {
            MockJSRuntime
                .Setup(x => x.InvokeAsync<string>("themeManager.getTheme", It.IsAny<object[]>()))
                .ReturnsAsync(theme);

            // Match the exact generic type used by InvokeVoidAsync (IJSVoidResult) to avoid InvalidCastException
            MockJSRuntime
                .Setup(x => x.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>("themeManager.setTheme", It.IsAny<object[]>()))
                .Returns(new ValueTask<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(default(Microsoft.JSInterop.Infrastructure.IJSVoidResult)));
        }
    }
}