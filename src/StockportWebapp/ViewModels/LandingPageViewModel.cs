namespace StockportWebapp.ViewModels;

public class LandingPageViewModel
{
    public readonly LandingPage LandingPage;

    public LandingPageViewModel(LandingPage landingPage)
    {
        LandingPage = landingPage;
    }

    public bool ScreenWidth => LandingPage.ContentBlocks.Any(contentBlock => contentBlock is not null && contentBlock.ContentType.Contains("ScreenWidth"));
}