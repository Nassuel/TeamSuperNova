using NUnit.Framework;

/// <summary>
/// Unit tests for InputSanitizationService to ensure proper input sanitization and XSS prevention
/// </summary>
[TestFixture]
public class InputSanitizationServiceTests
{
    // Service instance for testing
    private InputSanitizationService _service;

    /// <summary>
    /// Setup method to initialize test dependencies before each test
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // Arrange - Create fresh service instance for each test
        _service = new InputSanitizationService();
    }

    /// <summary>
    /// Tests that SanitizeComment properly encodes HTML script tags
    /// </summary>
    [Test]
    public void SanitizeComment_Valid_Test_Script_Tag_Should_Return_Encoded_String()
    {
        // Arrange
        var data = "<script>alert('XSS')</script>";

        // Act
        var result = _service.SanitizeComment(data);

        // Assert
        Assert.That(result, Is.EqualTo("&lt;script&gt;alert(&#39;XSS&#39;)&lt;/script&gt;"));
    }

    /// <summary>
    /// Tests that SanitizeComment handles null input by returning empty string
    /// </summary>
    [Test]
    public void SanitizeComment_Invalid_Test_Null_Input_Should_Return_Empty_String()
    {
        // Arrange
        string data = null;

        // Act
        var result = _service.SanitizeComment(data);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that SanitizeComment handles empty string by returning empty string
    /// </summary>
    [Test]
    public void SanitizeComment_Invalid_Test_Empty_String_Should_Return_Empty_String()
    {
        // Arrange
        var data = string.Empty;

        // Act
        var result = _service.SanitizeComment(data);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that SanitizeComment handles whitespace only input by returning empty string
    /// </summary>
    [Test]
    public void SanitizeComment_Invalid_Test_Whitespace_Only_Should_Return_Empty_String()
    {
        // Arrange
        var data = "   ";

        // Act
        var result = _service.SanitizeComment(data);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that SanitizeComment preserves normal text without modification
    /// </summary>
    [Test]
    public void SanitizeComment_Valid_Test_Normal_Text_Should_Return_Same_Text()
    {
        // Arrange
        var data = "This is a normal comment";

        // Act
        var result = _service.SanitizeComment(data);

        // Assert
        Assert.That(result, Is.EqualTo("This is a normal comment"));
    }

    /// <summary>
    /// Tests that SanitizeComment encodes HTML anchor tags
    /// </summary>
    [Test]
    public void SanitizeComment_Valid_Test_Anchor_Tag_Should_Return_Encoded_String()
    {
        // Arrange
        var data = "<a href='javascript:alert(1)'>Click</a>";

        // Act
        var result = _service.SanitizeComment(data);

        // Assert
        Assert.That(result, Is.EqualTo("&lt;a href=&#39;javascript:alert(1)&#39;&gt;Click&lt;/a&gt;"));
    }

    /// <summary>
    /// Tests that SanitizeComment encodes special characters
    /// </summary>
    [Test]
    public void SanitizeComment_Valid_Test_Special_Characters_Should_Return_Encoded_String()
    {
        // Arrange
        var data = "Test & < > \" ' characters";

        // Act
        var result = _service.SanitizeComment(data);

        // Assert
        Assert.That(result, Is.EqualTo("Test &amp; &lt; &gt; &quot; &#39; characters"));
    }

    /// <summary>
    /// Tests that ValidateCommentLength returns true for valid length input
    /// </summary>
    [Test]
    public void ValidateCommentLength_Valid_Test_Within_Max_Length_Should_Return_True()
    {
        // Arrange
        var data = "Valid comment";
        int maxLength = 100;

        // Act
        var result = _service.ValidateCommentLength(data, maxLength);

        // Assert
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Tests that ValidateCommentLength returns false for input exceeding max length
    /// </summary>
    [Test]
    public void ValidateCommentLength_Invalid_Test_Exceeds_Max_Length_Should_Return_False()
    {
        // Arrange
        var data = "This is a very long comment that exceeds the maximum allowed length";
        int maxLength = 10;

        // Act
        var result = _service.ValidateCommentLength(data, maxLength);

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests that ValidateCommentLength returns false for null input
    /// </summary>
    [Test]
    public void ValidateCommentLength_Invalid_Test_Null_Input_Should_Return_False()
    {
        // Arrange
        string data = null;
        int maxLength = 100;

        // Act
        var result = _service.ValidateCommentLength(data, maxLength);

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests that ValidateCommentLength returns false for empty string
    /// </summary>
    [Test]
    public void ValidateCommentLength_Invalid_Test_Empty_String_Should_Return_False()
    {
        // Arrange
        var data = string.Empty;
        int maxLength = 100;

        // Act
        var result = _service.ValidateCommentLength(data, maxLength);

        // Assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Tests that ValidateCommentLength returns true for input exactly at max length
    /// </summary>
    [Test]
    public void ValidateCommentLength_Valid_Test_Exactly_Max_Length_Should_Return_True()
    {
        // Arrange
        var data = "12345";
        int maxLength = 5;

        // Act
        var result = _service.ValidateCommentLength(data, maxLength);

        // Assert
        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns removes script tags from input
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Valid_Test_Script_Tag_Should_Return_Cleaned_String()
    {
        // Arrange
        var data = "Hello <script>alert('XSS')</script> World";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo("Hello  World"));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns removes javascript protocol from input
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Valid_Test_Javascript_Protocol_Should_Return_Cleaned_String()
    {
        // Arrange
        var data = "<a href='javascript:alert(1)'>Click</a>";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo("<a href='alert(1)'>Click</a>"));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns removes onclick event handlers
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Valid_Test_OnClick_Event_Should_Return_Cleaned_String()
    {
        // Arrange
        var data = "<div onclick='alert(1)'>Click me</div>";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo("<div 'alert(1)'>Click me</div>"));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns handles null input by returning empty string
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Invalid_Test_Null_Input_Should_Return_Empty_String()
    {
        // Arrange
        string data = null;

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns handles empty string by returning empty string
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Invalid_Test_Empty_String_Should_Return_Empty_String()
    {
        // Arrange
        var data = string.Empty;

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns handles whitespace only input by returning empty string
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Invalid_Test_Whitespace_Only_Should_Return_Empty_String()
    {
        // Arrange
        var data = "   ";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns preserves safe content
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Valid_Test_Safe_Content_Should_Return_Same_String()
    {
        // Arrange
        var data = "This is a safe comment with no dangerous patterns";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo("This is a safe comment with no dangerous patterns"));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns removes multiple script tags
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Valid_Test_Multiple_Script_Tags_Should_Return_Cleaned_String()
    {
        // Arrange
        var data = "<script>alert(1)</script>Text<script>alert(2)</script>";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo("Text"));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns removes onload event handler
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Valid_Test_OnLoad_Event_Should_Return_Cleaned_String()
    {
        // Arrange
        var data = "<body onload='malicious()'>Content</body>";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo("<body 'malicious()'>Content</body>"));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns is case insensitive for script tags
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Valid_Test_Script_Tag_Case_Insensitive_Should_Return_Cleaned_String()
    {
        // Arrange
        var data = "<SCRIPT>alert('XSS')</SCRIPT>";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo(""));
    }

    /// <summary>
    /// Tests that RemoveDangerousPatterns is case insensitive for javascript protocol
    /// </summary>
    [Test]
    public void RemoveDangerousPatterns_Valid_Test_Javascript_Protocol_Case_Insensitive_Should_Return_Cleaned_String()
    {
        // Arrange
        var data = "<a href='JAVASCRIPT:alert(1)'>Click</a>";

        // Act
        var result = _service.RemoveDangerousPatterns(data);

        // Assert
        Assert.That(result, Is.EqualTo("<a href='alert(1)'>Click</a>"));
    }
}