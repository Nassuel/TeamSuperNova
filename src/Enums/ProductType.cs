namespace ContosoCrafts.WebSite.Enums
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

            // Fast fail: switch on product type to return display names
            switch (data)
            {
                case ProductTypeEnum.Undefined:
                    return "Undefined";

                case ProductTypeEnum.Laptop:
                    return "Laptop";

                case ProductTypeEnum.Keyboard:
                    return "Keyboard";

                case ProductTypeEnum.Mice:
                    return "Mice";

                case ProductTypeEnum.Headset:
                    return "Headset";

                case ProductTypeEnum.VrHeadsets:
                    return "VR Headsets";

                case ProductTypeEnum.Printer3D:
                    return "3D Printer";

                // Return empty string for unknown product types
                default:
                    return "";
            }

        }

    }

}