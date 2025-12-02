// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

/// <summary>
/// Opens the product modal programmatically
/// </summary>
function openProductModal() {

    // Get the modal element
    var modal = document.getElementById('productModal');

    // Fast fail: Modal not found
    if (!modal) {
        return;
    }

    // Use jQuery to show the modal (Bootstrap 4)
    $('#productModal').modal('show');

}

/// <summary>
/// Closes the product modal programmatically
/// </summary>
function closeProductModal() {

    // Get the modal element
    var modal = document.getElementById('productModal');

    // Fast fail: Modal not found
    if (!modal) {
        return;
    }

    // Use jQuery to hide the modal (Bootstrap 4)
    $('#productModal').modal('hide');

}

/// <summary>
/// Copies text to the clipboard
/// </summary>
/// <param name="text">Text to copy</param>
async function copyToClipboard(text) {

    // Fast fail: No text provided
    if (!text) {
        return false;
    }

    // Check if clipboard API is available
    if (navigator.clipboard) {

        // Use modern clipboard API
        await navigator.clipboard.writeText(text);
        return true;

    }

    // Fallback for older browsers
    var textArea = document.createElement("textarea");

    // Set the text
    textArea.value = text;

    // Prevent scrolling
    textArea.style.position = "fixed";
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.width = "2em";
    textArea.style.height = "2em";
    textArea.style.padding = "0";
    textArea.style.border = "none";
    textArea.style.outline = "none";
    textArea.style.boxShadow = "none";
    textArea.style.background = "transparent";

    // Add to document
    document.body.appendChild(textArea);

    // Select and copy
    textArea.focus();
    textArea.select();
    document.execCommand('copy');

    // Remove from document
    document.body.removeChild(textArea);

    return true;

}

/// <summary>
/// Updates the browser URL without navigation (for bookmarking)
/// </summary>
/// <param name="url">New URL to set</param>
function updateBrowserUrl(url) {

    // Fast fail: No URL provided
    if (!url) {
        return;
    }

    // Update URL without page reload
    window.history.pushState({}, '', url);

}

/// <summary>
/// Clears the product query parameter from URL
/// </summary>
function clearProductQueryParam() {

    // Get current URL without query string
    var url = window.location.protocol + "//" + window.location.host + window.location.pathname;

    // Update URL without page reload
    window.history.pushState({}, '', url);

}