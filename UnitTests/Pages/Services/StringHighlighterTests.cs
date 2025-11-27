using NUnit.Framework;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Services
{
    [TestFixture]
    public class StringHighlighterTests
    {
        [Test]
        public void HighlightMatch_ExactMatch_Should_Wrap_With_Mark()
        {
            // Arrange
            var data = new { Text = "Wonders of Thailand", Search = "Thai" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Wonders of <mark>Thai</mark>land"));
        }

        [Test]
        public void HighlightMatch_CaseInsensitive_Should_Wrap_With_Mark()
        {
            // Arrange
            var data = new { Text = "Wonders of THAILAND", Search = "thai" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Wonders of <mark>THAI</mark>LAND"));
        }

        [Test]
        public void HighlightMatch_NoMatch_Should_Return_Original()
        {
            // Arrange
            var data = new { Text = "Wonders of Thailand", Search = "Japan" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Wonders of Thailand"));
        }

        [Test]
        public void HighlightMatch_EmptySearchTerm_Should_Return_Original()
        {
            // Arrange
            var data = new { Text = "Wonders of Thailand", Search = "" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Wonders of Thailand"));
        }

        [Test]
        public void HighlightMatch_EmptyText_Should_Return_Empty()
        {
            // Arrange
            var data = new { Text = "", Search = "Thai" };

            // Act
            var result = StringHighlighter.HighlightMatch(data.Text, data.Search);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(""));
        }
    }
}