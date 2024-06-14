namespace StockportWebapp.ViewComponents;

public class ProfileInlineViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(ProfileViewModel profile, bool withoutBody)
    {
        return await Task.Run(() => View(profile));
    }
}