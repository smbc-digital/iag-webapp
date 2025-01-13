namespace StockportWebappTests_Unit.Unit.Parsers;

public class ProfileTagParserTest
{
    private readonly ProfileTagParser _parser;
    private readonly Mock<IViewRender> _viewRender = new();
    private readonly List<Profile> _profiles = new()
    {
        new() { Slug = "john-doe", Body = "John's bio" },
        new() { Slug = "jane-doe", Body = "Jane's bio" }
    };

    public ProfileTagParserTest() =>
        _parser = new ProfileTagParser(_viewRender.Object);

    [Fact]
    public void HasMatches_WithValidTag_ReturnsTrue()
    {
        // Act
        bool hasMatches = _parser.HasMatches("Hello, {{PROFILE: john-doe}}!");
        
        // Assert
        Assert.True(hasMatches);
    }

    [Fact]
    public void HasMatches_WithNoTag_ReturnsFalse()
    {
        // Act
        bool hasMatches = _parser.HasMatches("Hello, John Doe!");
        
        // Assert
        Assert.False(hasMatches);
    }

    [Fact]
    public void Parse_ReplacesTagWithProfileHtml()
    {
        // Arrange
        _viewRender
            .Setup(viewRender => viewRender.Render(It.IsAny<string>(), It.IsAny<ProfileViewModel>()))
            .Returns("<p>John's bio</p>");

        // Act
        string result = _parser.Parse("Hello, {{PROFILE:john-doe}}!", _profiles, It.IsAny<bool>());

        // Assert
        Assert.Equal("Hello, <p>John's bio</p>!", result);
    }

    [Fact]
    public void Parse_WithNonExistingProfile_RemovesTag()
    {
        // Act
        string result = _parser.Parse("Hello, {{PROFILE: non-existing}}!", _profiles, It.IsAny<bool>());
        
        // Assert
        Assert.Equal("Hello, !", result);
    }
}