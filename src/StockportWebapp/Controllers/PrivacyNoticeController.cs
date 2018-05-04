using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Repositories;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Http;

namespace StockportWebapp.Controllers
{
    public class PrivacyNoticeController : Controller
    {
        private readonly IProcessedContentRepository _repository;

        public PrivacyNoticeController(IProcessedContentRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("/privacy-notice/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var result = await _repository.Get<PrivacyNotice>(slug);

            if (!result.IsSuccessful())
                return result;

            var privacyNotice = result.Content as ProcessedPrivacyNotice;

            return View(privacyNotice);
        }
    }
}
