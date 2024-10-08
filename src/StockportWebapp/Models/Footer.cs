namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Footer
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public IEnumerable<SubItem> Links { get; set; }
    public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }
    public string FooterContent1 { get; set; }
    public string FooterContent2 { get; set; }
    public string FooterContent3 { get; set; }

    public Footer(string title, string slug, IEnumerable<SubItem> links, IEnumerable<SocialMediaLink> socialMediaLinks, string footerContent1, string footerContent2, string footerContent3)
    {
        Title = title;
        Slug = slug;
        Links = links;
        SocialMediaLinks = socialMediaLinks;
        FooterContent1 = footerContent1;
        FooterContent2 = footerContent2;
        FooterContent3 = footerContent3;
    }
}