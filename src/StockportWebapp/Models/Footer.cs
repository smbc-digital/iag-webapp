namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Footer(string title,
                    string slug,
                    IEnumerable<SubItem> links,
                    IEnumerable<SocialMediaLink> socialMediaLinks,
                    string footerContent1,
                    string footerContent2,
                    string footerContent3)
{
    public string Title { get; set; } = title;
    public string Slug { get; set; } = slug;
    public IEnumerable<SubItem> Links { get; set; } = links;
    public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; } = socialMediaLinks;
    public string FooterContent1 { get; set; } = footerContent1;
    public string FooterContent2 { get; set; } = footerContent2;
    public string FooterContent3 { get; set; } = footerContent3;
}