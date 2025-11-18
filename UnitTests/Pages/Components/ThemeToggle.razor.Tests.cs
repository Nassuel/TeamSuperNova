using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages.Components
{
    /// <summary>
    /// Unit tests for the ThemeToggle component
    /// </summary>
    [TestFixture]
    public class ThemeToggleTests
    {
        // Test context for Bunit
        private BunitContext TestContext;

        // Mock for JavaScript runtime
        private Mock<IJSRuntime> MockJSRuntime;

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
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = result.Find("button");

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button with class
            var buttonElement = result.Find("button.theme-toggle");

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = result.Find("button");

            // Get type attribute
            var typeAttribute = buttonElement.GetAttribute("type");

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = result.Find("button");

            // Get title attribute
            var titleAttribute = buttonElement.GetAttribute("title");

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Wait for component to initialize
            result.WaitForState(() => result.Find("i.fa-moon") != null);

            // Get icon element
            var iconElement = result.Find("i.fa-moon");

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("dark");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Wait for component to initialize
            result.WaitForState(() => result.Find("i.fa-sun") != null);

            // Get icon element
            var iconElement = result.Find("i.fa-sun");

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Wait for component to initialize
            result.WaitForState(() => result.Find("i.theme-icon") != null);

            // Get icon element
            var iconElement = result.Find("i.theme-icon");

            // Reset
            // No reset needed

            // Assert
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that clicking button invokes JavaScript toggle function
        /// </summary>
        [Test]
        public void Click_Valid_ButtonClicked_WithLightTheme_Should_Invoke_JavaScriptToggle()
        {
            // Arrange
            var data = SetupMockJSRuntime("light");

            // Setup toggle to return dark
            MockJSRuntime.Setup(x => x.InvokeAsync<string>("themeManager.toggle", It.IsAny<object[]>()))
                .ReturnsAsync("dark");

            // Render component
            var component = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = component.Find("button");

            // Act
            buttonElement.Click();

            // Reset
            // No reset needed

            // Assert
            MockJSRuntime.Verify(x => x.InvokeAsync<string>("themeManager.toggle", It.IsAny<object[]>()), Times.Once);
        }

        /// <summary>
        /// Tests that icon changes from moon to sun when toggling from light to dark
        /// </summary>
        [Test]
        public void Click_Valid_ButtonClicked_FromLightToDark_Should_Return_SunIcon()
        {
            // Arrange
            var data = SetupMockJSRuntime("light");

            // Setup toggle to return dark
            MockJSRuntime.Setup(x => x.InvokeAsync<string>("themeManager.toggle", It.IsAny<object[]>()))
                .ReturnsAsync("dark");

            // Render component
            var component = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = component.Find("button");

            // Act
            buttonElement.Click();

            // Wait for icon to change
            component.WaitForState(() => component.Find("i.fa-sun") != null);

            // Get result icon
            var result = component.Find("i.fa-sun");

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("dark");

            // Setup toggle to return light
            MockJSRuntime.Setup(x => x.InvokeAsync<string>("themeManager.toggle", It.IsAny<object[]>()))
                .ReturnsAsync("light");

            // Render component
            var component = TestContext.Render<ThemeToggle>();

            // Wait for initial render
            component.WaitForState(() => component.Find("i.fa-sun") != null);

            // Get button
            var buttonElement = component.Find("button");

            // Act
            buttonElement.Click();

            // Wait for icon to change
            component.WaitForState(() => component.Find("i.fa-moon") != null);

            // Get result icon
            var result = component.Find("i.fa-moon");

            // Reset
            // No reset needed

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
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Reset
            // No reset needed

            // Assert
            MockJSRuntime.Verify(x => x.InvokeAsync<string>("themeManager.getTheme", It.IsAny<object[]>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Tests that component defaults to light theme when saved theme is null
        /// </summary>
        [Test]
        public void Initialize_InValid_NullSavedTheme_WithComponentRendered_Should_Return_MoonIcon()
        {
            // Arrange
            MockJSRuntime.Setup(x => x.InvokeAsync<string>("themeManager.getTheme", It.IsAny<object[]>()))
                .ReturnsAsync((string)null);

            var data = MockJSRuntime.Object;

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Wait for initialization
            result.WaitForState(() => result.Find("i.fa-moon") != null);

            // Get moon icon
            var iconElement = result.Find("i.fa-moon");

            // Reset
            // No reset needed

            // Assert
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that multiple clicks toggle between themes correctly
        /// </summary>
        [Test]
        public void Click_Valid_MultipleClicks_WithAlternatingThemes_Should_Return_MoonIcon()
        {
            // Arrange
            var data = SetupMockJSRuntime("light");

            // Setup toggle sequence
            var toggleCallCount = 0;

            MockJSRuntime.Setup(x => x.InvokeAsync<string>("themeManager.toggle", It.IsAny<object[]>()))
                .ReturnsAsync(() =>
                {
                    toggleCallCount++;

                    // Check if odd count
                    if (toggleCallCount % 2 == 1)
                    {
                        return "dark";
                    }

                    return "light";
                });

            // Render component
            var component = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = component.Find("button");

            // Act - First click
            buttonElement.Click();

            // Wait for sun icon
            component.WaitForState(() => component.Find("i.fa-sun") != null);

            // Act - Second click
            buttonElement.Click();

            // Wait for moon icon
            component.WaitForState(() => component.Find("i.fa-moon") != null);

            // Get result icon
            var result = component.Find("i.fa-moon");

            // Reset
            // No reset needed

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that button is not disabled on initial render
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_WithLightTheme_Should_Return_ButtonNotDisabled()
        {
            // Arrange
            var data = SetupMockJSRuntime("light");

            // Act
            var result = TestContext.Render<ThemeToggle>();

            // Get button
            var buttonElement = result.Find("button");

            // Check if disabled
            var isDisabled = buttonElement.HasAttribute("disabled");

            // Reset
            // No reset needed

            // Assert
            Assert.That(isDisabled, Is.False);
        }

        /// <summary>
        /// Sets up mock JavaScript runtime with initial theme
        /// </summary>
        /// <param name="initialTheme">Initial theme value to return</param>
        /// <returns>Mock JavaScript runtime object</returns>
        private IJSRuntime SetupMockJSRuntime(string initialTheme)
        {
            // Setup getTheme to return initial theme
            MockJSRuntime.Setup(x => x.InvokeAsync<string>("themeManager.getTheme", It.IsAny<object[]>()))
                .ReturnsAsync(initialTheme);

            // Determine opposite theme
            var oppositeTheme = "light";

            // Check if light theme
            if (initialTheme == "light")
            {
                oppositeTheme = "dark";
            }

            // Setup toggle to return opposite theme
            MockJSRuntime.Setup(x => x.InvokeAsync<string>("themeManager.toggle", It.IsAny<object[]>()))
                .ReturnsAsync(oppositeTheme);

            return MockJSRuntime.Object;
        }
    }
}