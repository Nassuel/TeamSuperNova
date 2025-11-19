using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;
using NUnit.Framework;

namespace UnitTests.Pages.Components
{
    /// <summary>
    /// Unit tests for the CharacterCountedTextarea component
    /// </summary>
    [TestFixture]
    public class CharacterCountedTextareaTests
    {
        // Test context for Bunit
        public BunitContext TestContext;

        /// <summary>
        /// Sets up test context before each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Create test context
            TestContext = new BunitContext();
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

        #region Render Tests

        /// <summary>
        /// Tests that component renders successfully
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_NotNull()
        {
            // Arrange
            // (No arrangement needed)

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that textarea element exists after render
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_TextareaElement()
        {
            // Arrange
            // (No arrangement needed)

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>();

            // Get textarea
            var textareaElement = result.Find("textarea");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(textareaElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that label is displayed with correct text
        /// </summary>
        [Test]
        public void Render_Valid_LabelSet_Should_Return_LabelWithText()
        {
            // Arrange
            var labelText = "Test Label";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Label, labelText));

            // Get label
            var labelElement = result.Find("label");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(labelElement.TextContent, Is.EqualTo(labelText));
        }

        /// <summary>
        /// Tests that placeholder is set correctly
        /// </summary>
        [Test]
        public void Render_Valid_PlaceholderSet_Should_Return_TextareaWithPlaceholder()
        {
            // Arrange
            var placeholderText = "Enter text here";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Placeholder, placeholderText));

            // Get textarea
            var textareaElement = result.Find("textarea");

            // Get placeholder attribute
            var placeholder = textareaElement.GetAttribute("placeholder");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(placeholder, Is.EqualTo(placeholderText));
        }

        /// <summary>
        /// Tests that maxlength attribute is set correctly
        /// </summary>
        [Test]
        public void Render_Valid_MaxLengthSet_Should_Return_TextareaWithMaxLength()
        {
            // Arrange
            var maxLength = 100;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.MaxLength, maxLength));

            // Get textarea
            var textareaElement = result.Find("textarea");

            // Get maxlength attribute
            var maxLengthAttr = textareaElement.GetAttribute("maxlength");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(maxLengthAttr, Is.EqualTo(maxLength.ToString()));
        }

        /// <summary>
        /// Tests that rows attribute is set correctly
        /// </summary>
        [Test]
        public void Render_Valid_RowsSet_Should_Return_TextareaWithRows()
        {
            // Arrange
            var rows = 5;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Rows, rows));

            // Get textarea
            var textareaElement = result.Find("textarea");

            // Get rows attribute
            var rowsAttr = textareaElement.GetAttribute("rows");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(rowsAttr, Is.EqualTo(rows.ToString()));
        }

        /// <summary>
        /// Tests that name attribute is set correctly
        /// </summary>
        [Test]
        public void Render_Valid_NameSet_Should_Return_TextareaWithName()
        {
            // Arrange
            var name = "testTextarea";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Name, name));

            // Get textarea
            var textareaElement = result.Find("textarea");

            // Get name attribute
            var nameAttr = textareaElement.GetAttribute("name");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(nameAttr, Is.EqualTo(name));
        }

        /// <summary>
        /// Tests that character count displays correctly with initial value
        /// </summary>
        [Test]
        public void Render_Valid_ValueSet_Should_Return_CharacterCount()
        {
            // Arrange
            var initialValue = "Hello";
            var maxLength = 100;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, initialValue)
                .Add(p => p.MaxLength, maxLength));

            // Get character count text
            var charCountElement = result.Find("small");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCountElement.TextContent, Does.Contain($"{initialValue.Length} / {maxLength}"));
        }

        /// <summary>
        /// Tests that default character count is 0 with no value
        /// </summary>
        [Test]
        public void Render_Valid_NoValue_Should_Return_ZeroCharacterCount()
        {
            // Arrange
            var maxLength = 100;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.MaxLength, maxLength));

            // Get character count text
            var charCountElement = result.Find("small");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCountElement.TextContent, Does.Contain($"0 / {maxLength}"));
        }

        /// <summary>
        /// Tests that textarea has form-control class
        /// </summary>
        [Test]
        public void Render_Valid_ComponentInitialized_Should_Return_FormControlClass()
        {
            // Arrange
            // (No arrangement needed)

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>();

            // Get textarea
            var textareaElement = result.Find("textarea.form-control");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(textareaElement, Is.Not.Null);
        }

        #endregion

        #region OnInitialized Tests

        /// <summary>
        /// Tests that OnInitialized sets InternalValue from Value parameter
        /// </summary>
        [Test]
        public void OnInitialized_Valid_ValueSet_Should_Return_InternalValueSet()
        {
            // Arrange
            var initialValue = "Test Value";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, initialValue));

            // Get component instance
            var instance = result.Instance;

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(instance.InternalValue, Is.EqualTo(initialValue));
        }

        /// <summary>
        /// Tests that OnInitialized handles null value correctly
        /// </summary>
        [Test]
        public void OnInitialized_InValid_NullValue_Should_Return_EmptyString()
        {
            // Arrange
            string nullValue = null;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, nullValue));

            // Get component instance
            var instance = result.Instance;

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(instance.InternalValue, Is.EqualTo(string.Empty));
        }

        #endregion

        #region OnParametersSet Tests

        /// <summary>
        /// Tests that OnParametersSet updates InternalValue when Value changes
        /// </summary>
        [Test]
        public void OnParametersSet_Valid_ValueChanged_Should_Return_UpdatedInternalValue()
        {
            // Arrange
            var initialValue = "Initial";
            var updatedValue = "Updated";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, initialValue));

            // Update parameter
            result.Render(parameters => parameters
                .Add(p => p.Value, updatedValue));

            // Get component instance
            var instance = result.Instance;

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(instance.InternalValue, Is.EqualTo(updatedValue));
        }

        /// <summary>
        /// Tests that OnParametersSet handles null value correctly
        /// </summary>
        [Test]
        public void OnParametersSet_InValid_NullValue_Should_Return_EmptyString()
        {
            // Arrange
            var initialValue = "Initial";
            string nullValue = null;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, initialValue));

            // Update parameter to null
            result.Render(parameters => parameters
                .Add(p => p.Value, nullValue));

            // Get component instance
            var instance = result.Instance;

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(instance.InternalValue, Is.EqualTo(string.Empty));
        }

        #endregion

        #region GetCharCount Tests

        /// <summary>
        /// Tests that GetCharCount returns correct count for non-empty string
        /// </summary>
        [Test]
        public void GetCharCount_Valid_NonEmptyString_Should_Return_CorrectCount()
        {
            // Arrange
            var testValue = "Hello World";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue));

            // Get component instance
            var instance = result.Instance;

            // Get character count
            var charCount = instance.GetCharCount();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCount, Is.EqualTo(testValue.Length));
        }

        /// <summary>
        /// Tests that GetCharCount returns 0 for empty string
        /// </summary>
        [Test]
        public void GetCharCount_Valid_EmptyString_Should_Return_Zero()
        {
            // Arrange
            var emptyValue = string.Empty;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, emptyValue));

            // Get component instance
            var instance = result.Instance;

            // Get character count
            var charCount = instance.GetCharCount();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCount, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that GetCharCount returns 0 for null string
        /// </summary>
        [Test]
        public void GetCharCount_InValid_NullString_Should_Return_Zero()
        {
            // Arrange
            // Create component with null value
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, (string)null));

            // Act
            var instance = result.Instance;

            // Get character count
            var charCount = instance.GetCharCount();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCount, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that GetCharCount handles maximum length correctly
        /// </summary>
        [Test]
        public void GetCharCount_Valid_MaxLengthString_Should_Return_MaxLength()
        {
            // Arrange
            var maxLength = 50;
            var testValue = new string('A', maxLength);

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue)
                .Add(p => p.MaxLength, maxLength));

            // Get component instance
            var instance = result.Instance;

            // Get character count
            var charCount = instance.GetCharCount();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCount, Is.EqualTo(maxLength));
        }

        #endregion

        #region GetCharCountClass Tests

        /// <summary>
        /// Tests that GetCharCountClass returns text-muted when under limit
        /// </summary>
        [Test]
        public void GetCharCountClass_Valid_UnderLimit_Should_Return_TextMuted()
        {
            // Arrange
            var testValue = "Hello";
            var maxLength = 100;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue)
                .Add(p => p.MaxLength, maxLength));

            // Get component instance
            var instance = result.Instance;

            // Get CSS class
            var cssClass = instance.GetCharCountClass();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(cssClass, Is.EqualTo("text-muted"));
        }

        /// <summary>
        /// Tests that GetCharCountClass returns text-danger when at limit
        /// </summary>
        [Test]
        public void GetCharCountClass_Valid_AtLimit_Should_Return_TextDanger()
        {
            // Arrange
            var maxLength = 10;
            var testValue = new string('A', maxLength);

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue)
                .Add(p => p.MaxLength, maxLength));

            // Get component instance
            var instance = result.Instance;

            // Get CSS class
            var cssClass = instance.GetCharCountClass();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(cssClass, Is.EqualTo("text-danger"));
        }

        /// <summary>
        /// Tests that GetCharCountClass returns text-danger when exceeding limit
        /// </summary>
        [Test]
        public void GetCharCountClass_Valid_ExceedingLimit_Should_Return_TextDanger()
        {
            // Arrange
            var maxLength = 10;
            var testValue = new string('A', maxLength + 5);

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue)
                .Add(p => p.MaxLength, maxLength));

            // Get component instance
            var instance = result.Instance;

            // Get CSS class
            var cssClass = instance.GetCharCountClass();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(cssClass, Is.EqualTo("text-danger"));
        }

        /// <summary>
        /// Tests that GetCharCountClass returns text-muted for empty value
        /// </summary>
        [Test]
        public void GetCharCountClass_Valid_EmptyValue_Should_Return_TextMuted()
        {
            // Arrange
            var emptyValue = string.Empty;
            var maxLength = 100;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, emptyValue)
                .Add(p => p.MaxLength, maxLength));

            // Get component instance
            var instance = result.Instance;

            // Get CSS class
            var cssClass = instance.GetCharCountClass();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(cssClass, Is.EqualTo("text-muted"));
        }

        /// <summary>
        /// Tests that GetCharCountClass returns text-muted one character before limit
        /// </summary>
        [Test]
        public void GetCharCountClass_Valid_OneBeforeLimit_Should_Return_TextMuted()
        {
            // Arrange
            var maxLength = 10;
            var testValue = new string('A', maxLength - 1);

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue)
                .Add(p => p.MaxLength, maxLength));

            // Get component instance
            var instance = result.Instance;

            // Get CSS class
            var cssClass = instance.GetCharCountClass();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(cssClass, Is.EqualTo("text-muted"));
        }

        #endregion

        #region Character Count Display Tests

        /// <summary>
        /// Tests that character count display has text-muted class when under limit
        /// </summary>
        [Test]
        public void Render_Valid_UnderLimit_Should_Return_TextMutedClass()
        {
            // Arrange
            var testValue = "Hello";
            var maxLength = 100;

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue)
                .Add(p => p.MaxLength, maxLength));

            // Get character count element
            var charCountElement = result.Find("small.text-muted");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCountElement, Is.Not.Null);
        }

        /// <summary>
        /// Tests that character count display has text-danger class when at limit
        /// </summary>
        [Test]
        public void Render_Valid_AtLimit_Should_Return_TextDangerClass()
        {
            // Arrange
            var maxLength = 10;
            var testValue = new string('A', maxLength);

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue)
                .Add(p => p.MaxLength, maxLength));

            // Get character count element
            var charCountElement = result.Find("small.text-danger");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCountElement, Is.Not.Null);
        }

        #endregion

        #region Default Values Tests

        /// <summary>
        /// Tests that default MaxLength is 5000
        /// </summary>
        [Test]
        public void Render_Valid_DefaultMaxLength_Should_Return_5000()
        {
            // Arrange
            // (No arrangement needed)

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>();

            // Get textarea
            var textareaElement = result.Find("textarea");

            // Get maxlength attribute
            var maxLengthAttr = textareaElement.GetAttribute("maxlength");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(maxLengthAttr, Is.EqualTo("5000"));
        }

        /// <summary>
        /// Tests that default Rows is 10
        /// </summary>
        [Test]
        public void Render_Valid_DefaultRows_Should_Return_10()
        {
            // Arrange
            // (No arrangement needed)

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>();

            // Get textarea
            var textareaElement = result.Find("textarea");

            // Get rows attribute
            var rowsAttr = textareaElement.GetAttribute("rows");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(rowsAttr, Is.EqualTo("10"));
        }

        /// <summary>
        /// Tests that default Label is empty string
        /// </summary>
        [Test]
        public void Render_Valid_DefaultLabel_Should_Return_EmptyString()
        {
            // Arrange
            // (No arrangement needed)

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>();

            // Get label
            var labelElement = result.Find("label");

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(labelElement.TextContent, Is.EqualTo(string.Empty));
        }

        #endregion

        #region Edge Case Tests

        /// <summary>
        /// Tests that component handles very long text correctly
        /// </summary>
        [Test]
        public void Render_Valid_VeryLongText_Should_Return_CorrectCount()
        {
            // Arrange
            var maxLength = 10000;
            var testValue = new string('A', 9999);

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue)
                .Add(p => p.MaxLength, maxLength));

            // Get component instance
            var instance = result.Instance;

            // Get character count
            var charCount = instance.GetCharCount();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCount, Is.EqualTo(9999));
        }

        /// <summary>
        /// Tests that component handles special characters correctly
        /// </summary>
        [Test]
        public void GetCharCount_Valid_SpecialCharacters_Should_Return_CorrectCount()
        {
            // Arrange
            var testValue = "Hello @#$%^&*()_+-={}[]|:;<>?,./~`";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue));

            // Get component instance
            var instance = result.Instance;

            // Get character count
            var charCount = instance.GetCharCount();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCount, Is.EqualTo(testValue.Length));
        }

        /// <summary>
        /// Tests that component handles newlines correctly
        /// </summary>
        [Test]
        public void GetCharCount_Valid_Newlines_Should_Return_CorrectCount()
        {
            // Arrange
            var testValue = "Line 1\nLine 2\nLine 3";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue));

            // Get component instance
            var instance = result.Instance;

            // Get character count
            var charCount = instance.GetCharCount();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCount, Is.EqualTo(testValue.Length));
        }

        /// <summary>
        /// Tests that component handles Unicode characters correctly
        /// </summary>
        [Test]
        public void GetCharCount_Valid_UnicodeCharacters_Should_Return_CorrectCount()
        {
            // Arrange
            var testValue = "Hello 世界 🌍";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, testValue));

            // Get component instance
            var instance = result.Instance;

            // Get character count
            var charCount = instance.GetCharCount();

            // Reset
            // (No reset needed)

            // Assert
            Assert.That(charCount, Is.EqualTo(testValue.Length));
        }

        #endregion

        #region New ValueChanged Tests

        /// <summary>
        /// Tests that ValueChanged callback is invoked when textarea input changes
        /// </summary>
        [Test]
        public void ValueChanged_Should_Be_Invoked_On_Textarea_Input()
        {
            // Arrange
            bool called = false;
            string passedValue = null;

            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, string.Empty)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(this, (string s) => { called = true; passedValue = s; })));

            var textarea = result.Find("textarea");

            // Act - simulate user input (bind:event=oninput)
            textarea.Input("Test Input");

            // Assert
            Assert.That(called, Is.True);
            Assert.That(passedValue, Is.EqualTo("Test Input"));
            Assert.That(result.Instance.InternalValue, Is.EqualTo("Test Input"));
        }

        /// <summary>
        /// Tests that ValueChanged callback is invoked during initialization when Value is null
        /// </summary>
        [Test]
        public void ValueChanged_Should_Be_Invoked_On_Initialize_When_Value_Null()
        {
            // Arrange
            bool called = false;
            string passedValue = "not-set";

            // Act
            var result = TestContext.Render<CharacterCountedTextarea>(parameters => parameters
                .Add(p => p.Value, (string)null)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<string>(this, (string s) => { called = true; passedValue = s; })));

            // Assert
            Assert.That(called, Is.True);
            Assert.That(passedValue, Is.EqualTo(string.Empty));
            Assert.That(result.Instance.InternalValue, Is.EqualTo(string.Empty));
        }

        #endregion
    }
}