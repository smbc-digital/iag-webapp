using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class SubItem
    {
        public readonly string Title;
        public readonly string Icon;
        public readonly string Teaser;
        public readonly string NavigationLink;
        public readonly string Image;

        public SubItem(string slug, string title, string teaser, string icon, string type, string image)
        {
            Title = title;
            Icon = icon;
            Teaser = teaser;
            NavigationLink = TypeRoutes.GetUrlFor(type, slug);
            Image = image;
        }
    }
}