using ContosoCrafts.WebSite.Models;
using System;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Service for managing application theme state and persistence
    /// </summary>
    public class ThemeService
    {
        // Current theme mode
        private ThemeMode CurrentTheme;

        // Event for theme changes
        public event Action? OnThemeChanged;

        /// <summary>
        /// Initializes a new instance of the ThemeService class
        /// </summary>
        public ThemeService()
        {
            // Default to light theme
            CurrentTheme = ThemeMode.Light;
        }

        /// <summary>
        /// Gets the current theme mode
        /// </summary>
        /// <returns>The current ThemeMode</returns>
        public ThemeMode GetTheme()
        {
            return CurrentTheme;
        }

        /// <summary>
        /// Sets the theme mode
        /// </summary>
        /// <param name="themeMode">The theme mode to set</param>
        public void SetTheme(ThemeMode themeMode)
        {
            // Defensive programming: validate input
            if (themeMode == ThemeMode.Undefined)
            {
                return;
            }

            CurrentTheme = themeMode;

            // Notify subscribers
            NotifyThemeChanged();
        }

        /// <summary>
        /// Toggles between light and dark theme
        /// </summary>
        public void ToggleTheme()
        {
            // Check current theme
            if (CurrentTheme == ThemeMode.Light)
            {
                CurrentTheme = ThemeMode.Dark;
                // Notify subscribers
                NotifyThemeChanged();
                return;
            }

            // Check current theme
            if (CurrentTheme == ThemeMode.Dark)
            {
                CurrentTheme = ThemeMode.Light;
                NotifyThemeChanged();
                return;
            }

        }

        /// <summary>
        /// Notifies all subscribers that theme has changed
        /// </summary>
        private void NotifyThemeChanged()
        {

            // Check if event has subscribers
            if (OnThemeChanged == null)
            {
                return;
            }

            OnThemeChanged.Invoke();

        }

    }

}