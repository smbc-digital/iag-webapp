using Microsoft.AspNetCore.Mvc;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class ExternalTemplatesController : Controller
    {
        [Route("/ExternalTemplates/Democracy")]
        public IActionResult Democracy()
        {
            return View();
        }
    }
}
