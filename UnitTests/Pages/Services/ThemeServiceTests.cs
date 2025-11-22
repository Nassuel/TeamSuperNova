using ContosoCrafts.WebSite.Enums;
using ContosoCrafts.WebSite.Services;
using NUnit.Framework;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for ThemeService class
    /// Tests all methods and properties to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class ThemeServiceTests
    {
        #region TestSetup

        // Service instance for testing
        private ThemeService themeService;

        /// <summary>
        /// Initialize test environment before each test
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            themeService = new ThemeService();
        }

        #endregion TestSetup

        #region Constructor

        /// <summary>
        /// Test constructor creates valid instance
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Create_Instance()
        {
            // Arrange
            var data = new ThemeService();

            // Act
            var result = data;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Test constructor initializes with Light theme
        /// </summary>
        [Test]
        public void Constructor_Valid_Should_Initialize_With_Light_Theme()
        {
            // Arrange
            var data = new ThemeService();

            // Act
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Light));
        }

        #endregion Constructor

        #region GetTheme

        /// <summary>
        /// Test GetTheme returns Light theme by default
        /// </summary>
        [Test]
        public void GetTheme_Valid_Default_Should_Return_Light()
        {
            // Arrange
            var data = themeService;

            // Act
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Light));
        }

        /// <summary>
        /// Test GetTheme returns Dark after setting Dark theme
        /// </summary>
        [Test]
        public void GetTheme_Valid_After_SetDark_Should_Return_Dark()
        {
            // Arrange
            var data = themeService;
            data.SetTheme(ThemeMode.Dark);

            // Act
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Dark));
        }

        /// <summary>
        /// Test GetTheme returns Light after setting Light theme
        /// </summary>
        [Test]
        public void GetTheme_Valid_After_SetLight_Should_Return_Light()
        {
            // Arrange
            var data = themeService;
            data.SetTheme(ThemeMode.Dark);
            data.SetTheme(ThemeMode.Light);

            // Act
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Light));
        }

        #endregion GetTheme

        #region SetTheme

        /// <summary>
        /// Test SetTheme with Light sets theme to Light
        /// </summary>
        [Test]
        public void SetTheme_Valid_Light_Should_Set_Theme_To_Light()
        {
            // Arrange
            var data = themeService;

            // Act
            data.SetTheme(ThemeMode.Light);
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Light));
        }

        /// <summary>
        /// Test SetTheme with Dark sets theme to Dark
        /// </summary>
        [Test]
        public void SetTheme_Valid_Dark_Should_Set_Theme_To_Dark()
        {
            // Arrange
            var data = themeService;

            // Act
            data.SetTheme(ThemeMode.Dark);
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Dark));
        }

        /// <summary>
        /// Test SetTheme with Undefined does not change theme
        /// </summary>
        [Test]
        public void SetTheme_Invalid_Undefined_Should_Not_Change_Theme()
        {
            // Arrange
            var data = themeService;
            var originalTheme = data.GetTheme();

            // Act
            data.SetTheme(ThemeMode.Undefined);
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(originalTheme));
        }

        /// <summary>
        /// Test SetTheme with Undefined keeps Dark theme unchanged
        /// </summary>
        [Test]
        public void SetTheme_Invalid_Undefined_After_Dark_Should_Keep_Dark()
        {
            // Arrange
            var data = themeService;
            data.SetTheme(ThemeMode.Dark);

            // Act
            data.SetTheme(ThemeMode.Undefined);
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Dark));
        }

        /// <summary>
        /// Test SetTheme triggers OnThemeChanged event
        /// </summary>
        [Test]
        public void SetTheme_Valid_Dark_Should_Trigger_OnThemeChanged_Event()
        {
            // Arrange
            var data = themeService;
            var eventTriggered = false;
            data.OnThemeChanged += () => eventTriggered = true;

            // Act
            data.SetTheme(ThemeMode.Dark);
            var result = eventTriggered;

            // Reset

            // Assert
            Assert.That(result, Is.True);
        }

        #endregion SetTheme

        #region ToggleTheme

        /// <summary>
        /// Test ToggleTheme from Light switches to Dark
        /// </summary>
        [Test]
        public void ToggleTheme_Valid_From_Light_Should_Switch_To_Dark()
        {
            // Arrange
            var data = themeService;

            // Act
            data.ToggleTheme();
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Dark));
        }

        /// <summary>
        /// Test ToggleTheme from Dark switches to Light
        /// </summary>
        [Test]
        public void ToggleTheme_Valid_From_Dark_Should_Switch_To_Light()
        {
            // Arrange
            var data = themeService;
            data.SetTheme(ThemeMode.Dark);

            // Act
            data.ToggleTheme();
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Light));
        }

        /// <summary>
        /// Test ToggleTheme twice returns to original theme
        /// </summary>
        [Test]
        public void ToggleTheme_Valid_Twice_Should_Return_To_Original()
        {
            // Arrange
            var data = themeService;
            var originalTheme = data.GetTheme();

            // Act
            data.ToggleTheme();
            data.ToggleTheme();
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(originalTheme));
        }

        /// <summary>
        /// Test ToggleTheme from Light triggers event
        /// </summary>
        [Test]
        public void ToggleTheme_Valid_From_Light_Should_Trigger_Event()
        {
            // Arrange
            var data = themeService;
            var eventTriggered = false;
            data.OnThemeChanged += () => eventTriggered = true;

            // Act
            data.ToggleTheme();
            var result = eventTriggered;

            // Reset

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Test ToggleTheme from Dark triggers event
        /// </summary>
        [Test]
        public void ToggleTheme_Valid_From_Dark_Should_Trigger_Event()
        {
            // Arrange
            var data = themeService;
            data.SetTheme(ThemeMode.Dark);
            var eventTriggered = false;
            data.OnThemeChanged += () => eventTriggered = true;

            // Act
            data.ToggleTheme();
            var result = eventTriggered;

            // Reset

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Test ToggleTheme multiple times triggers event each time
        /// </summary>
        [Test]
        public void ToggleTheme_Valid_Multiple_Times_Should_Trigger_Event_Each_Time()
        {
            // Arrange
            var data = themeService;
            var eventCount = 0;
            data.OnThemeChanged += () => eventCount++;

            // Act
            data.ToggleTheme();
            data.ToggleTheme();
            data.ToggleTheme();
            var result = eventCount;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(3));
        }

        #endregion ToggleTheme

        #region OnThemeChanged

        /// <summary>
        /// Test SetTheme without subscribers does not throw exception
        /// </summary>
        [Test]
        public void SetTheme_Valid_No_Subscribers_Should_Not_Throw()
        {
            // Arrange
            var data = new ThemeService();

            // Act & Assert
            Assert.DoesNotThrow(() => data.SetTheme(ThemeMode.Dark));
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Dark));
        }

        /// <summary>
        /// Test ToggleTheme without subscribers does not throw exception
        /// </summary>
        [Test]
        public void ToggleTheme_Valid_No_Subscribers_Should_Not_Throw()
        {
            // Arrange
            var data = new ThemeService();

            // Act & Assert
            Assert.DoesNotThrow(() => data.ToggleTheme());
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Dark));
        }

        /// <summary>
        /// Test multiple subscribers all receive event
        /// </summary>
        [Test]
        public void OnThemeChanged_Valid_Multiple_Subscribers_Should_All_Receive_Event()
        {
            // Arrange
            var data = themeService;
            var subscriber1Called = false;
            var subscriber2Called = false;
            var subscriber3Called = false;
            data.OnThemeChanged += () => subscriber1Called = true;
            data.OnThemeChanged += () => subscriber2Called = true;
            data.OnThemeChanged += () => subscriber3Called = true;

            // Act
            data.ToggleTheme();
            var result = subscriber1Called && subscriber2Called && subscriber3Called;

            // Reset

            // Assert
            Assert.That(result, Is.True);
        }

        #endregion OnThemeChanged

        #region NotifyThemeChanged

        /// <summary>
        /// Test NotifyThemeChanged is called when SetTheme changes theme
        /// </summary>
        [Test]
        public void NotifyThemeChanged_Valid_SetTheme_Should_Invoke_Event()
        {
            // Arrange
            var data = themeService;
            var invocationCount = 0;
            data.OnThemeChanged += () => invocationCount++;

            // Act
            data.SetTheme(ThemeMode.Dark);
            data.SetTheme(ThemeMode.Light);
            var result = invocationCount;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        /// <summary>
        /// Test NotifyThemeChanged not called when event is null
        /// </summary>
        [Test]
        public void NotifyThemeChanged_Valid_Null_Event_Should_Not_Throw()
        {
            // Arrange
            var data = new ThemeService();

            // Act
            data.SetTheme(ThemeMode.Dark);
            var result = data.GetTheme();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ThemeMode.Dark));
        }

        #endregion NotifyThemeChanged
    }
}