using Microsoft.AspNetCore.Html;

namespace StockportWebappTests_Unit.Unit.Utils;

public class RichTextHelperTests
{
    private readonly RichTextHelper _helper = new();

    private static JsonElement Parse(string json)
        => JsonDocument.Parse(json).RootElement;

    private static string RenderAsString(JsonElement parent, int index)
        => new RichTextHelper().RenderNode(parent, index) as string ?? string.Empty;

    [Fact]
    public void RenderNode_ReturnsEmpty_WhenParentIsNotArray()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"{""foo"":""bar""}").RootElement;
        
        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RenderNode_Paragraph_RendersChildrenWrappedInP()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""paragraph"",
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""Hello"" } ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);

        // Assert
        Assert.Equal("<p>Hello</p>", result);
    }

    [Fact]
    public void RenderNode_Paragraph_CaptionParagraph_ReturnsEmpty()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""paragraph"",
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""^^^This is a caption"" } ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("heading-2", "<h2>Hi</h2>")]
    [InlineData("heading-3", "<h3>Hi</h3>")]
    [InlineData("heading-4", "<h4>Hi</h4>")]
    [InlineData("heading-5", "<h5>Hi</h5>")]
    [InlineData("heading-6", "<h6>Hi</h6>")]
    public void RenderNode_Headings_WrapsChildren(string nodeType, string expected)
    {
        // Arrange
        JsonElement json = JsonDocument.Parse($@"
        [
            {{
                ""nodeType"": ""{nodeType}"",
                ""content"": [ {{ ""nodeType"": ""text"", ""value"": ""Hi"" }} ]
            }}
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void RenderNode_UnorderedList_RendersUlWithLi()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
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
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<ul><li><p>item</p></li></ul>", result);
    }

    [Fact]
    public void RenderNode_OrderedList_RendersOlWithLi()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""ordered-list"",
                ""content"": [
                    {
                        ""nodeType"": ""list-item"",
                        ""content"": [
                            { ""nodeType"": ""paragraph"", ""content"": [ { ""nodeType"": ""text"", ""value"": ""item"" } ] }
                        ]
                    }
                ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<ol><li><p>item</p></li></ol>", result);
    }

    [Fact]
    public void RenderNode_ListItem_WrapsChildrenInLi()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""list-item"",
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""list item"" } ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<li>list item</li>", result);
    }

    [Fact]
    public void RenderNode_Text_WithMarks_AppliesFormatting()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""text"",
                ""value"": ""text"",
                ""marks"": [ { ""type"": ""bold"" }, { ""type"": ""italic"" }, { ""type"": ""underline"" } ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<u><em><strong>text</strong></em></u>", result);
    }

    [Fact]
    public void RenderNode_Hyperlink_RendersAnchor()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""hyperlink"",
                ""data"": { ""uri"": ""/go"" },
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""click"" } ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<a href='/go'>click</a>", result);
    }

    [Fact]
    public void RenderNode_InlineEntry_RendersInlineStatSpan()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""embedded-entry-inline"",
                ""data"": {
                    ""target"": {
                        ""jObject"": { ""statistic"": ""99"", ""body"": ""people"", ""icon"": ""fa-icon"" }
                    }
                }
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<span class='inline-stat fa-icon'><strong>99</strong> people</span>", result);
    }

    [Fact]
    public void RenderNode_EmbeddedAsset_RendersImgTag()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""embedded-asset-block"",
                ""data"": {
                    ""target"": { ""file"": { ""url"": ""/img.png"" }, ""description"": ""alt"" }
                }
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        string html = result.ToString();

        Assert.StartsWith("<img", html);
        Assert.Contains("src=\"/img.png?q=89&fm=webp\"", html);
        Assert.Contains("alt=\"alt\"", html);
        Assert.Contains("class=\"image-rounded\"", html);
        Assert.Contains("srcset=", html);
        Assert.Contains("sizes=", html);
    }

    [Fact]
    public void RenderNode_AssetHyperlink_RendersAnchor()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""asset-hyperlink"",
                ""data"": { ""target"": { ""file"": { ""url"": ""/f.png"" } } },
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""linktext"" } ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<a href='/f.png'>linktext</a>", result);
    }

    [Fact]
    public void RenderNode_EntryHyperlink_ReturnsChildren_WhenContentBlockMissing()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""entry-hyperlink"",
                ""data"": {},
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""link"" } ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("link", result);
    }

    [Fact]
    public void RenderNode_EntryHyperlink_RendersAnchor_WhenContentBlockPresent()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""entry-hyperlink"",
                ""data"": {
                    ""target"": {
                        ""jObject"": { ""slug"": ""my-slug"", ""contentType"": ""ct"" }
                    }
                },
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""entry text"" } ]
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<a href='/my-slug'>entry text</a>", result);
    }

    [Fact]
    public void RenderNode_EmbeddedEntry_ReturnsEmpty_WhenContentTypeEmpty()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""embedded-entry-block"",
                ""data"": { ""target"": { ""jObject"": { ""slug"": ""s"", ""contentType"": """" } } }
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RenderNode_EmbeddedEntry_ReturnsEmbeddedPartial_WhenContentTypePresent()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""embedded-entry-block"",
                ""data"": { ""target"": { ""jObject"": { ""slug"": ""s"", ""contentType"": ""ct"" } } }
            }
        ]").RootElement;
        
        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.IsType<EmbeddedPartial>(result);
    }

    [Fact]
    public void GetEmbeddedContentBlock_ReturnsNull_WhenNoJObject()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"
        {
            ""nodeType"": ""embedded-entry-block"",
            ""data"": { ""target"": {} }
        }").RootElement;

        // Act
        object result = RichTextHelper.GetEmbeddedContentBlock(node);
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void GetEmbeddedContentBlock_ReturnsContentBlock_WhenPresent()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"
        {
            ""nodeType"": ""embedded-entry-block"",
            ""data"": {
                ""target"": { ""jObject"": { ""slug"": ""s"", ""contentType"": ""ct"" } }
            }
        }").RootElement;

        // Act
        object result = RichTextHelper.GetEmbeddedContentBlock(node);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("s", result is ContentBlock cb ? cb.Slug : string.Empty);
    }

    [Fact]
    public void GetEmbeddedContentBlock_ReturnsNull_WhenMissingData()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"{ ""nodeType"": ""embedded-entry-block"" }").RootElement;

        // Act & Assert
        Assert.Null(RichTextHelper.GetEmbeddedContentBlock(node));
    }

    [Fact]
    public void GetEmbeddedContentBlock_ReturnsNull_WhenMissingTargetOrJObject()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"{ ""nodeType"": ""embedded-entry-block"", ""data"": {} }").RootElement;
        
        // Act & Assert
        Assert.Null(RichTextHelper.GetEmbeddedContentBlock(node));
    }

    [Fact]
    public void GetEmbeddedContentBlock_ReturnsNull_WhenMissingJObject()
    {
        // Arrange
        JsonElement node = JsonDocument.Parse(@"{ ""nodeType"": ""embedded-entry-block"", ""data"": { ""target"": {} } }").RootElement;

        // Act & Assert
        Assert.Null(RichTextHelper.GetEmbeddedContentBlock(node));
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
        ContentBlock content = RichTextHelper.GetEmbeddedContentBlock(node);

        // Assert
        Assert.NotNull(content);
        Assert.Equal("the-slug", content.Slug);
    }

    [Fact]
    public void RenderEmbeddedAsset_WithLeftFloat_AddsFigureClass()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""paragraph"",
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""^^^#left"" } ]
            },
            {
                ""nodeType"": ""embedded-asset-block"",
                ""data"": { ""target"": { ""file"": { ""url"": ""/img.png"" }, ""description"": ""alt"" } }
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 1);
        
        // Assert
        Assert.Contains("<figure class=\"image-left\"><p><img src=\"/img.png?q=89&fm=webp\" alt=\"alt\" class=\"image-rounded\" srcset=\"/img.png?w=722&q=89&fm=webp 722w, /img.png?w=969&q=89&fm=webp 969w, /img.png?w=852&q=89&fm=webp 852w\" sizes=\"(max-width: 767px) 722px, (min-width: 768px) and (max-width: 1023px) 969px, (min-width: 1024px) 852px\" /></p></figure>", result.ToString());
        Assert.Contains("img src=\"/img.png?q=89&fm=webp\"", result.ToString());
    }

    [Fact]
    public void RenderEmbeddedAsset_WithRightFloat_AddsFigureClass()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""paragraph"",
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""^^^#right"" } ]
            },
            {
                ""nodeType"": ""embedded-asset-block"",
                ""data"": { ""target"": { ""file"": { ""url"": ""/img.png"" }, ""description"": ""alt"" } }
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 1);

        // Assert
        string html = result.ToString();

        Assert.Contains("figure class=\"image-right\"", html);
        Assert.Contains("src=\"/img.png", html);
        Assert.Contains("image-rounded", html);
    }

    [Fact]
    public void RenderEmbeddedAsset_WithCaption_RendersFigcaption()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            {
                ""nodeType"": ""paragraph"",
                ""content"": [ { ""nodeType"": ""text"", ""value"": ""^^^This is a caption"" } ]
            },
            {
                ""nodeType"": ""embedded-asset-block"",
                ""data"": { ""target"": { ""file"": { ""url"": ""/img.png"" }, ""description"": ""alt"" } }
            }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 1);
        
        // Assert
        Assert.Contains("<figcaption>This is a caption</figcaption>", result.ToString());
    }

    [Fact]
    public void RenderChildren_ReturnsEmpty_WhenNoContent()
    {
        // Arrange
        JsonElement json = JsonDocument.Parse(@"
        [
            { ""nodeType"": ""paragraph"" }
        ]").RootElement;

        // Act
        object result = _helper.RenderNode(json, 0);
        
        // Assert
        Assert.Equal("<p></p>", result);
    }
}