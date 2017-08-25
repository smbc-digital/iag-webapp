using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockportWebapp.Dtos;
using StockportWebapp.FeatureToggling;
using StockportWebapp.QuestionBuilder;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Maps;
using StockportWebapp.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    [Route("smart/{slug}")]
    public class SmartAnswersController : BaseQuestionController<GenericSmartAnswersModel, GenericSmartAnswersMap>
    {
        public FeatureToggles _FeatureToggling;
        public SmartAnswersController(IHttpContextAccessor HttpContextAccessor, QuestionLoader questionLoader, FeatureToggles FeatureToggling) : base(HttpContextAccessor, questionLoader)
        {
            _FeatureToggling = FeatureToggling;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Structure == null) return NotFound();

            if (!_FeatureToggling.SmartAnswers) return NotFound();

            var page = GetPage(0);

            var result = new SmartAnswerViewModel { Page = page, Slug = Slug, Title = Title };
            return View(result);
        }

        [Route("unknown-error")]
        public IActionResult FriendlyError()
        {
            return NotFound();
        }

        [Route("summary")]
        public IActionResult Summary(string previousAnswersjson)
        {
            var model = new SmartAnswerSummaryViewModel();
            ViewBag.Title = "test";
            return View(model);
        }

        public override async Task<IActionResult> ProcessResults(GenericSmartAnswersModel result, string endpointName)
        {
            return RedirectToAction("FriendlyError");
        }
    }
}