namespace StockportWebapp.ContentFactory;

public static class ContentBlockAdapter
{
    public static ContentBlock FromJson(JsonElement entry)
    {
        return new ContentBlock
        {
            Slug = Get(entry, "slug"),
            Title = Get(entry, "title"),
            Teaser = Get(entry, "teaser"),
            Icon = Get(entry, "icon"),
            Type = Get(entry, "type"),
            ContentType = Get(entry, "contentType"),
            Image = GetImage(entry),
            MailingListId = Get(entry, "mailingListId"),
            Body = MarkdownWrapper.ToHtml(Get(entry, "body")),
            SubItems = GetSubItems(entry),
            Link = Get(entry, "link"),
            ButtonText = Get(entry, "buttonText"),
            ColourScheme = ParseColour(entry),
            Statistic = Get(entry, "statistic"),
            StatisticSubheading = Get(entry, "statisticSubheading"),
            VideoTitle = Get(entry, "videoTitle"),
            VideoToken = Get(entry, "videoToken"),
            VideoPlaceholderPhotoId = Get(entry, "videoPlaceholderPhotoId"),
            AssociatedTagCategory = Get(entry, "associatedTagCategory"),
            NewsArticle = null,
            Events = new List<Event>(),
            News = new List<News>(),
            ScreenReader = Get(entry, "screenReader"),
            AccountName = Get(entry, "accountName")
        };
    }

    public static string Get(JsonElement element, string name)
    {
        // direct match
        if (element.TryGetProperty(name, out JsonElement property))
            return Extract(property);

        // case-insensitive match
        foreach (var prop in element.EnumerateObject())
        {
            if (string.Equals(prop.Name, name, StringComparison.OrdinalIgnoreCase))
                return Extract(prop.Value);
        }

        // nested inside "fields"
        if (element.TryGetProperty("fields", out JsonElement fields))
        {
            foreach (var prop in fields.EnumerateObject())
            {
                if (string.Equals(prop.Name, name, StringComparison.OrdinalIgnoreCase))
                    return Extract(prop.Value);
            }
        }

        return string.Empty;
    }

    private static string Extract(JsonElement property)
    {
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

        if (element.TryGetProperty("colour", out JsonElement colour2))
        {
            if (Enum.TryParse<EColourScheme>(colour2.GetString(), true, out var scheme2))
                return scheme2;
        }

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