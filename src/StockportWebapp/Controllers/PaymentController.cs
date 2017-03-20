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

            var paymentSubmission = new PaymentSubmission();
            paymentSubmission.Payment = payment;

            return View(paymentSubmission);
        }

        //[Route("/payment/submit-payment")]
        //public async Task<IActionResult> Detail()
        //{
        //    var paymentSubmission = new PaymentSubmission();
        //    paymentSubmission.Payment = new ProcessedPayment();
        //    return View(paymentSubmission);
        //}

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
            StringBuilder sb = new StringBuilder();

            sb.Append("https://3dsecure.stockport.gov.uk/3dsecureTest/Sales/LaunchInternet.aspx");

            sb.Append("?returntext=Return to main menu");

            //sb.Append(defaultPaymentsPage);

            sb.Append("&ignoreconfirmation=false");

            sb.Append("&payforbasketmode=true");

            sb.Append("&data=BikeabilityContribution");

            sb.Append("&recordxml=<records>");

            sb.Append("<record>");

            sb.Append("<reference>11970700609</reference>");

            sb.Append("<fund>05</fund>");

            sb.Append("<amount>" + paymentSubmission.Amount.ToString() + "</amount>");

            sb.Append("<text6>Council Tax</text6>");

            sb.Append("</record>");

            sb.Append("</records>");

            sb.Append("&returnurl=http://old.stockport.gov.uk/payit/bikeabilitycontribution");

            return sb.ToString();
        }
    }
}
