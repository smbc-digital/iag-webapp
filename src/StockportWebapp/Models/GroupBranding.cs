namespace StockportWebapp.Models
{
    public class GroupBranding
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string File { get; set; }

        public string FileDescription { get; set; }

        public string Url { get; set; }

        public GroupBranding(string title, string text, string file, string fileDescription, string url)
        {
            Title = title;
            Text = text;
            File = file;
            FileDescription = fileDescription;
            Url = url;
        }
    }

    public class NullGroupBranding : GroupBranding
    {
        public NullGroupBranding() : base(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty) { }
    }
}

