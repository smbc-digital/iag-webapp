using System.Text.Json;
using Microsoft.AspNetCore.Html;

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
        if (!node.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Array)
            return string.Empty;

        var sb = new StringBuilder();
        foreach (var child in content.EnumerateArray())
        {
            var rendered = Render(child);
            if (rendered is string s) sb.Append(s);
            else if (rendered is IHtmlContent html) sb.Append(html.ToString());
        }
        return sb.ToString();
    }

    private static string RenderText(JsonElement node)
    {
        string text = node.GetProperty("value").GetString() ?? "";

        if (!node.TryGetProperty("marks", out var marks) ||
            marks.ValueKind != JsonValueKind.Array ||
            !marks.EnumerateArray().Any())
        {
            return text;
        }

        foreach (var mark in marks.EnumerateArray())
        {
            if (!mark.TryGetProperty("type", out var typeProp))
                continue;

            var type = typeProp.GetString();

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

    private static string RenderList(JsonElement node, string tag)
    {
        return $"<{tag}>{RenderChildren(node)}</{tag}>";
    }

    private static string RenderListItem(JsonElement node)
    {
        return $"<li>{RenderChildren(node)}</li>";
    }

    private static string RenderHyperlink(JsonElement node)
    {
        string uri = node.GetProperty("data").GetProperty("uri").GetString() ?? "#";
        return $"<a href='{uri}'>{RenderChildren(node)}</a>";
    }

    private static object RenderEmbeddedEntry(JsonElement node)
    {
        var contentBlock = GetEmbeddedContentBlock(node);
        if (contentBlock == null || string.IsNullOrEmpty(contentBlock.ContentType))
            return string.Empty;

        return new EmbeddedPartial(contentBlock);
    }

    private static string RenderInlineEntry(JsonElement node)
    {
        if (!node.TryGetProperty("data", out var data) ||
            !data.TryGetProperty("target", out var target) ||
            !target.TryGetProperty("jObject", out var obj))
            return string.Empty;

        string statistic = obj.TryGetProperty("statistic", out var statProp) ? statProp.GetString() : "";
        string body = obj.TryGetProperty("body", out var bodyProp) ? bodyProp.GetString() : "";
        string icon = obj.TryGetProperty("icon", out var iconProp) ? iconProp.GetString() : "";
        // string colour = obj.TryGetProperty("colourScheme", out var colourProp) ? colourProp.GetString() : "default";

        var iconHtml = !string.IsNullOrEmpty(icon) ? $"<i class='{icon}'></i>" : "";
        // return $"<span class='inline-stat {colour}'>{iconHtml}<strong>{statistic}</strong> {body}</span>";
        return $"<span class='inline-stat'>{iconHtml}<strong>{statistic}</strong> {body}</span>";
    }

    private static string RenderEmbeddedAsset(JsonElement node)
    {
        var target = node.GetProperty("data").GetProperty("target");
        string url = GetAssetUrl(target);
        string alt = target.TryGetProperty("description", out var desc) ? desc.GetString() : "";
        return $"<img src='{url}' alt='{alt}' />";
    }

   private static string RenderAssetHyperlink(JsonElement node)
    {
        // Get the target object safely
        if (!node.TryGetProperty("data", out var data) ||
            !data.TryGetProperty("target", out var target))
            return RenderChildren(node); // fallback to inner text

        // Drill down to file.url
        string url = "#"; // default
        if (target.TryGetProperty("file", out var file) &&
            file.TryGetProperty("url", out var urlProp) &&
            urlProp.ValueKind == JsonValueKind.String)
        {
            url = urlProp.GetString()!;
        }

        // Render the children of the hyperlink node as inner text
        string innerText = RenderChildren(node);

        return $"<a href='{url}'>{innerText}</a>";
    }

    private static string RenderEntryHyperlink(JsonElement node)
    {
        var target = node.GetProperty("data").GetProperty("target");
        var contentBlock = GetEmbeddedContentBlock(node);
        if (contentBlock == null) return RenderChildren(node);
        string text = RenderChildren(node);
        // Use slug as href
        string href = "/" + contentBlock.Slug;
        return $"<a href='{href}'>{text}</a>";
    }

    private static string GetAssetUrl(JsonElement target)
    {
        if (target.TryGetProperty("file", out var file) && file.TryGetProperty("url", out var urlProp) && urlProp.ValueKind == JsonValueKind.String)
            return urlProp.GetString() ?? "#";

        // fallback to localized files
        if (target.TryGetProperty("filesLocalized", out var filesLocalized)
            && filesLocalized.TryGetProperty("en-GB", out var enGb)
            && enGb.TryGetProperty("url", out var localizedUrl)
            && localizedUrl.ValueKind == JsonValueKind.String)
        {
            return localizedUrl.GetString() ?? "#";
        }

        return "#";
    }

    public static ContentBlock GetEmbeddedContentBlock(JsonElement node)
    {
        if (!node.TryGetProperty("data", out var data)) return null;
        if (!data.TryGetProperty("target", out var target)) return null;
        if (!target.TryGetProperty("jObject", out var obj)) return null;

        return ContentBlockAdapter.FromJson(obj);
    }
}