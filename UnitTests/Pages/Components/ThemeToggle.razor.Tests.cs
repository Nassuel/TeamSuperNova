using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using NUnit.Framework;
using System.Linq;
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

        #region HelperMethods

        /// <summary>
        /// Helper to setup JSRuntime mock to return null theme
        /// </summary>
        private void SetupMockJSRuntimeWithNullTheme()
        {
            MockJSRuntime
                .Setup(x => x.InvokeAsync<string>("themeManager.getTheme", It.IsAny<object[]>()))
                .ReturnsAsync((string)null);

            MockJSRuntime
                .Setup(x => x.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>("themeManager.setTheme", It.IsAny<object[]>()))
                .Returns(new ValueTask<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(default(Microsoft.JSInterop.Infrastructure.IJSVoidResult)));
        }

        #endregion HelperMethods

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

        #region HandleToggleClick

        /// <summary>
        /// Test HandleToggleClick when IsToggling is true should return early
        /// </summary>
        [Test]
        public async Task HandleToggleClick_Invalid_IsToggling_True_Should_Return_Early()
        {
            // Arrange
            SetupMockJSRuntime("light");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Set IsToggling to true directly
            component.Instance.IsToggling = true;

            // Act
            await component.Instance.HandleToggleClick();

            // Assert - setTheme should NOT be called because IsToggling was true
            MockJSRuntime.Verify(
                x => x.InvokeAsync<Microsoft.JSInterop.Infrastructure.IJSVoidResult>(
                    "themeManager.setTheme",
                    It.IsAny<object[]>()),
                Times.Never);
        }

        /// <summary>
        /// Verifies that HandleToggleClick toggles the theme when IsToggling is explicitly set to false.
        /// Ensures that the JS runtime is invoked with "dark" theme and that the toggle logic is executed.
        /// </summary>
        [Test]
        public async Task HandleToggleClick_Valid_IsToggling_False_Should_Toggle_Theme()
        {
            // Arrange: Set initial theme to "light" via mock JS runtime
            SetupMockJSRuntime("light");

            // Render the ThemeToggle component
            var component = TestContext.Render<ThemeToggle>();

            // Wait until the light theme icon is present (fa-moon-o indicates light mode)
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Explicitly set IsToggling to false to allow theme toggle logic to proceed
            var prop = component.Instance.GetType().GetProperty("IsToggling");
            if (prop != null)
            {
                prop.SetValue(component.Instance, false);
            }

            // Act: Trigger the toggle click handler
            await component.InvokeAsync(() => component.Instance.HandleToggleClick());

            // Assert: Check if any invocation matches the expected theme toggle call
            var found = false;

            foreach (var invocation in MockJSRuntime.Invocations)
            {
                var methodIsInvokeAsync = invocation.Method.Name.Equals("InvokeAsync");

                var hasIdentifier = false;
                var identifier = invocation.Arguments.Count > 0 ? invocation.Arguments[0] as string : null;
                if (identifier != null)
                {
                    hasIdentifier = identifier.Equals("themeManager.setTheme");
                }

                var hasArgs = false;
                var args = invocation.Arguments.Count > 1 ? invocation.Arguments[1] as object[] : null;
                if (args != null && args.Length == 1)
                {
                    var theme = args[0]?.ToString();
                    if (theme != null)
                    {
                        hasArgs = theme.Equals("dark");
                    }
                }

                if (methodIsInvokeAsync)
                {
                    if (hasIdentifier)
                    {
                        if (hasArgs)
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }

            // Confirm that the theme toggle invocation exists
            Assert.That(found, Is.True);
        }




        #endregion HandleToggleClick

        #region GetThemeName

        /// <summary>
        /// Test GetThemeName returns dark when theme is dark
        /// </summary>
        [Test]
        public void GetThemeName_Valid_DarkTheme_Should_Return_Dark()
        {
            // Arrange
            SetupMockJSRuntime("dark");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-sun-o") != null);

            // Act
            var result = component.Instance.GetThemeName();

            // Assert
            Assert.That(result, Is.EqualTo("dark"));
        }

        /// <summary>
        /// Test GetThemeName returns light when theme is light
        /// </summary>
        [Test]
        public void GetThemeName_Valid_LightTheme_Should_Return_Light()
        {
            // Arrange
            SetupMockJSRuntime("light");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act
            var result = component.Instance.GetThemeName();

            // Assert
            Assert.That(result, Is.EqualTo("light"));
        }

        #endregion GetThemeName

        #region UpdateBrowserTheme

        /// <summary>
        /// Verifies that UpdateBrowserTheme invokes JavaScript with the correct theme name.
        /// Confirms that the JS runtime receives a call to set the theme to "dark".
        /// </summary>
        [Test]
        public async Task UpdateBrowserTheme_Valid_ThemeName_Should_Call_JavaScript()
        {
            // Arrange: Initialize mock JS runtime with "light" theme
            SetupMockJSRuntime("light");

            // Render the ThemeToggle component
            var component = TestContext.Render<ThemeToggle>();

            // Wait until the light theme icon is present (fa-moon-o indicates light mode)
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act: Call UpdateBrowserTheme with "dark"
            await component.Instance.UpdateBrowserTheme("dark");

            // Assert: Check if a matching JS invocation exists
            var found = false;

            foreach (var invocation in MockJSRuntime.Invocations)
            {
                // Confirm method name is "InvokeAsync"
                var methodIsCorrect = invocation.Method.Name.Equals("InvokeAsync");

                // Extract and validate the identifier argument
                var identifier = invocation.Arguments.Count > 0 ? invocation.Arguments[0] as string : null;
                var identifierIsCorrect = identifier != null && identifier.Equals("themeManager.setTheme");

                // Extract and validate the theme argument
                var args = invocation.Arguments.Count > 1 ? invocation.Arguments[1] as object[] : null;
                var themeIsCorrect = false;
                if (args != null && args.Length == 1)
                {
                    var theme = args[0]?.ToString();
                    if (theme != null)
                    {
                        themeIsCorrect = theme.Equals("dark");
                    }
                }

                // Mark as found if all conditions are satisfied
                if (methodIsCorrect)
                {
                    if (identifierIsCorrect)
                    {
                        if (themeIsCorrect)
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }

            // Confirm that the theme update was triggered via JS
            Assert.That(found, Is.True);
        }

        /// <summary>
        /// Verifies that UpdateBrowserTheme does not invoke JavaScript when the theme name is null.
        /// Confirms that no JS runtime call is made to "themeManager.setTheme".
        /// </summary>
        [Test]
        public async Task UpdateBrowserTheme_Invalid_Null_Should_Not_Call_JavaScript()
        {
            // Arrange: Initialize mock JS runtime with "light" theme
            SetupMockJSRuntime("light");

            // Render the ThemeToggle component
            var component = TestContext.Render<ThemeToggle>();

            // Wait until the light theme icon is present (fa-moon-o indicates light mode)
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act: Call UpdateBrowserTheme with null theme
            await component.Instance.UpdateBrowserTheme(null);

            // Assert: Verify that no matching JS invocation exists
            var found = false;

            foreach (var invocation in MockJSRuntime.Invocations)
            {
                // Confirm method name is "InvokeAsync"
                var methodIsCorrect = invocation.Method.Name.Equals("InvokeAsync");

                // Extract and validate the identifier argument
                var identifier = invocation.Arguments.Count > 0 ? invocation.Arguments[0] as string : null;
                var identifierIsCorrect = identifier != null && identifier.Equals("themeManager.setTheme");

                // If both method and identifier match, mark as found
                if (methodIsCorrect)
                {
                    if (identifierIsCorrect)
                    {
                        found = true;
                        break;
                    }
                }
            }

            // Confirm that no theme update was triggered via JS
            Assert.That(found, Is.False);
        }



        /// <summary>
        /// Test UpdateBrowserTheme with empty string does not call JavaScript
        /// </summary>
        [Test]
        public async Task UpdateBrowserTheme_Invalid_Empty_Should_Not_Call_JavaScript()
        {
            // Arrange
            SetupMockJSRuntime("light");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act
            await component.Instance.UpdateBrowserTheme(string.Empty);

            // Assert
            var setThemeCalls = MockJSRuntime.Invocations
                .Count(invocation =>
                    invocation.Method.Name == "InvokeAsync" &&
                    invocation.Arguments[0] is string identifier &&
                    identifier == "themeManager.setTheme");

            Assert.That(setThemeCalls, Is.EqualTo(0));
        }


        /// <summary>
        /// Test UpdateBrowserTheme with whitespace does not call JavaScript
        /// </summary>
        [Test]
        public async Task UpdateBrowserTheme_Invalid_Whitespace_Should_Not_Call_JavaScript()
        {
            // Arrange
            SetupMockJSRuntime("light");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act
            await component.Instance.UpdateBrowserTheme("   ");

            // Assert
            var setThemeCalls = MockJSRuntime.Invocations
                .Count(invocation =>
                    invocation.Method.Name == "InvokeAsync" &&
                    invocation.Arguments[0] is string identifier &&
                    identifier == "themeManager.setTheme");

            Assert.That(setThemeCalls, Is.EqualTo(0));
        }


        #endregion UpdateBrowserTheme

        #region LoadSavedTheme

        /// <summary>
        /// Test LoadSavedTheme returns saved theme from JavaScript
        /// </summary>
        [Test]
        public async Task LoadSavedTheme_Valid_JavaScript_Returns_Dark_Should_Return_Dark()
        {
            // Arrange
            SetupMockJSRuntime("dark");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-sun-o") != null);

            // Act
            var result = await component.Instance.LoadSavedTheme();

            // Assert
            Assert.That(result, Is.EqualTo("dark"));
        }

        /// <summary>
        /// Test LoadSavedTheme returns light when JavaScript returns null
        /// </summary>
        [Test]
        public async Task LoadSavedTheme_Invalid_JavaScript_Returns_Null_Should_Return_Light()
        {
            // Arrange
            SetupMockJSRuntimeWithNullTheme();
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("button") != null);

            // Act
            var result = await component.Instance.LoadSavedTheme();

            // Assert
            Assert.That(result, Is.EqualTo("light"));
        }

        #endregion LoadSavedTheme

        #region UpdateServiceTheme

        /// <summary>
        /// Test UpdateServiceTheme with dark sets dark theme
        /// </summary>
        [Test]
        public void UpdateServiceTheme_Valid_Dark_Should_Set_Dark_Theme()
        {
            // Arrange
            SetupMockJSRuntime("light");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act
            component.Instance.UpdateServiceTheme("dark");
            component.Render();

            // Assert
            var iconElement = component.Find("i.fa-sun-o");
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Test UpdateServiceTheme with light sets light theme
        /// </summary>
        [Test]
        public void UpdateServiceTheme_Valid_Light_Should_Set_Light_Theme()
        {
            // Arrange
            SetupMockJSRuntime("dark");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-sun-o") != null);

            // Act
            component.Instance.UpdateServiceTheme("light");
            component.Render();

            // Assert
            var iconElement = component.Find("i.fa-moon-o");
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Test UpdateServiceTheme with null does not change theme
        /// </summary>
        [Test]
        public void UpdateServiceTheme_Invalid_Null_Should_Not_Change_Theme()
        {
            // Arrange
            SetupMockJSRuntime("light");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act
            component.Instance.UpdateServiceTheme(null);
            component.Render();

            // Assert - Should still be light theme (moon icon)
            var iconElement = component.Find("i.fa-moon-o");
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Test UpdateServiceTheme with empty string does not change theme
        /// </summary>
        [Test]
        public void UpdateServiceTheme_Invalid_Empty_Should_Not_Change_Theme()
        {
            // Arrange
            SetupMockJSRuntime("light");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act
            component.Instance.UpdateServiceTheme(string.Empty);
            component.Render();

            // Assert
            var iconElement = component.Find("i.fa-moon-o");
            Assert.That(iconElement, Is.Not.Null);
        }

        /// <summary>
        /// Test UpdateServiceTheme with whitespace does not change theme
        /// </summary>
        [Test]
        public void UpdateServiceTheme_Invalid_Whitespace_Should_Not_Change_Theme()
        {
            // Arrange
            SetupMockJSRuntime("light");
            var component = TestContext.Render<ThemeToggle>();
            component.WaitForState(() => component.Find("i.fa-moon-o") != null);

            // Act
            component.Instance.UpdateServiceTheme("   ");
            component.Render();

            // Assert
            var iconElement = component.Find("i.fa-moon-o");
            Assert.That(iconElement, Is.Not.Null);
        }

        #endregion UpdateServiceTheme
    }
}