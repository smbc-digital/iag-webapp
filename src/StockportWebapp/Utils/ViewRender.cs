namespace StockportWebapp.Utils;

public interface IViewRender
{
    string Render<TModel>(string name, TModel model);
}

public class ViewRender : IViewRender
{
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ViewRender(
        IRazorViewEngine viewEngine,
        ITempDataProvider tempDataProvider,
        IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    public string Render<TModel>(string name, TModel model)
    {
        ActionContext actionContext = GetActionContext();

        Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult viewEngineResult = _viewEngine.FindView(actionContext, name, false);

        if (!viewEngineResult.Success)
            throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));

        Microsoft.AspNetCore.Mvc.ViewEngines.IView view = viewEngineResult.View;

        using (StringWriter output = new())
        {
            ViewContext viewContext = new(
                actionContext,
                view,
                new ViewDataDictionary<TModel>(
                    metadataProvider: new EmptyModelMetadataProvider(),
                    modelState: new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(
                    actionContext.HttpContext,
                    _tempDataProvider),
                output,
                new HtmlHelperOptions());

            view.RenderAsync(viewContext).GetAwaiter().GetResult();

            return output.ToString();
        }
    }

    private ActionContext GetActionContext()
    {
        DefaultHttpContext httpContext = new();
        httpContext.Request.Headers.Add("BUSINESS-ID", _httpContextAccessor.HttpContext.Request.Headers["BUSINESS-ID"]);
        httpContext.RequestServices = _serviceProvider;
        httpContext.Request.Host = _httpContextAccessor.HttpContext.Request.Host;
        httpContext.Request.Scheme = _httpContextAccessor.HttpContext.Request.Scheme;
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
}