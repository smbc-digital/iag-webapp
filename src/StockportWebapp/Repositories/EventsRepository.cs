using System.Collections.Generic;
using System.Net;
using StockportWebapp.Models;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;

namespace StockportWebapp.Repositories
{
    public interface IEventsRepository
    {
        string GenerateEmailBody(EventSubmission eventSubmission);
        Task<HttpStatusCode> SendEmailMessage(EventSubmission eventSubmission);
    }

    public class EventsRepository : IEventsRepository
    {
        private readonly ILogger<EventsRepository> _logger;
        private readonly IHttpEmailClient _emailClient;
        private readonly IApplicationConfiguration _configuration;
        private readonly BusinessId _businessId;

        public EventsRepository(ILogger<EventsRepository> logger, 
                                IHttpEmailClient emailClient,
                                IApplicationConfiguration configuration,
                                BusinessId businessId )
        {
            _logger = logger;
            _emailClient = emailClient;
            _configuration = configuration;
            _businessId = businessId;

        }

        public string GenerateEmailBody(EventSubmission eventSubmission)
        {
            var stringBuilder = new StringBuilder("<h1>Event Submssion</h1>");
            stringBuilder.Append($"<p>Title: {eventSubmission.Title}<br />");
            stringBuilder.Append($"Teaser: {eventSubmission.Teaser}<br />");
            stringBuilder.Append($"Event Date: {eventSubmission.EventDate:dddd dd MMMM yyyy}<br />");
            stringBuilder.Append($"Start Time: {eventSubmission.StartTime}<br />");
            stringBuilder.Append($"End Time: {eventSubmission.EndTime}<br />");
            stringBuilder.Append($"End Date: {eventSubmission.EndDate}<br />");
            stringBuilder.Append($"Frequency: {eventSubmission.Frequency}<br />");
            stringBuilder.Append($"Fee: {eventSubmission.Fee}<br />");
            stringBuilder.Append($"Location: {eventSubmission.Location}<br />");            
            stringBuilder.Append($"Submitted By: {eventSubmission.SubmittedBy}<br />");
            stringBuilder.Append($"Description: {eventSubmission.Description}<br />");
            if (eventSubmission.Image != null) stringBuilder.Append($"Image: {eventSubmission.Image.FileName}<br />");
            if (eventSubmission.Attachment != null) stringBuilder.Append($"Attachment: {eventSubmission.Attachment.FileName}<br />");
            stringBuilder.Append($"<br />Submitter Email: {eventSubmission.SubmitterEmail}");
            stringBuilder.Append("</p>");

            return stringBuilder.ToString();
        }

        public Task<HttpStatusCode> SendEmailMessage(EventSubmission eventSubmission)
        {
            var messageSubject = $"[Event] - {eventSubmission.Title}";

            _logger.LogInformation("Sending event submission form email");

            var attachments = new List<IFormFile>();
            if (eventSubmission.Image != null) attachments.Add(eventSubmission.Image);
            if (eventSubmission.Attachment != null) attachments.Add(eventSubmission.Attachment);

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBody(eventSubmission),
               _configuration.GetEmailEmailFrom(_businessId.ToString()).ToString(),
               _configuration.GetEventSubmissionEmail(_businessId.ToString()).ToString(),
               attachments));
        }
    }
}
