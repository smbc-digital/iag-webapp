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
    [Route("building-regs-garage")]
    public class BuildingRegsGarageController : BaseQuestionController<BuildingRegsGarageModel, BuildingRegsGarageMap>
    {

        public BuildingRegsGarageController(IBuildingRegsGarageQuestions questionStructure, IHttpContextAccessor HttpContextAccessor) : base(questionStructure.Structure, HttpContextAccessor)
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

        public override async Task<IActionResult> ProcessResults(BuildingRegsGarageModel result, string endpointName)
        {
            return RedirectToAction("FriendlyError");
        }
    }
}