namespace StockportWebapp.ViewComponents;

public class ProfileInlineViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(Models.Profile profile, bool withoutBody)
    {
        return await Task.Run(() => View(profile));
    }
}
