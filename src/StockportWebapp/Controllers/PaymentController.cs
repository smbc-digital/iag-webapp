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

        private readonly IRepository _repository;
        private readonly IProcessedContentRepository _processedContentRepository;       
        private readonly ILogger<PaymentController> _logger;
        private readonly IEventsRepository _eventsRepository;
        private readonly IApplicationConfiguration _config;
        private readonly BusinessId _businessId;
        private readonly IFilteredUrl _filteredUrl;
        private readonly FeatureToggles _featureToggles;

        public PaymentController(IRepository repository,
                                IProcessedContentRepository processedContentRepository,
                                IEventsRepository eventsRepository, IRssFeedFactory rssFeedFactory,
                                ILogger<PaymentController> logger, 
                                IApplicationConfiguration config, 
                                BusinessId businessId,
                                IFilteredUrl filteredUrl, 
                                FeatureToggles featureToggles)
        {
            _repository = repository;
            _processedContentRepository = processedContentRepository;
            _eventsRepository = eventsRepository;          
            _logger = logger;
            _config = config;
            _businessId = businessId;
            _filteredUrl = filteredUrl;
            _featureToggles = featureToggles;
        }

        [Route("/payments/{slug}")]
        public async Task<IActionResult> Article(string slug)
        {
            var paymentHttpResponse = await _repository.Get<Payment>(slug);

            if (!paymentHttpResponse.IsSuccessful())
                return paymentHttpResponse;

            var payment = paymentHttpResponse.Content as ProcessedPayment;
            return View(payment);
        }
    }
}
