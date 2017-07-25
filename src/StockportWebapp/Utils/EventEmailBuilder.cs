using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Emails.Models;
using StockportWebapp.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace StockportWebapp.Utils
{
    public class EventEmailBuilder : EmailBuilder
    {
        private readonly ILogger<EventEmailBuilder> _logger;
        private readonly IApplicationConfiguration _configuration;
        private readonly BusinessId _businessId;
        private readonly IHttpEmailClient _emailClient;
        private readonly string _fromEmail;

        public EventEmailBuilder(ILogger<EventEmailBuilder> logger,
            IHttpEmailClient emailClient,
            IApplicationConfiguration configuration,
            BusinessId businessId)
        {
            _logger = logger;
            _configuration = configuration;
            _businessId = businessId;
            _emailClient = emailClient;
            _fromEmail = _configuration.GetEmailEmailFrom(_businessId.ToString()).IsValid()
                ? _configuration.GetEmailEmailFrom(_businessId.ToString()).ToString()
                : string.Empty;

        }

        public virtual Task<HttpStatusCode> SendEmailAddNew(EventSubmission eventSubmission)
        {
            var messageSubject = $"[Event date] - {eventSubmission.EventDate.Value.ToString("d MMMM yyyy")}, [Event] - {eventSubmission.Title}";

            _logger.LogInformation("Sending event submission form email");

            var attachments = new List<IFormFile>();
            if (eventSubmission.Image != null) attachments.Add(eventSubmission.Image);
            if (eventSubmission.Attachment != null) attachments.Add(eventSubmission.Attachment);

            var categories = string.Concat(string.IsNullOrEmpty(eventSubmission.Category1) ? string.Empty : $"{eventSubmission.Category1}, ",
                                            string.IsNullOrEmpty(eventSubmission.Category2) ? string.Empty : $"{eventSubmission.Category2}, ",
                                            string.IsNullOrEmpty(eventSubmission.Category3) ? string.Empty : $"{eventSubmission.Category3}, ").TrimEnd(' ').TrimEnd(',');

            var imagePath = FileHelper.GetFileNameFromPath(eventSubmission.Image);
            var attachmentPath = FileHelper.GetFileNameFromPath(eventSubmission.Attachment);

            var emailBody = new EventAdd
            {
                Title = eventSubmission.Title,
                EventDate = eventSubmission.EventDate.HasValue ? ((DateTime)eventSubmission.EventDate).ToString("dddd dd MMMM yyyy") : "-",
                EndDate = eventSubmission.EndDate.HasValue ? ((DateTime)eventSubmission.EndDate).ToString("dddd dd MMMM yyyy") : "-",
                StartTime = eventSubmission.StartTime.HasValue ? ((DateTime)eventSubmission.StartTime).ToString("HH:mm") : "-",
                EndTime = eventSubmission.EndTime.HasValue ? ((DateTime)eventSubmission.EndTime).ToString("HH:mm") : "-",
                Frequency = !string.IsNullOrEmpty(eventSubmission.Frequency) ? eventSubmission.Frequency : "-",
                Fee = !string.IsNullOrEmpty(eventSubmission.Fee) ? eventSubmission.Fee : "-",
                Location = !string.IsNullOrEmpty(eventSubmission.Location) ? eventSubmission.Location : "-",
                SubmittedBy = !string.IsNullOrEmpty(eventSubmission.SubmittedBy) ? eventSubmission.SubmittedBy : "-",
                Description = !string.IsNullOrEmpty(eventSubmission.Description) ? eventSubmission.Description : "-",
                ImagePath = !string.IsNullOrEmpty(imagePath) ? imagePath : "-",
                AttachmentPath = !string.IsNullOrEmpty(attachmentPath) ? attachmentPath : "-",
                SubmitterEmail = !string.IsNullOrEmpty(eventSubmission.SubmitterEmail) ? eventSubmission.SubmitterEmail : "-",
                Categories = categories,
                GroupName = string.IsNullOrEmpty(eventSubmission.GroupName) ? string.Empty : $"Group name: {eventSubmission.GroupName}"
            };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBodyFromHtml(emailBody),
                _fromEmail,
                _configuration.GetEventSubmissionEmail(_businessId.ToString()).ToString(),
                eventSubmission.SubmitterEmail,
                attachments));
        }
    }
}
