using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Dtos;
using StockportWebapp.QuestionBuilder;
using StockportWebapp.QuestionBuilder.Maps;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace StockportWebapp.Controllers
{
    [Route("building-regs")]
    public class BuildingRegsController : BaseQuestionController<BuildingRegsModel, BuildingRegsMap>
    {

        public BuildingRegsController(IHttpContextAccessor HttpContextAccessor, QuestionLoader questionLoader) : base("building-regs", HttpContextAccessor, questionLoader)
        {

        }

        [HttpGet]
        public IActionResult Index()
        {
            var page = GetPage(0);
            return View(page);
        }

        [Route("unknown-error")]
        public IActionResult FriendlyError()
        {
            return View();
        }

        public override async Task<IActionResult> ProcessResults(BuildingRegsModel result, string endpointName)
        {
            return RedirectToAction("FriendlyError");
        }
    }
}