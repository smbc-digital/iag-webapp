namespace StockportWebapp.ViewModels;

public class LandingPageViewModel(LandingPage landingPage)
{
    public readonly LandingPage LandingPage = landingPage;

    public PageHeaderViewModel PageHeader => new()
    {
        Title = LandingPage.Title,
        Subtitle = LandingPage.Subtitle,
        HeaderImageUrl = LandingPage.HeaderImage?.Url,
        HeaderHighlight = LandingPage.PageSections?.FirstOrDefault(pageSection => pageSection.ContentType.Equals("HeaderHighlight")),
        HeaderHighlightType = LandingPage.HeaderHighlightType
    };
}