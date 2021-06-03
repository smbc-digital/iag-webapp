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
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Validation;
using StockportWebapp.ViewDetails;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly IRepository _repository;
        private readonly IHttpEmailClient _emailClient;
        private readonly ILogger<ContactUsController> _logger;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly BusinessId _businessId;

        public ContactUsController(IRepository repository, IHttpEmailClient emailClient, ILogger<ContactUsController> logger, IApplicationConfiguration applicationConfiguration, BusinessId businessId)
        {
            _repository = repository;
            _emailClient = emailClient;
            _logger = logger;
            _applicationConfiguration = applicationConfiguration;
            _businessId = businessId;           
        }

        [Route("/contact-us")]
        [HttpPost, IgnoreAntiforgeryToken]
        [ServiceFilter(typeof(ValidateReCaptchaAttribute))]
        public async Task<IActionResult> Contact(ContactUsDetails contactUsDetails)
        {
            var contactUsModel = await GetContactUsId(contactUsDetails.ServiceEmailId);

            contactUsDetails.ServiceEmail = contactUsModel.EmailAddress;

            string redirectUrl;
            if (!string.IsNullOrEmpty(contactUsModel.SuccessPageReturnUrl))
            {
                redirectUrl = contactUsModel.SuccessPageReturnUrl;
            }
            else
            {
                var referer = Request.Headers["referer"];
                if (string.IsNullOrEmpty(referer))
                {
                    return NotFound();
                }
                redirectUrl = new UriBuilder(referer).Path;
            }

            var message = "We have been unable to process the request. Please try again later.";

            if (ModelState.IsValid)
            {
                var successCode = await SendEmailMessage(contactUsDetails);
                if (IsSuccess(successCode))
                {
                    return RedirectToAction("ThankYouMessage", new ThankYouMessageViewModel {
                        ReturnUrl = redirectUrl, ButtonText = contactUsModel.SuccessPageButtonText
                    });
                }
            }
            else
            {
                message = GetErrorsFromModelState(ModelState);
            }

            var toUrl = $"{redirectUrl}?message={message}" + "#error-message-anchor";
            return await Task.FromResult(Redirect(toUrl));
        }

        private async Task<ContactUsId> GetContactUsId(string serviceEmailId)
        {
            var response = await _repository.Get<ContactUsId>(serviceEmailId);

            if (!response.IsSuccessful())
            {
                ModelState.AddModelError(string.Empty, "We are currently having issues sending your inquiry. You can email your message to webcontent@stockport.gov.uk");
            }
            else
            {
                return response.Content as ContactUsId;
            }

            return null;
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
        public async Task<IActionResult> ThankYouMessage(ThankYouMessageViewModel viewModel)
        {
            return await Task.FromResult(View("ThankYouMessage", viewModel));
        }
    }

}