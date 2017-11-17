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
using System.Linq;

namespace StockportWebapp.Utils
{
    public class EventEmailBuilder
    {
        private readonly ILogger<EventEmailBuilder> _logger;
        private readonly IApplicationConfiguration _configuration;
        private readonly BusinessId _businessId;
        private readonly IHttpEmailClient _emailClient;
        private readonly string _fromEmail;
        private readonly IEmailHandler _emailHandler;

        public EventEmailBuilder(ILogger<EventEmailBuilder> logger,
            IHttpEmailClient emailClient,
            IApplicationConfiguration configuration,
            BusinessId businessId, IEmailHandler emailHandler)
        {
            _logger = logger;
            _configuration = configuration;
            _businessId = businessId;
            _emailHandler = emailHandler;
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
                Categories = eventSubmission.CategoriesList,
                GroupName = string.IsNullOrEmpty(eventSubmission.GroupName) ? string.Empty : $"Group name: {eventSubmission.GroupName}",
                Occurrences = eventSubmission.Occurrences == 0 ? string.Empty : $"(occurs {eventSubmission.Occurrences} times)",
            };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailHandler.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail,
                _configuration.GetEventSubmissionEmail(_businessId.ToString()).ToString(),
                eventSubmission.SubmitterEmail,
                attachments));
        }

        public virtual Task<HttpStatusCode> SendEmailEditEvent(EventSubmission eventDetail, string updatedByEmail)
        {
            var messageSubject = $"Edit event {eventDetail.Title}";

            _logger.LogInformation("Sending event edit form email");

            var emailBody = new EventEdit
            {
                Title = eventDetail.Title,
                EventDate = eventDetail.EventDate.HasValue ? ((DateTime)eventDetail.EventDate).ToString("dddd dd MMMM yyyy") : "-",
                EndDate = eventDetail.EndDate.HasValue ? ((DateTime)eventDetail.EndDate).ToString("dddd dd MMMM yyyy") : "-",
                StartTime = eventDetail.StartTime.HasValue ? ((DateTime)eventDetail.StartTime).ToString("HH:mm") : "-",
                EndTime = eventDetail.EndTime.HasValue ? ((DateTime)eventDetail.EndTime).ToString("HH:mm") : "-",
                Frequency = !string.IsNullOrEmpty(eventDetail.Frequency) ? eventDetail.Frequency : "-",
                Fee = !string.IsNullOrEmpty(eventDetail.Fee) ? eventDetail.Fee : "-",
                Location = !string.IsNullOrEmpty(eventDetail.Location) ? eventDetail.Location : "-",
                SubmittedBy = !string.IsNullOrEmpty(eventDetail.SubmittedBy) ? eventDetail.SubmittedBy : "-",
                Description = !string.IsNullOrEmpty(eventDetail.Description) ? eventDetail.Description : "-",
                SubmitterEmail = !string.IsNullOrEmpty(eventDetail.SubmitterEmail) ? eventDetail.SubmitterEmail : "-",
                Categories = eventDetail.CategoriesList,
                GroupName = string.IsNullOrEmpty(eventDetail.GroupName) ? string.Empty : $"Group name: {eventDetail.GroupName}",
                Occurrences = eventDetail.Occurrences == 0 ? string.Empty : $"(occurs {eventDetail.Occurrences} times)",
            };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailHandler.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail,
                _configuration.GetEventSubmissionEmail(_businessId.ToString()).ToString(),
                eventDetail.SubmitterEmail,
                null));
        }
    }
}
