namespace StockportWebapp.Models.Groups;

public class GroupBranding
{
    public string Title { get; set; }

    public string Text { get; set; }

    public MediaAsset File { get; set; } = new MediaAsset();

    public string Url { get; set; }

    public GroupBranding(string title, string text, MediaAsset file, string url)
    {
        Title = title;
        Text = text;
        File = file;
        Url = url;
    }
}

public class NullGroupBranding : GroupBranding
{
    public NullGroupBranding() : base(string.Empty, string.Empty, new MediaAsset(), string.Empty) { }
}

