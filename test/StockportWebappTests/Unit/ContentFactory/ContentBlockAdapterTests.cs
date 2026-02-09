namespace StockportWebappTests_Unit.Unit.ContentFactory;

public class ContentBlockAdapterTests
{
    [Fact]
    public void Get_ReturnsString_WhenPropertyExistsDirectly()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse("{\"slug\":\"my-slug\"}").RootElement;

        // Act
        string result = ContentBlockAdapter.Get(json, "slug");

        // Assert
        Assert.Equal("my-slug", result);
    }

    [Fact]
    public void Get_IsCaseInsensitive_WhenPropertyNameCasingDiffers()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse("{\"TiTlE\":\"My Title\"}").RootElement;

        // Act
        string result = ContentBlockAdapter.Get(json, "title");

        // Assert
        Assert.Equal("My Title", result);
    }

    [Fact]
    public void Get_FindsValueInsideFieldsObject()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse("{\"fields\": {\"teaser\":\"a teaser\"}}").RootElement;

        // Act
        string result = ContentBlockAdapter.Get(json, "teaser");

        // Assert
        Assert.Equal("a teaser", result);
    }

    [Fact]
    public void Get_ReturnsEmptyString_WhenPropertyMissing()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse("{\"some\":\"value\"}").RootElement;

        // Act
        string result = ContentBlockAdapter.Get(json, "nonexistent");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Get_ExtractsNumberAndBooleans_AsStrings()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse("{\"n\":123, \"t\": true, \"f\": false}").RootElement;

        // Act & Assert
        Assert.Equal("123", ContentBlockAdapter.Get(json, "n"));
        Assert.Equal("true", ContentBlockAdapter.Get(json, "t"));
        Assert.Equal("false", ContentBlockAdapter.Get(json, "f"));
    }

    [Fact]
    public void ParseColour_ReturnsNone_WhenNoColourSchemeProperty()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse("{\"something\":\"value\"}").RootElement;

        // Act
        object result = typeof(ContentBlockAdapter)
            .GetMethod("ParseColour", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .Invoke(null, [json]);

        // Assert
        Assert.Equal(EColourScheme.None, result);
    }

    [Fact]
    public void ParseColour_UsesColourField_WhenPresentAndValid()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"{ ""colourScheme"": ""whatever"", ""colour"": ""Teal"" }").RootElement;

        // Act
        object result = typeof(ContentBlockAdapter)
            .GetMethod("ParseColour", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .Invoke(null, [json]);

        // Assert
        Assert.Equal(EColourScheme.Teal, result);
    }

    [Fact]
    public void ParseColour_UsesColourScheme_WhenColourFieldMissing()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"{ ""colourScheme"": ""Pink"" }").RootElement;

        // Act
        object result = typeof(ContentBlockAdapter)
            .GetMethod("ParseColour", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .Invoke(null, [json]);

        // Assert
        Assert.Equal(EColourScheme.Pink, result);
    }

    [Fact]
    public void ParseColour_ReturnsNone_ForInvalidValues()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"{ ""colourScheme"": ""INVALID_VALUE"" }").RootElement;

        // Act
        object result = typeof(ContentBlockAdapter)
            .GetMethod("ParseColour", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .Invoke(null, [json]);

        // Assert
        Assert.Equal(EColourScheme.None, result);
    }

    [Fact]
    public void GetImage_ReturnsEmpty_WhenNoImageProperty()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse("{\"slug\":\"x\"}").RootElement;

        // Act
        object result = typeof(ContentBlockAdapter)
            .GetMethod("GetImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .Invoke(null, [json]);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetImage_ReturnsId_WhenSysIdPresent()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""image"": {
                ""sys"": { ""id"": ""image-123"" }
            }
        }").RootElement;

        // Act
        object result = typeof(ContentBlockAdapter)
            .GetMethod("GetImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .Invoke(null, [json]);

        // Assert
        Assert.Equal("image-123", result);
    }

    [Fact]
    public void GetImage_ReturnsEmpty_WhenImageHasNoSysOrId()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"{ ""image"": { ""foo"": ""bar"" } }").RootElement;

        // Act
        object result = typeof(ContentBlockAdapter)
            .GetMethod("GetImage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .Invoke(null, [json]);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void FromJson_MapsFields_Correctly()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""slug"": ""the-slug"",
            ""title"": ""The Title"",
            ""teaser"": ""a teaser"",
            ""type"": ""card"",
            ""contentType"": ""ct"",
            ""image"": { ""sys"": { ""id"": ""img-1"" } },
            ""mailingListId"": ""ml-1"",
            ""body"": ""# header"",
            ""subItems"": [
                { ""slug"": ""child-1"", ""title"": ""Child 1"", ""body"": ""text"" }
            ],
            ""link"": ""https://example.com"",
            ""buttonText"": ""Do it"",
            ""colourScheme"": ""Teal"",
            ""statistic"": ""42"",
            ""screenReader"": ""sr"",
            ""accountName"": ""account""
        }").RootElement;

        // Act
        ContentBlock block = ContentBlockAdapter.FromJson(json);

        // Assert
        Assert.Equal("the-slug", block.Slug);
        Assert.Equal("The Title", block.Title);
        Assert.Equal("a teaser", block.Teaser);
        Assert.Equal("card", block.Type);
        Assert.Equal("ct", block.ContentType);
        Assert.Equal("img-1", block.Image);
        Assert.Equal("ml-1", block.MailingListId);
        Assert.NotEmpty(block.Body);
        Assert.Single(block.SubItems);
        Assert.Equal("https://example.com", block.Link);
        Assert.Equal("Do it", block.ButtonText);
        Assert.Equal(EColourScheme.Teal, block.ColourScheme);
        Assert.Equal("42", block.Statistic);
        Assert.Equal("sr", block.ScreenReader);
        Assert.Equal("account", block.AccountName);
    }
}