using NUnit.Framework;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Services
{
    /// <summary>
    /// Unit tests for StringHighlighter service
    /// Tests all methods to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class StringHighlighterTests
    {
        #region HighlightMatch

        /// <summary>
        /// Test HighlightMatch with exact match wraps text with mark tag
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_ExactMatch_Should_Wrap_With_Mark()
        {
            // Arrange
            var data = new { Text = "Gaming Laptop Pro", Search = "Laptop" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Gaming <mark>Laptop</mark> Pro"));
        }

        /// <summary>
        /// Test HighlightMatch with case insensitive match wraps text with mark tag
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_CaseInsensitive_Should_Wrap_With_Mark()
        {
            // Arrange
            var data = new { Text = "Mechanical KEYBOARD RGB", Search = "keyboard" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Mechanical <mark>KEYBOARD</mark> RGB"));
        }

        /// <summary>
        /// Test HighlightMatch with no match returns original text
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_NoMatch_Should_Return_Original()
        {
            // Arrange
            var data = new { Text = "Wireless Mouse", Search = "Monitor" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Wireless Mouse"));
        }

        /// <summary>
        /// Test HighlightMatch with empty search term returns original text
        /// </summary>
        [Test]
        public void HighlightMatch_Invalid_EmptySearchTerm_Should_Return_Original()
        {
            // Arrange
            var data = new { Text = "USB Headset", Search = "" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("USB Headset"));
        }

        /// <summary>
        /// Test HighlightMatch with empty text returns empty string
        /// </summary>
        [Test]
        public void HighlightMatch_Invalid_EmptyText_Should_Return_Empty()
        {
            // Arrange
            var data = new { Text = "", Search = "Gaming" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(""));
        }

        /// <summary>
        /// Test HighlightMatch with null text returns empty or handles gracefully
        /// </summary>
        [Test]
        public void HighlightMatch_Invalid_NullText_Should_Handle_Gracefully()
        {
            // Arrange
            var data = new { Text = (string)null, Search = "Test" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.Null.Or.Empty);
        }

        /// <summary>
        /// Test HighlightMatch with null search term returns original text
        /// </summary>
        [Test]
        public void HighlightMatch_Invalid_NullSearchTerm_Should_Return_Original()
        {
            // Arrange
            var data = new { Text = "3D Printer", Search = (string)null };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("3D Printer"));
        }

        /// <summary>
        /// Test HighlightMatch with whitespace search term returns original text
        /// </summary>
        [Test]
        public void HighlightMatch_Invalid_WhitespaceSearchTerm_Should_Return_Original()
        {
            // Arrange
            var data = new { Text = "VR Headset", Search = "   " };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("VR Headset"));
        }

        /// <summary>
        /// Test HighlightMatch with partial match at beginning of text
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_PartialMatchAtStart_Should_Wrap_With_Mark()
        {
            // Arrange
            var data = new { Text = "Bluetooth Speaker", Search = "Blue" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("<mark>Blue</mark>tooth Speaker"));
        }

        /// <summary>
        /// Test HighlightMatch with partial match at end of text
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_PartialMatchAtEnd_Should_Wrap_With_Mark()
        {
            // Arrange
            var data = new { Text = "Smart Watch", Search = "Watch" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Smart <mark>Watch</mark>"));
        }

        /// <summary>
        /// Test HighlightMatch with multiple occurrences highlights first match only
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_MultipleOccurrences_Should_Highlight_First()
        {
            // Arrange
            var data = new { Text = "Pro Gaming Pro Mouse", Search = "Pro" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("<mark>Pro</mark> Gaming Pro Mouse"));
        }

        /// <summary>
        /// Test HighlightMatch with single character search term
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_SingleCharacter_Should_Wrap_With_Mark()
        {
            // Arrange
            var data = new { Text = "USB Cable", Search = "U" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("<mark>U</mark>SB Cable"));
        }

        /// <summary>
        /// Test HighlightMatch with search term longer than text returns original
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_SearchLongerThanText_Should_Return_Original()
        {
            // Arrange
            var data = new { Text = "Mouse", Search = "Wireless Mouse Pad" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Mouse"));
        }

        /// <summary>
        /// Test HighlightMatch with special characters in text
        /// </summary>
        [Test]
        public void HighlightMatch_Valid_SpecialCharacters_Should_Wrap_With_Mark()
        {
            // Arrange
            var data = new { Text = "RGB LED Strip (5m)", Search = "LED" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("RGB <mark>LED</mark> Strip (5m)"));
        }

        #endregion HighlightMatch
    }
}