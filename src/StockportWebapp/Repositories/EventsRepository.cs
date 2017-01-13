using System.Net;
using StockportWebapp.Models;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.v3;
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
            var stringBuilder = new StringBuilder("Event Submssion\n\n");
            stringBuilder.Append($"Title: {eventSubmission.Title}\n");
            stringBuilder.Append($"Teaser: {eventSubmission.Teaser}\n");
            stringBuilder.Append($"Event Date: {eventSubmission.EventDate:dddd dd MMMM yyyy}\n");
            stringBuilder.Append($"Start Time: {eventSubmission.StartTime}\n");
            stringBuilder.Append($"End Time: {eventSubmission.EndTime}\n");
            stringBuilder.Append($"Occurrences: {eventSubmission.Occurrences}\n");
            stringBuilder.Append($"Fee: {eventSubmission.Fee}\n");
            stringBuilder.Append($"Location: {eventSubmission.Location}\n");            
            stringBuilder.Append($"Submitted By: {eventSubmission.SubmittedBy}\n");
            stringBuilder.Append($"Description: {eventSubmission.Description}\n");
            if (eventSubmission.Image != null) stringBuilder.Append($"Image: {eventSubmission.Image.FileName}\n");
            if (eventSubmission.Attachment != null) stringBuilder.Append($"Attachment: {eventSubmission.Attachment.FileName}\n");

            return stringBuilder.ToString();
        }

        public Task<HttpStatusCode> SendEmailMessage(EventSubmission eventSubmission)
        {
            var messageSubject = $"[Event] - {eventSubmission.Title}";

            _logger.LogInformation("Sending event submission form email");

           
            return _emailClient.SendEmailToService
                (messageSubject,
                GenerateEmailBody(eventSubmission),
               _configuration.GetEventSubmissionEmail(_businessId.ToString()).ToString(),
               eventSubmission.SubmitterEmail);
        }
    }
}
