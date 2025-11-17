// Theme management functions

window.themeManager = {
    // Initialize theme from localStorage or default to light
    initialize: function () {
        // Get stored theme
        var storedTheme = localStorage.getItem('theme');

        // Check if theme exists
        if (storedTheme === null) {
            // Set default theme
            this.setTheme('light');
            return;
        }

        // Apply stored theme
        this.setTheme(storedTheme);
    },

    // Set theme and save to localStorage
    setTheme: function (themeName) {
        // Validate input
        if (themeName === null) {
            return;
        }

        // Check if theme is empty
        if (themeName === '') {
            return;
        }

        // Set data attribute
        if (themeName === 'dark') {
            document.documentElement.setAttribute('data-theme', 'dark');
            localStorage.setItem('theme', 'dark');
            return;
        }

        // Set light theme
        if (themeName === 'light') {
            document.documentElement.removeAttribute('data-theme');
            localStorage.setItem('theme', 'light');
            return;
        }
    },

    // Toggle between light and dark theme
    toggle: function () {
        // Get current theme
        var currentTheme = localStorage.getItem('theme');

        // Check if dark theme
        if (currentTheme === 'dark') {
            this.setTheme('light');
            return 'light';
        }

        // Set dark theme
        this.setTheme('dark');
        return 'dark';
    },

    // Get current theme
    getTheme: function () {
        // Get stored theme
        var storedTheme = localStorage.getItem('theme');

        // Check if theme exists
        if (storedTheme === null) {
            return 'light';
        }

        return storedTheme;
    }
};

// Initialize theme on page load
document.addEventListener('DOMContentLoaded', function () {
    window.themeManager.initialize();
});