using System.Collections.Generic;

namespace StockportWebapp.Models
{
    public class Footer
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Copyright { get; set; }
        public IEnumerable<SubItem> Links { get; set; }
        public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }

        public Footer(string title, string slug, string copyright, IEnumerable<SubItem> links, IEnumerable<SocialMediaLink> socialMediaLinks)
        {
            Title = title;
            Slug = slug;
            Copyright = copyright;
            Links = links;
            SocialMediaLinks = socialMediaLinks;
        }
    }   
}
