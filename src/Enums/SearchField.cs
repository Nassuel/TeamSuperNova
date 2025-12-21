namespace ContosoCrafts.WebSite.Enums
{

    /// <summary>
    /// Enum representing the available fields for product search functionality
    /// </summary>
    public enum SearchFieldEnum
    {

        // Default undefined state
        Undefined = 0,

        // Search by product brand name
        Brand = 10,

        // Search by product description text
        Description = 20,

        // Search by product type category
        Type = 30

    }

    /// <summary>
    /// Extension methods for SearchFieldEnum to provide display-friendly names
    /// </summary>
    public static class SearchFieldEnumExtensions
    {

        /// <summary>
        /// Returns a human-readable display name for the search field enum value
        /// </summary>
        /// <param name="searchField">The search field enum value</param>
        /// <returns>Display-friendly string representation of the search field</returns>
        public static string DisplayName(this SearchFieldEnum data)
        {

            // Fast fail: switch on search field to return display names
            switch (data)
            {
                case SearchFieldEnum.Brand:
                    return "Brand";

                case SearchFieldEnum.Description:
                    return "Description";

                case SearchFieldEnum.Type:
                    return "Type";

                // Default for Undefined or unrecognized values
                default:
                    return "Unknown";
            }

        }

    }

}
