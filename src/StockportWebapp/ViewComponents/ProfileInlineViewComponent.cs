namespace StockportWebapp.ViewComponents;

public class ProfileInlineViewComponent : ViewComponent
{
    [ExcludeFromCodeCoverage]
    public async Task<IViewComponentResult> InvokeAsync(ProfileViewModel profile, bool withoutBody) =>
        await Task.Run(() => View(profile));
}