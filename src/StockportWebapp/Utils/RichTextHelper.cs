namespace StockportWebapp.Utils;

public interface IRichTextHelper
{
    object RenderNode(JsonElement parent, int index);
}

public class RichTextHelper : IRichTextHelper
{
    public object RenderNode(JsonElement parent, int index)
    {
        if (parent.ValueKind != JsonValueKind.Array ||
            index < 0 ||
            index >= parent.GetArrayLength())
            return string.Empty;

        JsonElement node = parent[index];
        string nodeType = node.GetProperty("nodeType").GetString() ?? string.Empty;

        if (nodeType.Equals("paragraph") && IsCaptionParagraph(node))
            return string.Empty;

        return nodeType switch
        {
            "paragraph" => Wrap("p", RenderChildren(node)),
            "heading-2" => Wrap("h2", RenderChildren(node)),
            "heading-3" => Wrap("h3", RenderChildren(node)),
            "heading-4" => Wrap("h4", RenderChildren(node)),
            "heading-5" => Wrap("h5", RenderChildren(node)),
            "heading-6" => Wrap("h6", RenderChildren(node)),
            "unordered-list" => Wrap("ul", RenderChildren(node)),
            "ordered-list" => Wrap("ol", RenderChildren(node)),
            "list-item" => Wrap("li", RenderChildren(node)),
            "text" => RenderText(node),
            "hyperlink" => RenderHyperlink(node),
            "embedded-entry-block" => RenderEmbeddedEntry(node),
            "embedded-entry-inline" => RenderInlineEntry(node),
            "embedded-asset-block" => RenderEmbeddedAsset(parent, index),
            "asset-hyperlink" => RenderAssetHyperlink(node),
            "entry-hyperlink" => RenderEntryHyperlink(node),
            _ => string.Empty
        };
    }

    private string RenderChildren(JsonElement node)
    {
        if (!node.TryGetProperty("content", out JsonElement content) ||
            content.ValueKind != JsonValueKind.Array)
            return string.Empty;

        StringBuilder stringBuilder = new();

        for (int i = 0; i < content.GetArrayLength(); i++)
        {
            stringBuilder.Append(RenderNode(content, i));
        }

        return stringBuilder.ToString();
    }

    private static string Wrap(string tag, string content)
        => $"<{tag}>{content}</{tag}>";
    
    #region Text rendering

    private string RenderText(JsonElement node)
    {
        string text = node.GetProperty("value").GetString() ?? string.Empty;

        if (!node.TryGetProperty("marks", out JsonElement marks) ||
                marks.ValueKind != JsonValueKind.Array ||
                !marks.EnumerateArray().Any())
            return text;

        foreach (JsonElement mark in marks.EnumerateArray())
        {
            string type = mark.GetProperty("type").GetString() ?? string.Empty;

            text = type switch
            {
                "bold" => Wrap("strong", text),
                "italic" => Wrap("em", text),
                "underline" => Wrap("u", text),
                _ => text
            };
        }

        return text;
    }
    
    #endregion

    #region Hyperlinks

    private string RenderHyperlink(JsonElement node)
    {
        string uri = node.GetProperty("data").GetProperty("uri").GetString() ?? "#";

        return $"<a href='{uri}'>{RenderChildren(node)}</a>";
    }

    private string RenderAssetHyperlink(JsonElement node)
    {
        JsonElement target = node.GetPropertyOrDefault("data").GetPropertyOrDefault("target");
        string url = target.GetPropertyOrDefault("file").GetStringOrDefault("url", "#");

        return $"<a href='{url}'>{RenderChildren(node)}</a>";
    }

    private string RenderEntryHyperlink(JsonElement node)
    {
        ContentBlock contentBlock = GetEmbeddedContentBlock(node);
        if (contentBlock is null)
            return RenderChildren(node);
        
        return $"<a href='/{contentBlock.Slug}'>{RenderChildren(node)}</a>";
    }

    #endregion
    
    #region Embedded entries

    private object RenderEmbeddedEntry(JsonElement node)
    {
        ContentBlock contentBlock = GetEmbeddedContentBlock(node);
        if (contentBlock is null || string.IsNullOrEmpty(contentBlock.ContentType))
            return string.Empty;

        return new EmbeddedPartial(contentBlock);
    }

    private string RenderInlineEntry(JsonElement node)
    {
        JsonElement obj = node.GetPropertyOrDefault("data")
            .GetPropertyOrDefault("target")
            .GetPropertyOrDefault("jObject");
        
        string statistic = obj.GetStringOrDefault("statistic");
        string body = obj.GetStringOrDefault("body");
        string icon = obj.GetStringOrDefault("icon");

        return $"<span class='inline-stat {icon}'><strong>{statistic}</strong> {body}</span>";
    }

    public static ContentBlock GetEmbeddedContentBlock(JsonElement node)
    {
        JsonElement obj = node.GetPropertyOrDefault("data")
            .GetPropertyOrDefault("target")
            .GetPropertyOrDefault("jObject");

         if (obj.ValueKind == JsonValueKind.Undefined)
             return null;

         return ContentBlockAdapter.FromJson(obj);
    }

    #endregion

    #region Assets

    private string RenderEmbeddedAsset(JsonElement parent, int index)
    {
        JsonElement node = parent[index];
        JsonElement target = node.GetPropertyOrDefault("data").GetPropertyOrDefault("target");

        string url = GetAssetUrl(target);
        string alt = target.GetStringOrDefault("description");
        string floatClass = GetFloatClass(parent, index - 1) ?? GetFloatClass(parent, index + 1);
        string caption = TryGetCaption(parent, index - 1) ?? TryGetCaption(parent, index + 1);
        
        bool hasCaption = !string.IsNullOrEmpty(caption);
        string imgClass = hasCaption ? string.Empty : "class=\"image-rounded\"";
        string srcset = $"{url}?w=722&q=89&fm=webp 722w, " +
                    $"{url}?w=969&q=89&fm=webp 969w, " +
                    $"{url}?w=852&q=89&fm=webp 852w";

        string sizes = $"(max-width: 767px) 722px, " +
                    $"(min-width: 768px) and (max-width: 1023px) 969px, " +
                    $"(min-width: 1024px) 852px";

        if (string.IsNullOrEmpty(floatClass) && string.IsNullOrEmpty(caption))
            return $"<img src=\"{url}?q=89&fm=webp\" alt=\"{alt}\" {imgClass} srcset=\"{srcset}\" sizes=\"{sizes}\" />";

        StringBuilder stringBuilder = new();

        stringBuilder.Append($@"<figure class=""{floatClass}"">");
        stringBuilder.Append($@"<p><img src=""{url}?q=89&fm=webp"" alt=""{alt}"" {imgClass} srcset=""{srcset}"" sizes=""{sizes}"" /></p>");

        if (!string.IsNullOrEmpty(caption))
            stringBuilder.Append($"<figcaption>{caption}</figcaption>");

        stringBuilder.Append("</figure>");

        return stringBuilder.ToString();
    }

    private static string GetAssetUrl(JsonElement target) =>
        target.GetPropertyOrDefault("file").GetStringOrDefault("url", "#");

    #endregion

    #region Caption / float parsing

    private static bool IsCaptionParagraph(JsonElement node)
    {
        JsonElement content = node.GetPropertyOrDefault("content");

        if (content.ValueKind != JsonValueKind.Array || content.GetArrayLength().Equals(0))
            return false;

        string value = content[0].GetStringOrDefault("value");

        return value.StartsWith("^^^");
    }

    private static string? GetFloatClass(JsonElement parent, int index)
    {
        if (!IsValidIndex(parent, index))
            return null;

        JsonElement node = parent[index];
        if (!node.GetString("nodeType").Equals("paragraph"))
            return null;

        JsonElement content = node.GetPropertyOrDefault("content");
        if (content.GetArrayLength().Equals(0))
            return null;

        string value = content[0].GetStringOrDefault("value");

        if (value.StartsWith("^^^#left"))
            return "image-left";

        if (value.StartsWith("^^^#right"))
            return "image-right";

        return null;
    }

    private static string? TryGetCaption(JsonElement parent, int index)
    {
        if (!IsValidIndex(parent, index))
            return null;
        
        JsonElement node = parent[index];
        if (!node.GetString("nodeType").Equals("paragraph"))
            return null;
        
        JsonElement content = node.GetPropertyOrDefault("content");
        if (content.GetArrayLength().Equals(0))
            return null;

        string value = content[0].GetStringOrDefault("value");
        if (!value.StartsWith("^^^"))
            return null;
        
        if (value.StartsWith("^^^#left") || value.StartsWith("^^^#right"))
            return null;
        
        string caption = value.Substring(3).Trim();

        return string.IsNullOrWhiteSpace(caption)
            ? null
            : caption;
    }

    private static bool IsValidIndex(JsonElement parent, int index) =>
        parent.ValueKind == JsonValueKind.Array &&
        index >= 0 &&
        index < parent.GetArrayLength();

    #endregion
}

internal static class JsonExtensions
{
    public static JsonElement GetPropertyOrDefault(this JsonElement element, string name)
    {
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty(name, out var value))
            return value;

        return default;
    }

    public static string GetStringOrDefault(this JsonElement element, string propertyName, string defaultValue = "")
    {
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty(propertyName, out var value) &&
            value.ValueKind == JsonValueKind.String)
            return value.GetString() ?? defaultValue;

        return defaultValue;
    }

    public static string GetString(this JsonElement element, string propertyName)
        => element.GetStringOrDefault(propertyName, string.Empty);
}