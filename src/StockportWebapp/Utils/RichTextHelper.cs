namespace StockportWebapp.Utils;

public static class RichTextRenderer
{
    public static object Render(JsonElement node)
    {
        if (!node.TryGetProperty("nodeType", out JsonElement nodeTypeProp))
            return string.Empty;

        string nodeType = nodeTypeProp.GetString() ?? string.Empty;

        return nodeType switch
        {
            "paragraph" => $"<p>{RenderChildren(node)}</p>",
            "heading-1" => $"<h1>{RenderChildren(node)}</h1>",
            "heading-2" => $"<h2>{RenderChildren(node)}</h2>",
            "heading-3" => $"<h3>{RenderChildren(node)}</h3>",
            "unordered-list" => RenderList(node, "ul"),
            "ordered-list" => RenderList(node, "ol"),
            "list-item" => RenderListItem(node),
            "hyperlink" => RenderHyperlink(node),
            "text" => RenderText(node),
            "embedded-entry-block" => RenderEmbeddedEntry(node),
            "embedded-entry-inline" => RenderInlineEntry(node),
            "embedded-asset-block" => RenderEmbeddedAsset(node),
            "asset-hyperlink" => RenderAssetHyperlink(node),
            "entry-hyperlink" => RenderEntryHyperlink(node),
            _ => string.Empty
        };
    }

    private static string RenderChildren(JsonElement node)
    {
        if (!node.TryGetProperty("content", out JsonElement content) || content.ValueKind != JsonValueKind.Array)
            return string.Empty;

        StringBuilder sb = new();
        foreach (JsonElement child in content.EnumerateArray())
        {
            object rendered = Render(child);
            if (rendered is string s)
                sb.Append(s);
            else if (rendered is IHtmlContent html)
                sb.Append(html.ToString());
        }

        return sb.ToString();
    }

    private static string RenderText(JsonElement node)
    {
        string text = node.GetProperty("value").GetString() ?? string.Empty;

        if (!node.TryGetProperty("marks", out JsonElement marks) ||
                marks.ValueKind != JsonValueKind.Array ||
                !marks.EnumerateArray().Any())
            return text;

        foreach (JsonElement mark in marks.EnumerateArray())
        {
            if (!mark.TryGetProperty("type", out JsonElement typeProp))
                continue;

            string type = typeProp.GetString();

            text = type switch
            {
                "bold" => $"<strong>{text}</strong>",
                "italic" => $"<em>{text}</em>",
                "underline" => $"<u>{text}</u>",
                _ => text
            };
        }

        return text;
    }

    private static string RenderList(JsonElement node, string tag) =>
        $"<{tag}>{RenderChildren(node)}</{tag}>";

    private static string RenderListItem(JsonElement node) =>
        $"<li>{RenderChildren(node)}</li>";

    private static string RenderHyperlink(JsonElement node)
    {
        string uri = node.GetProperty("data").GetProperty("uri").GetString() ?? "#";

        return $"<a href='{uri}'>{RenderChildren(node)}</a>";
    }

    private static string RenderInlineEntry(JsonElement node)
    {
        if (!node.TryGetProperty("data", out JsonElement data) ||
            !data.TryGetProperty("target", out JsonElement target) ||
            !target.TryGetProperty("jObject", out JsonElement obj))
            return string.Empty;

        string statistic = obj.TryGetProperty("statistic", out JsonElement statProp)
            ? statProp.GetString()
            : string.Empty;
        
        string body = obj.TryGetProperty("body", out JsonElement bodyProp)
            ? bodyProp.GetString()
            : string.Empty;
        
        string icon = obj.TryGetProperty("icon", out JsonElement iconProp)
            ? iconProp.GetString()
            : string.Empty;

        string iconHtml = !string.IsNullOrEmpty(icon)
            ? $"<i class='{icon}'></i>"
            : string.Empty;

        return $"<span class='inline-stat'>{iconHtml}<strong>{statistic}</strong> {body}</span>";
    }

    private static string RenderEmbeddedAsset(JsonElement node)
    {
        JsonElement target = node.GetProperty("data").GetProperty("target");
        string url = GetAssetUrl(target);
        string alt = target.TryGetProperty("description", out JsonElement desc)
            ? desc.GetString()
            : string.Empty;
        
        return $"<img src='{url}' alt='{alt}' />";
    }

   private static string RenderAssetHyperlink(JsonElement node)
    {
        if (!node.TryGetProperty("data", out JsonElement data) ||
            !data.TryGetProperty("target", out JsonElement target))
            return RenderChildren(node);

        string url = "#";
        if (target.TryGetProperty("file", out JsonElement file) &&
            file.TryGetProperty("url", out JsonElement urlProp) &&
            urlProp.ValueKind == JsonValueKind.String)
        {
            url = urlProp.GetString()!;
        }

        string innerText = RenderChildren(node);

        return $"<a href='{url}'>{innerText}</a>";
    }

    private static string RenderEntryHyperlink(JsonElement node)
    {
        ContentBlock contentBlock = GetEmbeddedContentBlock(node);
        if (contentBlock is null)
            return RenderChildren(node);
        
        string text = RenderChildren(node);

        return $"<a href='/{contentBlock.Slug}'>{text}</a>";
    }

    private static string GetAssetUrl(JsonElement target)
    {
        if (target.TryGetProperty("file", out JsonElement file)
            && file.TryGetProperty("url", out JsonElement urlProp)
            && urlProp.ValueKind == JsonValueKind.String)
            return urlProp.GetString() ?? "#";

        return "#";
    }

    public static ContentBlock GetEmbeddedContentBlock(JsonElement node)
    {
        if (!node.TryGetProperty("data", out JsonElement data))
            return null;

        if (!data.TryGetProperty("target", out JsonElement target))
            return null;
        
        if (!target.TryGetProperty("jObject", out JsonElement obj))
            return null;

        return ContentBlockAdapter.FromJson(obj);
    }

    private static object RenderEmbeddedEntry(JsonElement node)
    {
        var entry = GetEmbeddedEntry(node);
        if (entry == null)
            return string.Empty;

        if (entry.IsContentBlock)
            return new EmbeddedPartial(entry.ContentBlock!);

        if (entry.IsImageBlock)
            return new EmbeddedImagePartial(entry.ImageBlock!);

        return string.Empty;
    }

    public static EmbeddedEntry? GetEmbeddedEntry(JsonElement node)
    {
        if (!node.TryGetProperty("data", out JsonElement data) ||
            !data.TryGetProperty("target", out JsonElement target) ||
            !target.TryGetProperty("jObject", out JsonElement obj))
            return null;

        if (!obj.TryGetProperty("sys", out JsonElement sys) ||
            !sys.TryGetProperty("contentType", out JsonElement contentType) ||
            !contentType.TryGetProperty("sys", out JsonElement contentTypeSys) ||
            !contentTypeSys.TryGetProperty("id", out JsonElement idProp))
            return null;

        string type = idProp.GetString() ?? string.Empty;

        return type switch
        {
            "contentBlock" => new EmbeddedEntry
            {
                ContentBlock = ContentBlockAdapter.FromJson(obj)
            },

            "imageBlock" => new EmbeddedEntry
            {
                ImageBlock = ImageBlockAdapter.FromJson(obj)
            },

            _ => null
        };
    }
}