namespace StockportWebapp.Models;

public class ContactUsCategory(string title, string bodyTextLeft, string bodyTextRight, string icon)
{
    public string Title { get; } = title;
    public string BodyTextLeft { get; } = bodyTextLeft;
    public string BodyTextRight { get; } = bodyTextRight;
    public string Icon { get; set; } = icon;
}