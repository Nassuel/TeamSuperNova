using System;

namespace ContosoCrafts.WebSite.Services
{

    /// <summary>
    /// Static utility class for highlighting search terms in text
    /// </summary>
    public static class StringHighlighter
    {

        /// <summary>
        /// Highlights the first occurrence of searchTerm in text using case-insensitive comparison
        /// </summary>
        /// <param name="text">Text to search within</param>
        /// <param name="searchTerm">Term to highlight</param>
        /// <returns>Text with highlighted search term wrapped in mark tags</returns>
        public static string HighlightMatch(string text, string searchTerm)
        {

            // Fast fail: Check if search term is null or whitespace
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return text;
            }

            // Fast fail: Check if text is null or whitespace
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            // Set comparison type to case-insensitive
            var comparison = StringComparison.OrdinalIgnoreCase;

            // Find index of search term in text
            int index = text.IndexOf(searchTerm, comparison);

            // Fast fail: Check if search term not found
            if (index < 0)
            {
                return text;
            }

            // Extract text before the match
            string before = text.Substring(0, index);

            // Extract the matching text
            string match = text.Substring(index, searchTerm.Length);

            // Extract text after the match
            string after = text.Substring(index + searchTerm.Length);

            // Return text with highlighted match
            return $"{before}<mark>{match}</mark>{after}";

        }

    }

}