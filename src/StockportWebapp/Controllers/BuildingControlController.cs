using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Dtos;
using StockportWebapp.FeatureToggling;
using StockportWebapp.QuestionBuilder;
using StockportWebapp.QuestionBuilder.Maps;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace StockportWebapp.Controllers
{
    [Route("building-control-application-selection")]
    public class BuildingControlController : BaseQuestionController<BuildingRegsModel, BuildingRegsMap>
    {
        public FeatureToggles _FeatureToggling;
        public BuildingControlController(IHttpContextAccessor HttpContextAccessor, QuestionLoader questionLoader, FeatureToggles FeatureToggling) : base("building-control-application-selection", HttpContextAccessor, questionLoader)
        {
            _FeatureToggling = FeatureToggling;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!_FeatureToggling.SmartAnswers) return Redirect("FriendlyError");

            var page = GetPage(0);
            return View(page);
        }

        [Route("unknown-error")]
        public IActionResult FriendlyError()
        {
            return RedirectToAction("Error", new { id = 404 });
        }

        public override async Task<IActionResult> ProcessResults(BuildingRegsModel result, string endpointName)
        {
            return RedirectToAction("FriendlyError");
        }
    }
}