namespace StockportWebapp.Models.ProcessedModels
{
    public class ProcessedContactUsCategory : IProcessedContentType
    {
        public readonly string Title;
        public string BodyTextLeft;
        public string BodyTextRight;
        public string Icon;


        public ProcessedContactUsCategory(string title, string bodyTextLeft, string bodyTextRight, string icon)
        {
            Title = title;
            BodyTextLeft = bodyTextLeft;
            BodyTextRight = bodyTextRight;
            Icon = icon;
        }
    }
}


