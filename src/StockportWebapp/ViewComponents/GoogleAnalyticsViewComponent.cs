namespace StockportWebapp.ViewComponents;

public class GoogleAnalyticsViewComponent(IApplicationConfiguration config, BusinessId businessId) : ViewComponent
{
    private readonly IApplicationConfiguration _config = config;
    private readonly BusinessId _businessId = businessId;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        AppSetting googleAnalyticsCode = _config.GetGoogleAnalyticsCode(_businessId.ToString());
        
        return await Task.FromResult(View(model: googleAnalyticsCode));
    }
}