namespace StockportWebapp.ViewComponents;

public class ProfileInlineViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(Profile profile, bool withoutBody)
    {
        return await Task.Run(() => View(profile));
    }
}
