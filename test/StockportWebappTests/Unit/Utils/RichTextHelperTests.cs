using Microsoft.AspNetCore.Html;

namespace StockportWebappTests_Unit.Unit.Utils;

public class RichTextRendererTests
{
    [Fact]
    public void Render_ReturnsEmpty_ForNodeWithoutNodeType()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse("{\"foo\":\"bar\"}").RootElement;
        
        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Render_Paragraph_RendersInnerTextWrappedInP()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""nodeType"": ""paragraph"",
            ""content"": [
                { ""nodeType"": ""text"", ""value"": ""hello"" }
            ]
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal("<p>hello</p>", result);
    }

    [Theory]
    [InlineData("heading-1", "<h1>h</h1>")]
    [InlineData("heading-2", "<h2>h</h2>")]
    [InlineData("heading-3", "<h3>h</h3>")]
    public void Render_Headings_RenderChildrenWrapped(string nodeType, string expected)
    {
        // Arrange
        JsonElement json = JsonDocument.Parse($@"
        {{
            ""nodeType"": ""{nodeType}"",
            ""content"": [ {{ ""nodeType"": ""text"", ""value"": ""h"" }} ]
        }}").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Render_UnorderedList_ProducesUlWithLi()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""nodeType"": ""unordered-list"",
            ""content"": [
                {
                    ""nodeType"": ""list-item"",
                    ""content"": [
                        { ""nodeType"": ""paragraph"", ""content"": [ { ""nodeType"": ""text"", ""value"": ""item"" } ] }
                    ]
                }
            ]
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal("<ul><li><p>item</p></li></ul>", result);
    }

    [Fact]
    public void Render_Text_WithMarks_AppliesFormatting()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""nodeType"": ""text"",
            ""value"": ""text"",
            ""marks"": [ { ""type"": ""bold"" }, { ""type"": ""italic"" }, { ""type"": ""underline"" } ]
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal("<u><em><strong>text</strong></em></u>", result);
    }

    [Fact]
    public void Render_Hyperlink_RendersAnchorWithUriAndChildren()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""nodeType"": ""hyperlink"",
            ""data"": { ""uri"": ""/go"" },
            ""content"": [ { ""nodeType"": ""text"", ""value"": ""click"" } ]
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal("<a href='/go'>click</a>", result);
    }

    [Fact]
    public void Render_InlineEntry_ReturnsInlineStatSpan_WithIconWhenProvided()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""nodeType"": ""embedded-entry-inline"",
            ""data"": {
                ""target"": {
                    ""jObject"": {
                        ""statistic"": ""99"",
                        ""body"": ""people"",
                        ""icon"": ""fa-icon""
                    }
                }
            }
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal("<span class='inline-stat'><i class='fa-icon'></i><strong>99</strong> people</span>", result);
    }

    [Fact]
    public void Render_EmbeddedAsset_UsesFileUrlAndDescription()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""nodeType"": ""embedded-asset-block"",
            ""data"": {
                ""target"": {
                    ""file"": { ""url"": ""/img.png"" },
                    ""description"": ""alt text""
                }
            }
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal("<img src='/img.png' alt='alt text' />", result);
    }

    [Fact]
    public void Render_AssetHyperlink_ReturnsChildren_WhenNoTarget()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""nodeType"": ""asset-hyperlink"",
            ""content"": [ { ""nodeType"": ""text"", ""value"": ""inner"" } ]
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal("inner", result);
    }

    [Fact]
    public void Render_AssetHyperlink_UsesFileUrl_WhenPresent()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        {
            ""nodeType"": ""asset-hyperlink"",
            ""data"": { ""target"": { ""file"": { ""url"": ""/f.png"" } } },
            ""content"": [ { ""nodeType"": ""text"", ""value"": ""linktext"" } ]
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(json);

        // Assert
        Assert.Equal("<a href='/f.png'>linktext</a>", result);
    }

    [Fact]
    public void GetEmbeddedContentBlock_ReturnsNull_WhenMissingData()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"{ ""nodeType"": ""embedded-entry-block"" }").RootElement;

        // Act & Assert
        Assert.Null(RichTextRenderer.GetEmbeddedContentBlock(node));
    }

    [Fact]
    public void GetEmbeddedContentBlock_ReturnsNull_WhenMissingTargetOrJObject()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"{ ""nodeType"": ""embedded-entry-block"", ""data"": {} }").RootElement;
        
        // Act & Assert
        Assert.Null(RichTextRenderer.GetEmbeddedContentBlock(node));
    }

    [Fact]
    public void GetEmbeddedContentBlock_ReturnsNull_WhenMissingJObject()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"{ ""nodeType"": ""embedded-entry-block"", ""data"": { ""target"": {} } }").RootElement;

        // Act & Assert
        Assert.Null(RichTextRenderer.GetEmbeddedContentBlock(node));
    }

    [Fact]
    public void GetEmbeddedContentBlock_ReturnsContentBlock_WhenJObjectPresent()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"
        {
            ""nodeType"": ""embedded-entry-block"",
            ""data"": {
                ""target"": {
                    ""jObject"": {
                        ""slug"": ""the-slug"",
                        ""contentType"": ""ct""
                    }
                }
            }
        }").RootElement;

        // Act
        ContentBlock content = RichTextRenderer.GetEmbeddedContentBlock(node);

        // Assert
        Assert.NotNull(content);
        Assert.Equal("the-slug", content.Slug);
    }

    [Fact]
    public void Render_EntryHyperlink_RendersChildren_WhenContentBlockMissing()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"
        {
            ""nodeType"": ""entry-hyperlink"",
            ""data"": {},
            ""content"": [ { ""nodeType"": ""text"", ""value"": ""x"" } ]
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(node);
        
        // Assert
        Assert.Equal("x", result);
    }

    [Fact]
    public void Render_EntryHyperlink_RendersAnchor_WhenContentBlockPresent()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"
        {
            ""nodeType"": ""entry-hyperlink"",
            ""data"": {
                ""target"": {
                    ""jObject"": { ""slug"": ""entry-slug"" }
                }
            },
            ""content"": [ { ""nodeType"": ""text"", ""value"": ""entry text"" } ]
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(node);

        // Assert
        Assert.Equal("<a href='/entry-slug'>entry text</a>", result);
    }

    [Fact]
    public void Render_EmbeddedEntry_ReturnsEmpty_WhenContentTypeEmpty()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"
        {
            ""nodeType"": ""embedded-entry-block"",
            ""data"": {
                ""target"": {
                    ""jObject"": { ""slug"": ""s"", ""contentType"": """" }
                }
            }
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(node);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Render_EmbeddedEntry_ReturnsPartialObject_WhenContentTypePresent()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"
        {
            ""nodeType"": ""embedded-entry-block"",
            ""data"": {
                ""target"": {
                    ""jObject"": { ""slug"": ""s"", ""contentType"": ""ct"" }
                }
            }
        }").RootElement;

        // Act
        object result = RichTextRenderer.Render(node);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EmbeddedPartial>(result);
        Assert.IsNotType<string>(result);

        if (result is IHtmlContent html)
            Assert.False(string.IsNullOrEmpty(html.ToString()));
    }
}