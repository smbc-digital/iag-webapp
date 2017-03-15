using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz.Util;
using StockportWebapp.Config;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.RSS;
using StockportWebapp.Utils;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class PaymentController : Controller
    {

        private readonly IProcessedContentRepository _repository;

        public PaymentController(IProcessedContentRepository repository)
        {
            _repository = repository;
        }

        [Route("/payment/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;
            return View(payment);
        }
    }
}
