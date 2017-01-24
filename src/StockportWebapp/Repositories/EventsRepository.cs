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
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<p>Thank you for submitting an event</p>");
            stringBuilder.Append("<p>Before your event is added to the calendar, it will be reviewed and if it is successful then it will be published on the website.</p>");
            stringBuilder.Append("<h1>Event submission</h1>");

            stringBuilder.Append($"<p>Event name: {eventSubmission.Title}<br />");
            stringBuilder.Append($"Event date: {eventSubmission.EventDate:dddd dd MMMM yyyy}<br />");
            stringBuilder.Append($"Start time: {eventSubmission.StartTime}<br />");
            stringBuilder.Append($"End time: {eventSubmission.EndTime}<br />");
            stringBuilder.Append($"How often does your event occur?: {eventSubmission.Frequency}<br />");
            if (eventSubmission.EndDate.HasValue) stringBuilder.Append($"End date: {eventSubmission.EndDate:dddd dd MMMM yyyy}<br />");
            stringBuilder.Append($"Price: {eventSubmission.Fee}<br />");
            stringBuilder.Append($"Location: {eventSubmission.Location}<br />");            
            stringBuilder.Append($"Organiser name: {eventSubmission.SubmittedBy}<br />");
            stringBuilder.Append($"Description: {eventSubmission.Description}<br />");
            if (eventSubmission.Image != null) stringBuilder.Append($"Event image: {eventSubmission.Image.FileName}<br />");
            if (eventSubmission.Attachment != null) stringBuilder.Append($"Additional event document: {eventSubmission.Attachment.FileName}<br />");
            stringBuilder.Append($"<br />Organiser email address: {eventSubmission.SubmitterEmail}");
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

            var fromEmail = _configuration.GetEmailEmailFrom(_businessId.ToString()).IsValid()
                ? _configuration.GetEmailEmailFrom(_businessId.ToString()).ToString()
                : string.Empty;

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBody(eventSubmission),
                fromEmail,
               _configuration.GetEventSubmissionEmail(_businessId.ToString()).ToString(),
               eventSubmission.SubmitterEmail,
               attachments));
        }
    }
}
