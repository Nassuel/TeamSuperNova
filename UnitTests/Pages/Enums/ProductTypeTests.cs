using ContosoCrafts.WebSite.Enums;
using NUnit.Framework;
using System;
using System.Linq;

namespace UnitTests.Pages.Enums
{
    /// <summary>
    /// Unit tests for ProductTypeEnum and ProductTypeEnumExtensions
    /// Tests all enum values and extension methods to achieve 100% code coverage
    /// </summary>
    [TestFixture]
    public class ProductTypeEnumTests
    {
        #region EnumValues

        /// <summary>
        /// Test Undefined enum value equals 0
        /// </summary>
        [Test]
        public void ProductTypeEnum_Valid_Undefined_Should_Equal_0()
        {
            // Arrange
            var data = ProductTypeEnum.Undefined;

            // Act
            var result = (int)data;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        /// <summary>
        /// Test Laptop enum value equals 5
        /// </summary>
        [Test]
        public void ProductTypeEnum_Valid_Laptop_Should_Equal_5()
        {
            // Arrange
            var data = ProductTypeEnum.Laptop;

            // Act
            var result = (int)data;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(5));
        }

        /// <summary>
        /// Test Keyboard enum value equals 7
        /// </summary>
        [Test]
        public void ProductTypeEnum_Valid_Keyboard_Should_Equal_7()
        {
            // Arrange
            var data = ProductTypeEnum.Keyboard;

            // Act
            var result = (int)data;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(7));
        }

        /// <summary>
        /// Test Mice enum value equals 11
        /// </summary>
        [Test]
        public void ProductTypeEnum_Valid_Mice_Should_Equal_11()
        {
            // Arrange
            var data = ProductTypeEnum.Mice;

            // Act
            var result = (int)data;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(11));
        }

        /// <summary>
        /// Test Headset enum value equals 15
        /// </summary>
        [Test]
        public void ProductTypeEnum_Valid_Headset_Should_Equal_15()
        {
            // Arrange
            var data = ProductTypeEnum.Headset;

            // Act
            var result = (int)data;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(15));
        }

        /// <summary>
        /// Test VrHeadsets enum value equals 17
        /// </summary>
        [Test]
        public void ProductTypeEnum_Valid_VrHeadsets_Should_Equal_17()
        {
            // Arrange
            var data = ProductTypeEnum.VrHeadsets;

            // Act
            var result = (int)data;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(17));
        }

        /// <summary>
        /// Test Printer3D enum value equals 20
        /// </summary>
        [Test]
        public void ProductTypeEnum_Valid_Printer3D_Should_Equal_20()
        {
            // Arrange
            var data = ProductTypeEnum.Printer3D;

            // Act
            var result = (int)data;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(20));
        }

        /// <summary>
        /// Test enum contains expected number of values
        /// </summary>
        [Test]
        public void ProductTypeEnum_Valid_Count_Should_Equal_7()
        {
            // Arrange
            var data = Enum.GetValues(typeof(ProductTypeEnum));

            // Act
            var result = data.Length;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(7));
        }

        #endregion EnumValues

        #region DisplayName

        /// <summary>
        /// Test DisplayName for Undefined returns "Undefined"
        /// </summary>
        [Test]
        public void DisplayName_Valid_Undefined_Should_Return_Undefined_String()
        {
            // Arrange
            var data = ProductTypeEnum.Undefined;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Undefined"));
        }

        /// <summary>
        /// Test DisplayName for Laptop returns "Laptop"
        /// </summary>
        [Test]
        public void DisplayName_Valid_Laptop_Should_Return_Laptop_String()
        {
            // Arrange
            var data = ProductTypeEnum.Laptop;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Laptop"));
        }

        /// <summary>
        /// Test DisplayName for Keyboard returns "Keyboard"
        /// </summary>
        [Test]
        public void DisplayName_Valid_Keyboard_Should_Return_Keyboard_String()
        {
            // Arrange
            var data = ProductTypeEnum.Keyboard;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Keyboard"));
        }

        /// <summary>
        /// Test DisplayName for Mice returns "Mice"
        /// </summary>
        [Test]
        public void DisplayName_Valid_Mice_Should_Return_Mice_String()
        {
            // Arrange
            var data = ProductTypeEnum.Mice;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Mice"));
        }

        /// <summary>
        /// Test DisplayName for Headset returns "Headset"
        /// </summary>
        [Test]
        public void DisplayName_Valid_Headset_Should_Return_Headset_String()
        {
            // Arrange
            var data = ProductTypeEnum.Headset;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("Headset"));
        }

        /// <summary>
        /// Test DisplayName for VrHeadsets returns "VR Headsets"
        /// </summary>
        [Test]
        public void DisplayName_Valid_VrHeadsets_Should_Return_VR_Headsets_String()
        {
            // Arrange
            var data = ProductTypeEnum.VrHeadsets;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("VR Headsets"));
        }

        /// <summary>
        /// Test DisplayName for Printer3D returns "3D Printer"
        /// </summary>
        [Test]
        public void DisplayName_Valid_Printer3D_Should_Return_3D_Printer_String()
        {
            // Arrange
            var data = ProductTypeEnum.Printer3D;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo("3D Printer"));
        }

        /// <summary>
        /// Test DisplayName for unknown positive enum value returns empty string
        /// </summary>
        [Test]
        public void DisplayName_Invalid_Unknown_Positive_Value_Should_Return_Empty_String()
        {
            // Arrange
            var data = (ProductTypeEnum)999;

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// Test DisplayName for unknown negative enum value returns empty string
        /// </summary>
        [Test]
        public void DisplayName_Invalid_Unknown_Negative_Value_Should_Return_Empty_String()
        {
            // Arrange
            var data = (ProductTypeEnum)(-1);

            // Act
            var result = data.DisplayName();

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        #endregion DisplayName

        #region DisplayNameNotNull

        /// <summary>
        /// Test DisplayName never returns null for any defined value
        /// </summary>
        [Test]
        public void DisplayName_Valid_All_Defined_Values_Should_Return_Not_Null()
        {
            // Arrange
            var data = Enum.GetValues(typeof(ProductTypeEnum)).Cast<ProductTypeEnum>();

            // Act & Assert
            foreach (var value in data)
            {
                // Act
                var result = value.DisplayName();

                // Reset

                // Assert
                Assert.That(result, Is.Not.Null);
            }
        }

        /// <summary>
        /// Test DisplayName returns non-empty string for all defined values
        /// </summary>
        [Test]
        public void DisplayName_Valid_All_Defined_Values_Should_Return_Non_Empty_String()
        {
            // Arrange
            var data = Enum.GetValues(typeof(ProductTypeEnum)).Cast<ProductTypeEnum>();

            // Act & Assert
            foreach (var value in data)
            {
                // Act
                var result = value.DisplayName();

                // Reset

                // Assert
                Assert.That(result, Is.Not.Empty);
            }
        }

        #endregion DisplayNameNotNull

        #region EnumParsing

        /// <summary>
        /// Test parsing string "Laptop" returns Laptop enum
        /// </summary>
        [Test]
        public void Parse_Valid_Laptop_String_Should_Return_Laptop_Enum()
        {
            // Arrange
            var data = "Laptop";

            // Act
            var result = Enum.Parse<ProductTypeEnum>(data);

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ProductTypeEnum.Laptop));
        }

        /// <summary>
        /// Test parsing integer 5 returns Laptop enum
        /// </summary>
        [Test]
        public void Cast_Valid_Integer_5_Should_Return_Laptop_Enum()
        {
            // Arrange
            var data = 5;

            // Act
            var result = (ProductTypeEnum)data;

            // Reset

            // Assert
            Assert.That(result, Is.EqualTo(ProductTypeEnum.Laptop));
        }

        /// <summary>
        /// Test IsDefined returns true for valid values
        /// </summary>
        [Test]
        public void IsDefined_Valid_Keyboard_Should_Return_True()
        {
            // Arrange
            var data = ProductTypeEnum.Keyboard;

            // Act
            var result = Enum.IsDefined(typeof(ProductTypeEnum), data);

            // Reset

            // Assert
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Test IsDefined returns false for invalid values
        /// </summary>
        [Test]
        public void IsDefined_Invalid_Value_999_Should_Return_False()
        {
            // Arrange
            var data = (ProductTypeEnum)999;

            // Act
            var result = Enum.IsDefined(typeof(ProductTypeEnum), data);

            // Reset

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion EnumParsing
    }
}
