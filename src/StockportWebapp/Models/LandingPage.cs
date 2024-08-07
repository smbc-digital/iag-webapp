namespace StockportWebapp.Models;
[ExcludeFromCodeCoverage]
public class LandingPage
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public IEnumerable<Alert> Alerts { get; set; }
    public string Teaser { get; set; }
    public string MetaDescription { get; set; }
    public string Image { get; set; }
    public string HeaderType { get; set; }
    public string HeaderImage { get; set; }
    public IEnumerable<SubItem> ContentBlocks { get; set; }

    public LandingPage()
    {
    }
}