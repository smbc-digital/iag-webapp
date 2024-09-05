namespace StockportWebapp.ViewModels;

public class LandingPageViewModel
{
    public readonly LandingPage LandingPage;

    public LandingPageViewModel(LandingPage landingPage)
    {
        LandingPage = landingPage;
    }

    public bool ScreenWidth => LandingPage.PageSections.Any(pageSection => pageSection is not null && pageSection.ContentType.Contains("ScreenWidth"));
}