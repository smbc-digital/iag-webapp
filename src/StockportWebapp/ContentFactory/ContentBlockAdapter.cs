namespace StockportWebapp.ContentFactory;

public static class ContentBlockAdapter
{
    public static ContentBlock FromJson(JsonElement entry)
    {
        return new ContentBlock(
            Get(entry, "slug"),
            Get(entry, "title"),
            Get(entry, "teaser"),
            Get(entry, "icon"),
            Get(entry, "type"),
            Get(entry, "contentType"),
            GetImage(entry),
            Get(entry, "mailingListId"),
            Get(entry, "body"),
            GetSubItems(entry),
            Get(entry, "link"),
            Get(entry, "buttonText"),
            ParseColour(entry),
            Get(entry, "statistic"),
            Get(entry, "statisticSubheading"),
            Get(entry, "videoTitle"),
            Get(entry, "videoToken"),
            Get(entry, "videoPlaceholderPhotoId"),
            Get(entry, "associatedTagCategory"),
            null,
            new List<Event>(),
            new List<News>(),
            Get(entry, "screenReader"),
            Get(entry, "accountName"));
    }

    public static string Get(JsonElement element, string name)
    {
        if (!element.TryGetProperty(name, out JsonElement property))
            return string.Empty;

        return property.ValueKind switch
        {
            JsonValueKind.String => property.GetString() ?? string.Empty,
            JsonValueKind.Number => property.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            _ => string.Empty
        };
    }

    private static List<ContentBlock> GetSubItems(JsonElement element)
    {
        if (!element.TryGetProperty("subItems", out JsonElement items))
            return new();

        return items.EnumerateArray()
            .Select(FromJson)
            .ToList();
    }

    private static EColourScheme ParseColour(JsonElement element)
    {
        if (!element.TryGetProperty("colourScheme", out JsonElement colour))
            return EColourScheme.None;

        return Enum.TryParse<EColourScheme>(colour.GetString(), true, out var scheme)
            ? scheme
            : EColourScheme.None;
    }

    private static string GetImage(JsonElement element)
    {
        if (!element.TryGetProperty("image", out JsonElement img))
            return string.Empty;

        if (img.TryGetProperty("sys", out JsonElement sys) &&
            sys.TryGetProperty("id", out JsonElement id))
            return id.GetString();

        return string.Empty;
    }
}