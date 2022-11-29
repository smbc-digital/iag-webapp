namespace StockportWebapp.Models
{
    public class Footer
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public IEnumerable<SubItem> Links { get; set; }
        public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }

        public Footer(string title, string slug, IEnumerable<SubItem> links, IEnumerable<SocialMediaLink> socialMediaLinks)
        {
            Title = title;
            Slug = slug;
            Links = links;
            SocialMediaLinks = socialMediaLinks;
        }
    }
}
