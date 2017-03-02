using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Models;
using StockportWebapp.ViewDetails;

namespace StockportWebapp.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IHttpEmailClient _emailClient;
        private readonly ILogger<ContactUsController> _logger;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly BusinessId _businessId;

        public ContactUsController(IHttpEmailClient emailClient, ILogger<ContactUsController> logger, IApplicationConfiguration applicationConfiguration, BusinessId businessId)
        {
            _emailClient = emailClient;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
            _businessId = businessId;
        }

        [Route("/contact-us")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactUsDetails contactUsDetails)
        {
            var referer = Request.Headers["referer"];
            if (string.IsNullOrEmpty(referer)) return NotFound();

            var redirectUrl = new UriBuilder(referer).Path;
            var message = "We have been unable to process the request. Please try again later.";

            if (ModelState.IsValid)
            {
                var successCode = await SendEmailMessage(contactUsDetails);
                if (IsSuccess(successCode))
                {
                    return RedirectToAction("ThankYouMessage", routeValues: new { referer = redirectUrl} );
                }
            }
            else
            {
                message = GetErrorsFromModelState(ModelState);
            }

            var toUrl = $"{redirectUrl}?message={message}" + "#error-message-anchor";
            return await Task.FromResult(Redirect(toUrl));
        }

        private string GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            var message = new StringBuilder();

            foreach (var state in modelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    message.Append(state.Value.Errors.First().ErrorMessage + "<br />");
                }
            }
            return message.ToString();
        }

        private bool IsSuccess(HttpStatusCode statusCode)
        {
            return statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.Created;
        }

        private Task<HttpStatusCode> SendEmailMessage(ContactUsDetails contactUsDetails)
        {
            var messageSubject = $"{contactUsDetails.Title} - {contactUsDetails.Subject}";
            _logger.LogDebug("Sending contact us form email");

            var fromEmail = _applicationConfiguration.GetEmailEmailFrom(_businessId.ToString()).IsValid()
                ? _applicationConfiguration.GetEmailEmailFrom(_businessId.ToString()).ToString()
                : string.Empty;

            return _emailClient.SendEmailToService
                (new EmailMessage(messageSubject,
                CreateMessageBody(contactUsDetails),
                fromEmail,
                contactUsDetails.ServiceEmail,
                contactUsDetails.Email,
                new List<IFormFile>()));
        }

        private string CreateMessageBody(ContactUsDetails contactUsDetails)
        {
            return "Thank you for contacting us<br /><br />" +
                   "We have received your message and will get back to you." +
                   "This confirms that we have received your enquiry and a copy of the information received is detailed below:<br /><br />" +
                   $"SENDER : {contactUsDetails.Name}<br />" +
                   $"EMAIL: {contactUsDetails.Email}<br />" +
                   $"SUBJECT: {contactUsDetails.Subject}<br />" +
                   $"MESSAGE: {contactUsDetails.Message}<br /><br />" +
                   $"From page: {Request.Headers["referer"]}<br />";
        }

        [Route("/thank-you")]
        [HttpGet]
        public async Task<IActionResult> ThankYouMessage(string referer)
        {
            return await Task.FromResult(View("ThankYouMessage", referer));
        }
    }
}