using System;
using System.Collections.Generic;
using System.Net;
using StockportWebapp.Models;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Utils;

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

            stringBuilder.AppendLine("<head>");
            stringBuilder.AppendLine("<meta content=\"text/html; charset=UTF-8\" http-equiv=\"Content-Type\" />");
            stringBuilder.AppendLine("</head>");
            stringBuilder.AppendLine("<body>");
            stringBuilder.AppendLine("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" id=\"templateHeader\" style=\"background-color: #F4F4F4; border-collapse: collapse !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt;\" width=\"100%\">");
            stringBuilder.AppendLine("<tr>");
            stringBuilder.AppendLine("<td class=\"headerContent\" style=\"background-color: #055C58; font-family: Helvetica; font-size: 20px; font-weight: bold; line-height: 100%; mso-table-lspace: 0pt; mso-table-rspace: 0pt; padding-bottom: 10px; padding-left: 10px; padding-right: 0; padding-top: 10px; text-align: left; vertical-align: middle;\" valign=\"top\"><img class=\"flexibleImage\" src=\"https://s3-eu-west-1.amazonaws.com/live-iag-static-assets/logo-stockport-full%402x.png\" style =\"border: 0; height: auto; line-height: 100%; max-width: 225px; outline: none; text-decoration: none; width: 225px;\" width=\"225\" /></td>");
            stringBuilder.AppendLine("</tr>");
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("<h2 style=\"font-family: 'Source Sans Pro', sans-serif\">Thank you for adding an event</h2>");
            stringBuilder.AppendLine("<p style=\"font-family: 'Source Sans Pro', sans-serif\">Before your event is added to the calendar, it will be reviewed and if it is successful then it will be published on the website.</p>");
            stringBuilder.AppendLine("<h2 style=\"font-family: 'Source Sans Pro', sans-serif\">Your event</h2>");

            stringBuilder.AppendLine($"<p style=\"font-family: 'Source Sans Pro', sans-serif; line-height: 1.35em \">Event name: {eventSubmission.Title}<br />");
            if (eventSubmission.EventDate.HasValue) stringBuilder.AppendLine($"Event date: {eventSubmission.EventDate.Value:dddd dd MMMM yyyy}<br />");
            if (eventSubmission.StartTime.HasValue) stringBuilder.AppendLine($"Start time: {eventSubmission.StartTime.Value:HH:mm}<br />");
            if (eventSubmission.EndTime.HasValue) stringBuilder.AppendLine($"End time: {eventSubmission.EndTime.Value:HH:mm}<br />");
            if (!string.IsNullOrEmpty(eventSubmission.Frequency)) stringBuilder.AppendLine($"Frequency: {eventSubmission.Frequency}<br />");
            if (eventSubmission.EndDate.HasValue) stringBuilder.AppendLine($"End date: {eventSubmission.EndDate.Value:dddd dd MMMM yyyy}<br />");
            stringBuilder.AppendLine($"Price: {eventSubmission.Fee}<br />");
            stringBuilder.AppendLine($"Location: {eventSubmission.Location}<br />");            
            stringBuilder.AppendLine($"Organiser name: {eventSubmission.SubmittedBy}<br />");
            stringBuilder.AppendLine($"Description: {eventSubmission.Description}<br />");
            if (eventSubmission.Image != null) stringBuilder.AppendLine($"Event image: {FileHelper.GetFileNameFromPath(eventSubmission.Image)}<br />");
            stringBuilder.AppendLine($"Categories: {eventSubmission.Category1}");
            if (!string.IsNullOrEmpty(eventSubmission.Category2))
                stringBuilder.AppendLine($", {eventSubmission.Category2}");
            if (!string.IsNullOrEmpty(eventSubmission.Category3))
                stringBuilder.AppendLine($", {eventSubmission.Category3}");
            stringBuilder.AppendLine($"<br />");
            if (eventSubmission.Attachment != null) stringBuilder.AppendLine($"Additional event document: {FileHelper.GetFileNameFromPath(eventSubmission.Attachment)}<br />");
            stringBuilder.AppendLine($"<br />Organiser email address: {eventSubmission.SubmitterEmail}");
            stringBuilder.AppendLine("</p>");
            stringBuilder.AppendLine("<h2 style=\"font-family: 'Source Sans Pro', sans-serif\">Changes to your event</h2>");
            stringBuilder.AppendLine("<p style=\"font-family: 'Source Sans Pro', sans-serif\">You can let us know about a change to this event by emailing us at:</p>");
            stringBuilder.AppendLine("<p style=\"font-family: 'Source Sans Pro', sans-serif\"><a href=\"mailto:website.updates@stockport.gov.uk\"> website.updates@stockport.gov.uk</a></p>");
            stringBuilder.AppendLine("<p style=\"font-family: 'Source Sans Pro', sans-serif\">Thank you,</p> <br />");
            stringBuilder.AppendLine("<p style=\"font-family: 'Source Sans Pro', sans-serif\">Stockport Council</p> <br />");
            stringBuilder.AppendLine("</body>");
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
               attachments)
               );
        }
    }
}
