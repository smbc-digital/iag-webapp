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
using System.Text;
using StockportWebappTests.Unit.Builders;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Medium)]
    public class PaymentController : Controller
    {
        private readonly IProcessedContentRepository _repository;
        private readonly IParisLinkBuilder _parisLinkBuilder;

        public PaymentController(IProcessedContentRepository repository)
        {
            _repository = repository;
            _parisLinkBuilder = new ParisLinkBuilder();
        }

        [Route("/payment/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            var paymentSubmission = new PaymentSubmission();
            paymentSubmission.Payment = payment;

            return View(paymentSubmission);
        }

        [HttpPost]
        [Route("/payment/{slug}")]
        public async Task<IActionResult> Detail(string slug, PaymentSubmission paymentSubmission)
        {
            var response = await _repository.Get<Payment>(slug);

            if (!response.IsSuccessful())
                return response;

            var payment = response.Content as ProcessedPayment;

            paymentSubmission.Payment = payment;

            if (!ModelState.IsValid)
                return View(paymentSubmission);
            else
                return Redirect(CreateParisLink(paymentSubmission));

        }

        public string CreateParisLink(PaymentSubmission paymentSubmission)
        {
            ParisRecordXML xml = new ParisRecordXML() {
                amount = paymentSubmission.Amount.ToString(),
                fund = paymentSubmission.Payment.Fund,
                reference = paymentSubmission.Payment.ParisReference,
                text6 = paymentSubmission.Reference,
                memo = paymentSubmission.Reference
            };

            return _parisLinkBuilder.ReturnText("Return")
                             .IgnoreConfirmation("false")
                             .PayForBasketMode("true")
                             .Data(paymentSubmission.Payment.ParisReference)
                             .ParisRecordXML(xml)
                             .ReturnUrl("https://www.stockport.gov.uk")
                             .Build();
        }
    }
}
