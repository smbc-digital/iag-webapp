using System.Text.Json;

namespace StockportWebapp.Utils;

public static class RichTextRenderer
{
    /// <summary>
    /// Render normal rich text nodes (paragraphs, headings, hyperlinks, text) into HTML string
    /// </summary>
    public static string RenderTextNode(JsonElement node)
    {
        if (!node.TryGetProperty("nodeType", out JsonElement nodeTypeElement))
            return string.Empty;

        string nodeType = nodeTypeElement.GetString() ?? string.Empty;

        switch (nodeType)
        {
            case "paragraph":
                return $"<p>{RenderChildren(node)}</p>";
            case "heading-1":
                return $"<h1>{RenderChildren(node)}</h1>";
            case "heading-2":
                return $"<h2>{RenderChildren(node)}</h2>";
            case "heading-3":
                return $"<h3>{RenderChildren(node)}</h3>";
            case "hyperlink":
                string uri = node.GetProperty("data").GetProperty("uri").GetString() ?? "#";
                return $"<a href='{uri}'>{RenderChildren(node)}</a>";
            case "text":
                return node.GetProperty("value").GetString() ?? string.Empty;
            default:
                return string.Empty; // ignore unknown node types
        }
    }

    /// <summary>
    /// Render the child nodes recursively
    /// </summary>
    private static string RenderChildren(JsonElement node)
    {
        if (!node.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Array)
            return string.Empty;

        var sb = new StringBuilder();

        foreach (var child in content.EnumerateArray())
        {
            sb.Append(RenderTextNode(child));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Extract a ContentBlock from an embedded-entry-block node
    /// </summary>
    public static ContentBlock GetEmbeddedContentBlock(JsonElement node)
    {
        if (!node.TryGetProperty("data", out var data)) return null;
        if (!data.TryGetProperty("target", out var target)) return null;
        if (!target.TryGetProperty("jObject", out var obj)) return null;

        return ContentBlockAdapter.FromJson(obj); // assumes you have a ContentBlockAdapter
    }
}
