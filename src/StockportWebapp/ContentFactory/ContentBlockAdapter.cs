using System.Text.Json;
namespace StockportWebapp.ContentFactory;

public static class ContentBlockAdapter
{
    public static ContentBlock FromJson(JsonElement entry)
    {
        return new ContentBlock()
        {
            Slug = Get(entry, "slug"),
            Title = Get(entry, "title"),
            Teaser = Get(entry, "teaser"),
            Icon = Get(entry, "icon"),
            Type = Get(entry, "type"),
            ContentType = Get(entry, "contentType"),
            Image = Get(entry, "image"),
            MailingListId = Get(entry, "mailingListId"),
            Body = Get(entry, "body"),
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

    public static string Get(JsonElement e, string name)
    {
        if (!e.TryGetProperty(name, out var p))
            return "";

        return p.ValueKind switch
        {
            JsonValueKind.String => p.GetString() ?? "",
            JsonValueKind.Number => p.GetRawText(), // convert numbers to string
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            _ => "" // for Object, Array, Null, Undefined
        };
    }

    private static List<ContentBlock> GetSubItems(JsonElement e)
    {
        if (!e.TryGetProperty("subItems", out var items))
            return new();

        return items.EnumerateArray()
            .Select(FromJson)
            .ToList();
    }

    private static EColourScheme ParseColour(JsonElement e)
    {
        if (!e.TryGetProperty("colourScheme", out var c))
            return EColourScheme.None;

        return Enum.TryParse<EColourScheme>(c.GetString(), true, out var scheme)
            ? scheme
            : EColourScheme.None;
    }
}