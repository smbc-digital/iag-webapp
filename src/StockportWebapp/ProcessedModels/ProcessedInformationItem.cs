using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedInformationItem : IProcessedContentType
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public ProcessedInformationItem(string name, string icon, string text, string link)
        {
            Name = name;
            Icon = icon;
            Text = text;
            Link = link;
        }
    }
}
