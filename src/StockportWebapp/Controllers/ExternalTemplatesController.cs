using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Utils;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.ArticleStartPageNewsDuration)]
    public class ExternalTemplateController : Controller
    {
        private readonly IViewRender _viewRender;
        private readonly IHtmlUtilities _htmlUtilities;

        public ExternalTemplateController(IViewRender viewRender, IHtmlUtilities htmlUtilities)
        {
            _viewRender = viewRender;
            _htmlUtilities = htmlUtilities;
        }

        [Route("/ExternalTemplates/Democracy")]
        public IActionResult Democracy()
        {
            var view = _viewRender.Render("ExternalTemplates/Democracy", new {});
            view = _htmlUtilities.ConvertRelativeUrltoAbsolute(view, "https://www.stockport.gov.uk");
            return Content(view, "text/html");
        }
    }
}
