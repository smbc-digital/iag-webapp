using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Emails.Models;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace StockportWebapp.Utils
{
    public class GroupEmailBuilder : EmailBuilder
    {
        private readonly ILogger<GroupEmailBuilder> _logger;
        private readonly IApplicationConfiguration _configuration;
        private readonly BusinessId _businessId;
        private readonly IHttpEmailClient _emailClient;
        private readonly string _fromEmail;

        public GroupEmailBuilder(ILogger<GroupEmailBuilder> logger,
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

        public virtual Task<HttpStatusCode> SendEmailAddNew(GroupSubmission groupSubmission)
        {
            var messageSubject = $"[Group] - {groupSubmission.Name}";

            _logger.LogInformation("Sending group submission form email");

            var attachments = new List<IFormFile>();
            if (groupSubmission.Image != null) attachments.Add(groupSubmission.Image);

            var emailBody = new GroupAdd
            {
                Name = groupSubmission.Name,
                Location = groupSubmission.Address,
                Image = groupSubmission.Image != null ? FileHelper.GetFileNameFromPath(groupSubmission.Image) : "-",
                Description = groupSubmission.Description,
                Email = groupSubmission.Email,
                Phone = groupSubmission.PhoneNumber,
                Website = groupSubmission.Website,
                Categories = groupSubmission.CategoriesList
            };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBodyFromHtml(emailBody),
                _fromEmail,
                _configuration.GetGroupSubmissionEmail(_businessId.ToString()).ToString(),
                groupSubmission.Email,
                attachments));
        }

        public virtual void SendEmailDelete(ProcessedGroup group)
        {
            var messageSubject = $"Delete {group.Name}";

            _logger.LogInformation("Sending group delete email");

            var emailBody = new GroupDelete { Name = group.Name };

            _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBodyFromHtml(emailBody),
                _fromEmail, _configuration.GetGroupArchiveEmail(_businessId.ToString()).ToString(), group.Email,
                new List<IFormFile>()));

            foreach (var groupAdministrator in group.GroupAdministrators.Items)
            {
                _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBodyFromHtml(emailBody),
                _fromEmail, groupAdministrator.Email, new List<IFormFile>()));
            }
        }

        public virtual void SendEmailArchive(ProcessedGroup group)
        {
            var messageSubject = $"Archive {group.Name}";

            _logger.LogInformation("Sending group archive email");

            var emailBody = new GroupDelete { Name = group.Name };

            _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBodyFromHtml(emailBody),
                _fromEmail, _configuration.GetGroupArchiveEmail(_businessId.ToString()).ToString(), group.Email,
                new List<IFormFile>()));

            foreach (var groupAdministrator in group.GroupAdministrators.Items)
            {
                _emailClient.SendEmailToService(new EmailMessage(messageSubject, GenerateEmailBodyFromHtml(emailBody),
                _fromEmail, groupAdministrator.Email, new List<IFormFile>()));
            }
        }

        public virtual Task<HttpStatusCode> SendEmailChangeGroupInfo(ChangeGroupInfoViewModel changeGroupInfo)
        {
            var messageSubject = $"Changes to a group's information - {changeGroupInfo.GroupName}";

            _logger.LogInformation("Sending group submission form email");

            var emailBody = new ChangeGroupInfoConfirmation
            {
                Email = changeGroupInfo.Email,
                Name = changeGroupInfo.Name,
                Subject = changeGroupInfo.Subject,
                Message = changeGroupInfo.Message,
                Slug = changeGroupInfo.Slug
            };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject,
                                                                    GenerateEmailBodyFromHtml(emailBody),
                                                                    _fromEmail,
                                                                    _configuration.GetGroupSubmissionEmail(_businessId.ToString()).ToString(),
                                                                    changeGroupInfo.Email,
                                                                    new List<IFormFile>()));
        }
    }
}
