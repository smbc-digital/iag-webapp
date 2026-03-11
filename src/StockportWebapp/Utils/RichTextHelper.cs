namespace StockportWebapp.Utils;

public interface IRichTextHelper
{
    object RenderNode(JsonElement parent, int index);
}

public class RichTextHelper(IViewRender viewRenderer) : IRichTextHelper
{
    private IViewRender _viewRenderer = viewRenderer;
    private Dictionary<int, string> _columnAlignments = new();

    public object RenderNode(JsonElement parent, int index)
    {
        if (!IsValidIndex(parent, index))
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
            "hr" => RenderHorizontalRule(),
            "table" => RenderTable(node),
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

    private static bool IsValidIndex(JsonElement parent, int index) =>
        parent.ValueKind == JsonValueKind.Array &&
        index >= 0 &&
        index < parent.GetArrayLength();

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
    
    private static string RenderHorizontalRule() =>
        "<hr />";

    #region Tables
    
    private string RenderTable(JsonElement node)
    {
        _columnAlignments.Clear();

        if (!node.TryGetProperty("content", out JsonElement rows) ||
            rows.ValueKind != JsonValueKind.Array)
            return string.Empty;

        StringBuilder sb = new();

        sb.Append("<div class=\"table\"><table>");

        bool headerProcessed = false;
        bool tbodyOpened = false;

        for (int i = 0; i < rows.GetArrayLength(); i++)
        {
            JsonElement row = rows[i];

            if (!headerProcessed && IsHeaderRow(row))
            {
                sb.Append("<thead>");
                sb.Append(RenderTableRow(row, true));
                sb.Append("</thead>");
                headerProcessed = true;
                continue;
            }

            if (!tbodyOpened)
            {
                sb.Append("<tbody>");
                tbodyOpened = true;
            }

            sb.Append(RenderTableRow(row, false));
        }

        if (tbodyOpened)
            sb.Append("</tbody>");

        sb.Append("</table></div>");

        return sb.ToString();
    }

    private string RenderTableRow(JsonElement node, bool isHeader)
    {
        if (!node.TryGetProperty("content", out JsonElement cells))
            return "<tr></tr>";

        StringBuilder sb = new();

        sb.Append("<tr>");

        for (int i = 0; i < cells.GetArrayLength(); i++)
        {
            JsonElement cell = cells[i];
            string nodeType = cell.GetStringOrDefault("nodeType");

            string tag = nodeType.Equals("table-header-cell")
                ? "th"
                : "td";

            sb.Append(RenderTableCell(cell, tag, i, isHeader));
        }

        sb.Append("</tr>");

        return sb.ToString();
    }

    private string RenderTableCell(JsonElement node, string tag, int columnIndex, bool isHeader)
    {
        string content = RenderChildren(node);

        content = StripParagraphWrapper(content);

        if (isHeader)
        {
            if (content.Contains(">>"))
            {
                _columnAlignments[columnIndex] = "text-right";
                content = content.Replace(">>", string.Empty);
            }
            else if (content.Contains("<<"))
            {
                _columnAlignments[columnIndex] = "text-left";
                content = content.Replace("<<", string.Empty);
            }
            else if (content.Contains("=="))
            {
                _columnAlignments[columnIndex] = "text-center";
                content = content.Replace("==", string.Empty);
            }
        }

        string alignmentClass = _columnAlignments.ContainsKey(columnIndex)
            ? _columnAlignments[columnIndex]
            : "text-left";

        return $"<{tag} class=\"{alignmentClass}\">{content}</{tag}>";
    }

    private static string StripParagraphWrapper(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        html = html.Trim();

        if (html.StartsWith("<p>") && html.EndsWith("</p>"))
            return html.Substring(3, html.Length - 7);

        return html;
    }

    private static bool IsHeaderRow(JsonElement row)
    {
        if (!row.TryGetProperty("content", out JsonElement cells) ||
            cells.ValueKind != JsonValueKind.Array)
            return false;

        foreach (JsonElement cell in cells.EnumerateArray())
        {
            string type = cell.GetStringOrDefault("nodeType");

            if (!type.Equals("table-header-cell"))
                return false;
        }

        return true;
    }

    #endregion

    #region Text

    private static string RenderText(JsonElement node)
    {
        string text = node.GetStringOrDefault("value");
        JsonElement marks = node.GetPropertyOrDefault("marks");

        if (marks.ValueKind != JsonValueKind.Array || marks.GetArrayLength() == 0)
            return text;

        foreach (JsonElement mark in marks.EnumerateArray())
        {
            string type = mark.GetStringOrDefault("type");

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
        string uri = node.GetPropertyOrDefault("data").GetStringOrDefault("uri", "#");

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
        JsonElement obj = node
            .GetPropertyOrDefault("data")
            .GetPropertyOrDefault("target")
            .GetPropertyOrDefault("jObject");

        if (obj.ValueKind == JsonValueKind.Undefined)
            return string.Empty;

        if (IsContentType(obj, "alert"))
            return RenderAlert(obj);

        if (IsContentType(obj, "quote"))
            return RenderQuote(obj);
        
        ContentBlock contentBlock = GetEmbeddedContentBlock(node);
        if (contentBlock is null || string.IsNullOrEmpty(contentBlock.ContentType))
            return string.Empty;

        return new EmbeddedPartial(contentBlock);
    }

    private static string RenderInlineEntry(JsonElement node)
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

        return obj.ValueKind == JsonValueKind.Undefined
            ? null
            : ContentBlockAdapter.FromJson(obj);
    }

    private static bool IsContentType(JsonElement obj, string type) =>
        obj.GetPropertyOrDefault("sys")
           .GetPropertyOrDefault("contentType")
           .GetPropertyOrDefault("sys")
           .GetStringOrDefault("id")
           .Equals(type, StringComparison.OrdinalIgnoreCase);

    private string RenderAlert(JsonElement obj)
    {
        string title = obj.GetStringOrDefault("title");
        string body = obj.GetStringOrDefault("body");
        string severity = obj.GetStringOrDefault("severity", Severity.Information);
        
        DateTime sunrise = obj.TryGetProperty("sunriseDate", out JsonElement sunriseProp) &&
                        sunriseProp.TryGetDateTime(out DateTime sunriseDt)
            ? sunriseDt
            : DateTime.MinValue;

        DateTime sunset = obj.TryGetProperty("sunsetDate", out JsonElement sunsetProp) &&
                        sunsetProp.TryGetDateTime(out DateTime sunsetDt)
            ? sunsetDt
            : DateTime.MaxValue;

        string slug  = obj.GetStringOrDefault("slug");

        bool isStatic = obj.TryGetProperty("isStatic", out JsonElement staticProp) &&
                        staticProp.ValueKind == JsonValueKind.True;

        string imageUrl = obj.GetStringOrDefault("imageUrl");

        Alert alert = new(title, body, severity, sunrise, sunset, slug, isStatic, imageUrl);

        if (severity.Equals(Severity.Warning) || severity.Equals(Severity.Error))
            return _viewRenderer.Render("AlertsInlineWarning", alert);

        return _viewRenderer.Render("AlertsInline", alert);
    }

    private string RenderQuote(JsonElement obj)
    {
        string image = GetNestedString(obj, "image", "fields", "file", "url");
        string imageAlt = obj.GetStringOrDefault("imageAltText");
        string quote = obj.GetStringOrDefault("quote");
        string author = obj.GetStringOrDefault("author");
        string slug = obj.GetStringOrDefault("slug");

        EColourScheme theme = EColourScheme.Teal;
        if (obj.TryGetProperty("theme", out JsonElement themeProp) &&
            themeProp.ValueKind == JsonValueKind.String)
            Enum.TryParse(themeProp.GetString(), true, out theme);

        InlineQuote model = new(image, imageAlt, quote, author, slug, theme);

        return _viewRenderer.Render("InlineQuote", model);
    }

    private static string GetNestedString(JsonElement obj, params string[] path)
    {
        JsonElement current = obj;

        foreach (string segment in path)
        {
            if (!current.TryGetProperty(segment, out current))
                return string.Empty;
        }

        return current.ValueKind == JsonValueKind.String
            ? current.GetString() ?? string.Empty
            : string.Empty;
    }
    
    #endregion

    #region Assets

    private static string RenderEmbeddedAsset(JsonElement parent, int index)
    {
        JsonElement node = parent[index];
        JsonElement target = node.GetPropertyOrDefault("data").GetPropertyOrDefault("target");

        string url = target.GetPropertyOrDefault("file").GetStringOrDefault("url", "#");
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
            return $"<img src=\"{url}?q=89&fm=webp\" alt=\"{alt}\" {imgClass} srcset=\"{srcset}\" sizes=\"{sizes}\" loading=\"lazy\" />";

        StringBuilder stringBuilder = new();

        stringBuilder.Append($@"<figure class=""{floatClass}"">");
        stringBuilder.Append($@"<p><img src=""{url}?q=89&fm=webp"" alt=""{alt}"" {imgClass} srcset=""{srcset}"" sizes=""{sizes}"" loading=""lazy"" /></p>");

        if (!string.IsNullOrEmpty(caption))
            stringBuilder.Append($"<figcaption>{caption}</figcaption>");

        stringBuilder.Append("</figure>");

        return stringBuilder.ToString();
    }

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
        if (!node.GetStringOrDefault("nodeType").Equals("paragraph"))
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
        if (!node.GetStringOrDefault("nodeType").Equals("paragraph"))
            return null;
        
        JsonElement content = node.GetPropertyOrDefault("content");
        if (content.GetArrayLength().Equals(0))
            return null;

        StringBuilder textBuilder = new();

        foreach (JsonElement item in content.EnumerateArray())
        {
            if (item.GetStringOrDefault("nodeType").Equals("text"))
                textBuilder.Append(item.GetStringOrDefault("value"));
        }

        string value = textBuilder.ToString();

        if (!value.StartsWith("^^^"))
            return null;
        
        if (value.StartsWith("^^^#left") || value.StartsWith("^^^#right"))
            return null;
        
        string caption = value.Substring(3).Trim();

        return string.IsNullOrWhiteSpace(caption)
            ? null
            : caption;
    }

    #endregion
}

internal static class JsonExtensions
{
    public static JsonElement GetPropertyOrDefault(this JsonElement element, string name)
    {
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty(name, out JsonElement value))
            return value;

        return default;
    }

    public static string GetStringOrDefault(this JsonElement element, string propertyName, string defaultValue = "")
    {
        if (element.ValueKind == JsonValueKind.Object &&
            element.TryGetProperty(propertyName, out JsonElement value) &&
            value.ValueKind == JsonValueKind.String)
            return value.GetString() ?? defaultValue;

        return defaultValue;
    }
}