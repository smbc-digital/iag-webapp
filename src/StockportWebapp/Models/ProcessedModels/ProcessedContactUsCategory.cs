namespace StockportWebapp.Models.ProcessedModels;
[ExcludeFromCodeCoverage]
public class ProcessedContactUsCategory(string title,
                                        string bodyTextLeft,
                                        string bodyTextRight,
                                        string icon) : IProcessedContentType
{
    public readonly string Title = title;
    public string BodyTextLeft = bodyTextLeft;
    public string BodyTextRight = bodyTextRight;
    public string Icon = icon;
}