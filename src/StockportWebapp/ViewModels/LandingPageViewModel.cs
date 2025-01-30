namespace StockportWebapp.ViewModels;

public class LandingPageViewModel(LandingPage landingPage)
{
    public readonly LandingPage LandingPage = landingPage;

    public bool ScreenWidth =>
        LandingPage.PageSections.Any(pageSection => pageSection is not null && pageSection.ContentType.Contains("ScreenWidth"));
}