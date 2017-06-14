using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using StockportWebapp.Models;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StockportWebapp.AmazonSES;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public interface IGroupRepository
    {
        string GenerateEmailBody(GroupSubmission groupSubmission);
        Task<HttpStatusCode> SendEmailMessage(GroupSubmission groupSubmission);
        void SendEmailArchive(ProcessedGroup group);
        void SendEmailDelete(ProcessedGroup group);
    }

    public class GroupRepository : IGroupRepository
    {
        private readonly ILogger<GroupRepository> _logger;
        private readonly IApplicationConfiguration _configuration;
        private readonly BusinessId _businessId;
        private readonly IHttpEmailClient _emailClient;

        public GroupRepository(ILogger<GroupRepository> logger,
            IHttpEmailClient emailClient,
            IApplicationConfiguration configuration,
            BusinessId businessId)
        {
            _logger = logger;
            _configuration = configuration;
            _businessId = businessId;
            _emailClient = emailClient;
        }

        public string GenerateEmailBody(GroupSubmission groupSubmission)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<p>Thank you for submitting a group</p>");
            stringBuilder.Append("<p>Before your group is added to the group directory, it will be reviewed and if it is successful then it will be published on the website.</p>");
            stringBuilder.Append("<h1>Group submission</h1>");

            stringBuilder.Append($"<p>Group name: {groupSubmission.Name}<br />");
            stringBuilder.Append($"Location: {groupSubmission.Address}<br />");
            stringBuilder.Append($"Group description: {groupSubmission.Description}<br />");
            stringBuilder.Append($"Group categories: {groupSubmission.Categories}<br />");
            stringBuilder.Append($"Group email address: {groupSubmission.Email}<br />");
            stringBuilder.Append($"Group website: {groupSubmission.Website}<br />");
            stringBuilder.Append($"Group phone number: {groupSubmission.PhoneNumber}<br />");
            if (groupSubmission.Image != null) stringBuilder.Append($"Event image: {FileHelper.GetFileNameFromPath(groupSubmission.Image)}<br />");
            stringBuilder.Append("</p>");

            return stringBuilder.ToString();
        }

        public string GenerateEmailBodyArchive(ProcessedGroup group)
        {
            var stringBuilder = new StringBuilder();           
            stringBuilder.Append("<h1>"+group.Name+" has been successfully archived</h1>");

            stringBuilder.Append("<p>Your group has been archived straight away. This means it has been unpublished and that people will not be able to view it on the website.</p>");
            stringBuilder.Append("<p>Archiving the group means that you will be able republish it at any time in the future.</p>");

            return stringBuilder.ToString();
        }

        public string GenerateEmailBodyDelete(ProcessedGroup group)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<h1>" + group.Name + " has been successfully deleted</h1>");

            stringBuilder.Append("<p>Your group has been removed from the website straight away.</p>");
            stringBuilder.Append("<p>Please note, your group's information has been completely deleted from our systems and you will not be able to recover it.</p>");

            return stringBuilder.ToString();
        }

        public Task<HttpStatusCode> SendEmailMessage(GroupSubmission groupSubmission)
        {
            var messageSubject = $"[Group] - {groupSubmission.Name}";

            _logger.LogInformation("Sending group submission form email");

            var attachments = new List<IFormFile>();
            if (groupSubmission.Image != null) attachments.Add(groupSubmission.Image);
            
            var fromEmail = _configuration.GetEmailEmailFrom(_businessId.ToString()).IsValid()
                ? _configuration.GetEmailEmailFrom(_businessId.ToString()).ToString()
                : string.Empty;

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBody(groupSubmission),
                fromEmail,
               _configuration.GetGroupSubmissionEmail(_businessId.ToString()).ToString(),
               groupSubmission.Email,
               attachments)
               );
        }

        public void SendEmailArchive(ProcessedGroup group)
        {
            var messageSubject = $"Archive {group}";

            _logger.LogInformation("Sending group archive email");

            var fromEmail = _configuration.GetEmailEmailFrom(_businessId.ToString()).IsValid()
                ? _configuration.GetEmailEmailFrom(_businessId.ToString()).ToString()
                : string.Empty;

            foreach (var groupAdministrator in group.GroupAdministrators.Items)
            {
               _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBodyArchive(group),
               fromEmail, _configuration.GetGroupArchiveEmail(_businessId.ToString()).ToString(), groupAdministrator.Email, new List<IFormFile>())
              );
            }
        }

        public void SendEmailDelete(ProcessedGroup group)
        {
            var messageSubject = $"Delete {group}";

            _logger.LogInformation("Sending group delete email");

            var fromEmail = _configuration.GetEmailEmailFrom(_businessId.ToString()).IsValid()
                ? _configuration.GetEmailEmailFrom(_businessId.ToString()).ToString()
                : string.Empty;

            foreach (var groupAdministrator in group.GroupAdministrators.Items)
            {
                _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBodyDelete(group),
                fromEmail, _configuration.GetGroupArchiveEmail(_businessId.ToString()).ToString(), groupAdministrator.Email, new List<IFormFile>())
               );
            }
        }
    }
}