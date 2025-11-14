namespace ContosoCrafts.WebSite.Models
{

    /// <summary>
    /// Enum representing different types of products available
    /// </summary>
    public enum ProductTypeEnum
    {

        // Reserved for null or undefined cases
        Undefined = 0,

        // Laptop computer product type
        Laptop = 5,

        // Keyboard peripheral product type
        Keyboard = 7,

        // Mice peripheral product type
        Mice = 11,

        // Headset audio product type
        Headset = 15,

        // Virtual reality headset product type
        VrHeadsets = 17,

        // 3D printer product type
        Printer3D = 20,

    }

    /// <summary>
    /// Extension methods for ProductTypeEnum to provide display functionality
    /// </summary>
    public static class ProductTypeEnumExtensions
    {

        /// <summary>
        /// Converts ProductTypeEnum value to a human-readable display name
        /// </summary>
        /// <param name="data">The ProductTypeEnum value to convert</param>
        /// <returns>Display name string for the product type</returns>
        public static string DisplayName(this ProductTypeEnum data)
        {

            // Fast fail: Check if data is Undefined
            if (data == ProductTypeEnum.Undefined)
            {
                return "Undefined";
            }

            // Fast fail: Check if data is Laptop
            if (data == ProductTypeEnum.Laptop)
            {
                return "Laptop";
            }

            // Fast fail: Check if data is Keyboard
            if (data == ProductTypeEnum.Keyboard)
            {
                return "Keyboard";
            }

            // Fast fail: Check if data is Mice
            if (data == ProductTypeEnum.Mice)
            {
                return "Mice";
            }

            // Fast fail: Check if data is Headset
            if (data == ProductTypeEnum.Headset)
            {
                return "Headset";
            }

            // Fast fail: Check if data is VrHeadsets
            if (data == ProductTypeEnum.VrHeadsets)
            {
                return "VR Headsets";
            }

            // Fast fail: Check if data is Printer3D
            if (data == ProductTypeEnum.Printer3D)
            {
                return "3D Printer";
            }

            // Return empty string for unknown product types
            return "";

        }

    }

}