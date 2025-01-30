namespace StockportWebapp.Utils;

public interface IViewRender
{
    string Render<TModel>(string name, TModel model);
}

[ExcludeFromCodeCoverage]
public class ViewRender(IRazorViewEngine viewEngine,
                        ITempDataProvider tempDataProvider,
                        IServiceProvider serviceProvider,
                        IHttpContextAccessor httpContextAccessor) : IViewRender
{
    private readonly IRazorViewEngine _viewEngine = viewEngine;
    private readonly ITempDataProvider _tempDataProvider = tempDataProvider;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string Render<TModel>(string name, TModel model)
    {
        ActionContext actionContext = GetActionContext();

        Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult viewEngineResult = _viewEngine.FindView(actionContext, name, false);

        if (!viewEngineResult.Success)
            throw new InvalidOperationException($"Couldn't find view '{name}'");

        Microsoft.AspNetCore.Mvc.ViewEngines.IView view = viewEngineResult.View;

        using StringWriter output = new();
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

    private ActionContext GetActionContext()
    {
        DefaultHttpContext httpContext = new();
        httpContext.Request.Headers.Append("BUSINESS-ID", _httpContextAccessor.HttpContext.Request.Headers["BUSINESS-ID"]);
        httpContext.RequestServices = _serviceProvider;
        httpContext.Request.Host = _httpContextAccessor.HttpContext.Request.Host;
        httpContext.Request.Scheme = _httpContextAccessor.HttpContext.Request.Scheme;
        
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
}