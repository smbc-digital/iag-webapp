namespace StockportWebapp.Utils;

public interface IRichTextHelper
{
    object Render(JsonElement node);
}

public class RichTextHelper(IViewRender viewRenderer) : IRichTextHelper
{
    private IViewRender _viewRenderer = viewRenderer;

    public object Render(JsonElement node)
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

    private string RenderChildren(JsonElement node)
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

    private string RenderText(JsonElement node)
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

    private string RenderList(JsonElement node, string tag) =>
        $"<{tag}>{RenderChildren(node)}</{tag}>";

    private string RenderListItem(JsonElement node) =>
        $"<li>{RenderChildren(node)}</li>";

    private string RenderHyperlink(JsonElement node)
    {
        string uri = node.GetProperty("data").GetProperty("uri").GetString() ?? "#";

        return $"<a href='{uri}'>{RenderChildren(node)}</a>";
    }

    private object RenderEmbeddedEntry(JsonElement node)
    {
        if (!node.TryGetProperty("data", out var data) ||
            !data.TryGetProperty("target", out var target) ||
            !target.TryGetProperty("jObject", out var obj))
            return string.Empty;

        if (IsContentType(obj, "alert"))
            return RenderAlert(obj);

        if (IsContentType(obj, "quote"))
            return RenderQuote(obj);

        var block = ContentBlockAdapter.FromJson(obj);
        return new EmbeddedPartial(block);
    }

    private string GetNestedString(JsonElement obj, params string[] path)
    {
        JsonElement current = obj;

        foreach (var segment in path)
        {
            if (!current.TryGetProperty(segment, out current))
                return string.Empty;
        }

        return current.ValueKind == JsonValueKind.String
            ? current.GetString() ?? string.Empty
            : string.Empty;
    }

    private string RenderQuote(JsonElement obj)
    {
        string image = GetNestedString(obj, "image", "fields", "file", "url");
        string imageAlt = GetSafeString(obj, "imageAltText");
        string quote = GetSafeString(obj, "quote");
        string author = GetSafeString(obj, "author");
        string slug = GetSafeString(obj, "slug");

        EColourScheme theme = EColourScheme.Teal;
        if (obj.TryGetProperty("theme", out var themeProp) &&
            themeProp.ValueKind == JsonValueKind.String)
        {
            Enum.TryParse(themeProp.GetString(), true, out theme);
        }

        InlineQuote model = new(image, imageAlt, quote, author, slug, theme);

        return _viewRenderer.Render("InlineQuote", model);
    }

    private string GetSafeString(JsonElement obj, string propertyName)
    {
        if (obj.TryGetProperty(propertyName, out var prop) &&
            prop.ValueKind == JsonValueKind.String)
        {
            return prop.GetString() ?? string.Empty;
        }

        return string.Empty;
    }

    private bool IsContentType(JsonElement obj, string type) =>
        obj.TryGetProperty("sys", out var sys) &&
        sys.TryGetProperty("contentType", out var ct) &&
        ct.TryGetProperty("sys", out var ctSys) &&
        ctSys.TryGetProperty("id", out var idProp) &&
        idProp.GetString()?.Equals(type, StringComparison.OrdinalIgnoreCase) == true;

    private string RenderAlert(JsonElement obj)
    {
        string title = obj.TryGetProperty("title", out var titleProp)
            ? titleProp.GetString() ?? string.Empty
            : string.Empty;

        string body = obj.TryGetProperty("body", out var bodyProp)
            ? bodyProp.GetString() ?? string.Empty
            : string.Empty;

        string severity = obj.TryGetProperty("severity", out var sevProp)
            ? sevProp.GetString() ?? Severity.Information
            : Severity.Information;

        DateTime sunrise = obj.TryGetProperty("sunriseDate", out var sunriseProp) &&
                        sunriseProp.TryGetDateTime(out var sunriseDt)
            ? sunriseDt
            : DateTime.MinValue;

        DateTime sunset = obj.TryGetProperty("sunsetDate", out var sunsetProp) &&
                        sunsetProp.TryGetDateTime(out var sunsetDt)
            ? sunsetDt
            : DateTime.MaxValue;

        string slug = obj.TryGetProperty("slug", out var slugProp)
            ? slugProp.GetString() ?? string.Empty
            : string.Empty;

        bool isStatic = obj.TryGetProperty("isStatic", out var staticProp) &&
                        staticProp.ValueKind == JsonValueKind.True;

        string imageUrl = obj.TryGetProperty("imageUrl", out var imgProp)
            ? imgProp.GetString() ?? string.Empty
            : string.Empty;

        var alert = new Alert(title, body, severity, sunrise, sunset, slug, isStatic, imageUrl);

        if (severity.Equals(Severity.Warning) || severity.Equals(Severity.Error))
            return _viewRenderer.Render("AlertsInlineWarning", alert);

        return _viewRenderer.Render("AlertsInline", alert);
    }

    private string RenderInlineEntry(JsonElement node)
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

    private string RenderEmbeddedAsset(JsonElement node)
    {
        JsonElement target = node.GetProperty("data").GetProperty("target");
        string url = GetAssetUrl(target);
        string alt = target.TryGetProperty("description", out JsonElement desc)
            ? desc.GetString()
            : string.Empty;
        
        return $"<img src='{url}' alt='{alt}' />";
    }

   private string RenderAssetHyperlink(JsonElement node)
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

    private string RenderEntryHyperlink(JsonElement node)
    {
        ContentBlock contentBlock = GetEmbeddedContentBlock(node);
        if (contentBlock is null)
            return RenderChildren(node);
        
        string text = RenderChildren(node);

        return $"<a href='/{contentBlock.Slug}'>{text}</a>";
    }

    private string GetAssetUrl(JsonElement target)
    {
        if (target.TryGetProperty("file", out JsonElement file)
            && file.TryGetProperty("url", out JsonElement urlProp)
            && urlProp.ValueKind == JsonValueKind.String)
            return urlProp.GetString() ?? "#";

        return "#";
    }

    public ContentBlock GetEmbeddedContentBlock(JsonElement node)
    {
        if (!node.TryGetProperty("data", out JsonElement data))
            return null;

        if (!data.TryGetProperty("target", out JsonElement target))
            return null;
        
        if (!target.TryGetProperty("jObject", out JsonElement obj))
            return null;

        return ContentBlockAdapter.FromJson(obj);
    }
}