using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Config;
using StockportWebapp.Utils;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration=Cache.Short)]
    public class ExternalTemplateController : Controller
    {
        private readonly IViewRender _viewRender;
        private readonly IHtmlUtilities _htmlUtilities;
        private readonly HostHelper _hostHelper;
        private readonly IApplicationConfiguration _config;

        public ExternalTemplateController(IViewRender viewRender, IHtmlUtilities htmlUtilities, HostHelper hostHelper, IApplicationConfiguration config)
        {
            _hostHelper = hostHelper;
            _viewRender = viewRender;
            _htmlUtilities = htmlUtilities;
            _config = config;
        }

        [Route("/ExternalTemplates/Democracy")]
        public IActionResult Democracy()
        {
            var view = _viewRender.Render("ExternalTemplates/Democracy", new {});
            view = _htmlUtilities.ConvertRelativeUrltoAbsolute(view, _config.GetDemocracyHomeLink().ToString());
            return Content(view, "text/html");
        }
    }
}
