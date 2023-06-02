namespace StockportWebapp.ViewComponents;

public class SemanticFooterSocialMediaLinksViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(IEnumerable<SocialMediaLink> model, string additionalClasses)
    {
        return await Task.Run(() => View(new Tuple<IEnumerable<SocialMediaLink>, string>(model, additionalClasses)));
    }
}
