namespace StockportWebapp.Models
{
    public class InformationItem
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public InformationItem(string name, string icon, string text, string link)
        {
            Name = name;
            Icon = icon;
            Text = text;
            Link = link;
        }
    }
}
