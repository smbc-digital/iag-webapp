﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StockportWebapp.Dtos;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.QuestionBuilder;
using StockportWebapp.QuestionBuilder.Maps;
using StockportWebapp.Services;
using StockportWebapp.ViewModels;
using StockportWebapp.Utils;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    [Route("smart/{slug}")]
    public class SmartAnswersController : BaseQuestionController<GenericSmartAnswersModel, GenericSmartAnswersMap>
    {
        private readonly ISmartResultService _service;
        private readonly FeatureToggles _featuretogles;

        public SmartAnswersController(IHttpContextAccessor HttpContextAccessor, QuestionLoader questionLoader, FeatureToggles FeatureToggles, ISmartResultService service, IHttpClient _client, IConfiguration _config, ILogger<BaseQuestionController<GenericSmartAnswersModel, GenericSmartAnswersMap>> logger, ISmartAnswerStringHelper smartAnswerStringHelper) : base(HttpContextAccessor, questionLoader, FeatureToggles, _client, _config, logger, smartAnswerStringHelper)
        {
            _service = service;
            _featuretogles = FeatureToggles;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (Structure == null) return NotFound();

            var page = GetPage(0);

            var result = new SmartAnswerViewModel { Page = page, Slug = Slug, Title = Title, TypeformUrl = page.TypeformUrl};

            //var typeformUrl = 

            if (_featuretogles.SemanticLayout && _featuretogles.SemanticSmartAnswer.Contains(result.Slug))
            {
                return View("Semantic/Index", result);
            }

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
        
        [Route("{resultSlug}")]
        public async Task<IActionResult> Result(string resultSlug)
        {
            var entity = await _service.GetSmartResult(resultSlug);
            var model = new ConfirmationViewModel()
            {
                ButtonLink = entity.ButtonLink,
                Icon = entity.IconClass,
                IconColour = entity.IconColour,
                ButtonText = entity.ButtonText,
                Title = entity.Title,
                SubTitle = entity.Subheading,
                ConfirmationText = entity.Body,
                Crumbs = new List<Crumb>()  
            };
            return View("Confirmation", model);
        }

        public override IActionResult ProcessResults(GenericSmartAnswersModel result, string endpointName)
        {
            return RedirectToAction("FriendlyError");
        }
    }
}