using System.Net.NetworkInformation;

namespace ContosoCrafts.WebSite.Models
{
    public enum ProductTypeEnum
    {
        Undefined = 0, // Reserved for null cases
        Laptop = 5,
        Keyboard = 7,
        Mice = 11,
        Headset = 15,
        VrHeadsets = 17,
        Printer3D = 20,
    }

    public static class ProductTypeEnumExtensions
    {
        public static string DisplayName(this ProductTypeEnum data)
        {
            return data switch
            {
                ProductTypeEnum.Undefined => "Undefined",
                ProductTypeEnum.Laptop => "Laptop",
                ProductTypeEnum.Keyboard => "Keyboard",
                ProductTypeEnum.Mice => "Mice",
                ProductTypeEnum.Headset => "Headset",
                ProductTypeEnum.VrHeadsets => "VR Headsets",
                ProductTypeEnum.Printer3D => "Printer3D",

                // Default, Unknown
                _ => "",
            };
        }
    }
}
