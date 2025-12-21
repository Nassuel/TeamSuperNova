using ContosoCrafts.WebSite.Enums;
using NUnit.Framework;

namespace UnitTests.Enums
{

    /// <summary>
    /// Unit tests for SearchFieldEnumExtensions DisplayName method
    /// Tests all enum values for correct display name output
    /// </summary>
    [TestFixture]
    public class SearchFieldEnumExtensionsTests
    {

        #region DisplayName_Brand

        /// <summary>
        /// Test DisplayName returns Brand for SearchFieldEnum.Brand
        /// </summary>
        [Test]
        public void DisplayName_Valid_Brand_Should_Return_Brand()
        {

            // Arrange
            var data = SearchFieldEnum.Brand;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Brand"));

        }

        #endregion DisplayName_Brand

        #region DisplayName_Description

        /// <summary>
        /// Test DisplayName returns Description for SearchFieldEnum.Description
        /// </summary>
        [Test]
        public void DisplayName_Valid_Description_Should_Return_Description()
        {

            // Arrange
            var data = SearchFieldEnum.Description;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Description"));

        }

        #endregion DisplayName_Description

        #region DisplayName_Type

        /// <summary>
        /// Test DisplayName returns Type for SearchFieldEnum.Type
        /// </summary>
        [Test]
        public void DisplayName_Valid_Type_Should_Return_Type()
        {

            // Arrange
            var data = SearchFieldEnum.Type;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Type"));

        }

        #endregion DisplayName_Type

        #region DisplayName_Undefined

        /// <summary>
        /// Test DisplayName returns Unknown for SearchFieldEnum.Undefined
        /// </summary>
        [Test]
        public void DisplayName_Valid_Undefined_Should_Return_Unknown()
        {

            // Arrange
            var data = SearchFieldEnum.Undefined;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Unknown"));

        }

        #endregion DisplayName_Undefined

        #region DisplayName_InvalidValue

        /// <summary>
        /// Test DisplayName returns Unknown for invalid enum value
        /// </summary>
        [Test]
        public void DisplayName_Invalid_Value_Should_Return_Unknown()
        {

            // Arrange
            var data = (SearchFieldEnum)999;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Unknown"));

        }

        #endregion DisplayName_InvalidValue

    }

}
