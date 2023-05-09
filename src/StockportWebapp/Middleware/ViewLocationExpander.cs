namespace StockportWebapp.Middleware;

public class ViewLocationExpander : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        return viewLocations.Select(f => f.Replace("/Views/", $"/Views/{context.Values["theme"]}/"))
           .Append("/Views/Shared/{0}.cshtml")
           .Append("/Views/Shared/{1}/{0}.cshtml");
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
        context.Values["theme"] = context.ActionContext.HttpContext.Request.Headers["BUSINESS-ID"].ToString();
    }
}