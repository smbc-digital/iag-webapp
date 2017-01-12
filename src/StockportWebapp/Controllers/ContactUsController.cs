using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using StockportWebapp.AmazonSES;
using StockportWebapp.ViewDetails;

namespace StockportWebapp.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IHttpEmailClient _emailClient;
        private readonly ILogger<ContactUsController> _logger;

        private const string FailureMessage = "We have been unable to process the request. Please try again later.";

        public ContactUsController(IHttpEmailClient emailClient, ILogger<ContactUsController> logger)
        {
            _emailClient = emailClient;
            _logger = logger;
        }

        [Route("/contact-us")]
        [HttpPost]
        public async Task<IActionResult> Contact(ContactUsDetails contactUsDetails)
        {
            var referer = Request.Headers["referer"];
            if (string.IsNullOrEmpty(referer)) return NotFound();

            var redirectUrl = new UriBuilder(referer).Path;
            var message = FailureMessage;

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

            return _emailClient.SendEmailToService
                (messageSubject,
                CreateMessageBody(contactUsDetails),
                contactUsDetails.ServiceEmail,
                contactUsDetails.Email);
        }

        private string CreateMessageBody(ContactUsDetails contactUsDetails)
        {
            return "Thank you for contacting us\n\n"+
                   "We have received your message and will get back to you." +
                   "This confirms that we have received your enquiry and a copy of the information received is detailed below:\n\n" +
                   $"SENDER : {contactUsDetails.Name}\n" +
                   $"EMAIL: {contactUsDetails.Email}\n" +
                   $"SUBJECT: {contactUsDetails.Subject}\n" +
                   $"MESSAGE: {contactUsDetails.Message}\n" +
                   $"\n" +
                   $"From page: {Request.Headers["referer"]}\n";
        }

        [Route("/thank-you")]
        [HttpGet]
        public async Task<IActionResult> ThankYouMessage(string referer)
        {
            return await Task.FromResult(View("ThankYouMessage", referer));
        }
    }
}