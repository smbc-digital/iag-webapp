namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class SocialMediaLinksViewModel
{
    public string SocialMediaLinksSubheading { get; set; }

    public IEnumerable<SocialMediaLink> SocialMediaLinks { get; set; }
}