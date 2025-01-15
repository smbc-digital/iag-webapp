namespace StockportWebapp.ViewComponents;

public class ProfileInlineViewComponent : ViewComponent
{
    [ExcludeFromCodeCoverage]
    public async Task<IViewComponentResult> InvokeAsync(ProfileViewModel profile) =>
        await Task.Run(() => View(profile));
}