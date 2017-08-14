using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Helpers;
using StockportWebapp.Utils;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration=Cache.Short)]
    public class ExternalTemplateController : Controller
    {
        private readonly IViewRender _viewRender;
        private readonly IHtmlUtilities _htmlUtilities;
        private readonly HostHelper _hostHelper;

        public ExternalTemplateController(IViewRender viewRender, IHtmlUtilities htmlUtilities, HostHelper hostHelper)
        {
            _hostHelper = hostHelper;
            _viewRender = viewRender;
            _htmlUtilities = htmlUtilities;
        }

        [Route("/ExternalTemplates/Democracy")]
        public IActionResult Democracy()
        {
            var view = _viewRender.Render("ExternalTemplates/Democracy", new {});
            view = _htmlUtilities.ConvertRelativeUrltoAbsolute(view, _hostHelper.GetHost(Request));
            return Content(view, "text/html");
        }
    }
}
