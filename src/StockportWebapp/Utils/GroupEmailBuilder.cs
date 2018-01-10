using System;
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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom.Css;

namespace StockportWebapp.Utils
{
    public class GroupEmailBuilder
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
                DonationsNeeded = groupSubmission.Donations,
                Categories = groupSubmission.CategoriesList,
                VolunteeringText = groupSubmission.Volunteering ? groupSubmission.VolunteeringText : "-",
                VolunteeringNeeded = groupSubmission.Volunteering,
                DoantionsText = groupSubmission.Donations ? groupSubmission.DonationsText : "-",
                DonationUrl = groupSubmission.DonationsUrl,
                Facebook = groupSubmission.Facebook,
                Twitter = groupSubmission.Twitter,
                AgeRanges = groupSubmission.AgeRanges.Where(o => o.IsSelected).Select(o => o.Name).ToList(),
                Suitabilities = groupSubmission.Suitabilities.Where(o => o.IsSelected).Select(o => o.Name).ToList()
            };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
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

            var emailsTosend = string.Join(",", group.GroupAdministrators.Items.Select(i => i.Email).ToList());

            if (!string.IsNullOrEmpty(emailsTosend))
            {
                _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail, emailsTosend, new List<IFormFile>()));
            }
        }

        public virtual void SendEmailEventDelete(ProcessedEvents eventItem, ProcessedGroup group)
        {
            var messageSubject = $"Delete {eventItem.Title}";

            _logger.LogInformation("Sending event delete email");

            var emailBody = new EventDelete { Title = eventItem.Title };

            var emailsTosend = string.Join(",", group.GroupAdministrators.Items.Select(i => i.Email).ToList());
            emailsTosend = emailsTosend + "," +
                           _configuration.GetGroupSubmissionEmail(_businessId.ToString());

            if (!string.IsNullOrEmpty(emailsTosend))
            {
                _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail, emailsTosend, new List<IFormFile>()));
            }
        }

        public virtual void SendEmailArchive(Group group)
        {
            var messageSubject = $"Archive {group.Name}";

            _logger.LogInformation("Sending group archive email");

            var emailBody = new GroupArchive { Name = group.Name };

            var emailsTosend = string.Join(",", group.GroupAdministrators.Items.Select(i => i.Email).ToList());

            if (!string.IsNullOrEmpty(emailsTosend))
            {
                _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail, emailsTosend, new List<IFormFile>()));
            }
        }

        public virtual void SendEmailPublish(Group group)
        {
            var messageSubject = $"Publish {group.Name}";

            _logger.LogInformation("Sending group publish email");

            var emailBody = new GroupPublish() { Name = group.Name, Slug = group.Slug };

            var emailsTosend = string.Join(",", group.GroupAdministrators.Items.Select(i => i.Email).ToList());

            if (!string.IsNullOrEmpty(emailsTosend))
            {
                _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail, emailsTosend, new List<IFormFile>()));
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
                                                                    _emailClient.GenerateEmailBodyFromHtml(emailBody),
                                                                    _fromEmail,
                                                                    _configuration.GetGroupSubmissionEmail(_businessId.ToString()).ToString(),
                                                                    changeGroupInfo.Email,
                                                                    new List<IFormFile>()));
        }

        public virtual Task<HttpStatusCode> SendEmailReportGroup(ReportGroupViewModel reportGroupInfo)
        {
            var messageSubject = $"Changes to a group's information - {reportGroupInfo.GroupName}";

            _logger.LogInformation("Sending group submission form email");

            var emailBody = new ReportGroupInfoConfirmation
            {
                Email = reportGroupInfo.Email,
                Name = reportGroupInfo.Name,
                Subject = reportGroupInfo.Subject,
                Message = reportGroupInfo.Message,
                Slug = reportGroupInfo.Slug
            };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject,
                                                                    _emailClient.GenerateEmailBodyFromHtml(emailBody),
                                                                    _fromEmail,
                                                                    _configuration.GetGroupSubmissionEmail(_businessId.ToString()).ToString(),
                                                                    reportGroupInfo.Email,
                                                                    new List<IFormFile>()));
        }

        public virtual void SendEmailEditGroup(GroupSubmission group, string toEmail)
        {
            var messageSubject = $"Edit group {group.Name}";

            _logger.LogInformation("Sending edit group email");

            var emailBody = new GroupEdit {
                Name = group.Name,
                Categories = group.CategoriesList,
                Description = group.Description,
                Email = group.Email,
                Location = group.Address,
                Facebook = group.Facebook,
                Phone = group.PhoneNumber,
                Twitter = group.Twitter,
                Website = group.Website,
                AgeRanges = group.AgeRanges.Where(o => o.IsSelected).Select(o => o.Name).ToList(),
                Suitabilities = group.Suitabilities.Where(o => o.IsSelected).Select(o => o.Name).ToList(),
                Volunteering = group.Volunteering ? "Yes" : "No",
                VolunteeringText = group.VolunteeringText,
                AdditionalInformation = group.AdditionalInformation
            };

            var message = new EmailMessage(messageSubject,
                                            _emailClient.GenerateEmailBodyFromHtml(emailBody),
                                           _fromEmail, 
                                           toEmail + "," + _configuration.GetGroupArchiveEmail(_businessId.ToString()),
                                           new List<IFormFile>());

            _emailClient.SendEmailToService(message);
        }

        public virtual Task<HttpStatusCode> SendEmailEditUser(AddEditUserViewModel model)
        {
            var messageSubject = $"[Edit User] - {model.Name}";

            _logger.LogInformation("Sending Edit User email");

            var attachments = new List<IFormFile>();

            var emailBody = new EditUser() { Name = model.Name, Role = GetRoleByInitial(model.GroupAdministratorItem.Permission), PreviousRole = GetRoleByInitial(model.Previousrole) };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail,
                model.GroupAdministratorItem.Email,
                attachments));
        }

        public virtual Task<HttpStatusCode> SendEmailNewUser(AddEditUserViewModel model)
        {
            var messageSubject = $"[Add New User] - {model.Name}";

            _logger.LogInformation("Sending Add New User email");

            var attachments = new List<IFormFile>();

            var emailBody = new AddUser() { GroupName = model.Name, Role = GetRoleByInitial(model.GroupAdministratorItem.Permission) };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail,
                model.GroupAdministratorItem.Email,
                attachments));
        }

        public virtual Task<HttpStatusCode> SendEmailDeleteUser(RemoveUserViewModel model)
        {
            var messageSubject = $"[Delete User]";

            _logger.LogInformation("Sending Delete User email");

            var attachments = new List<IFormFile>();

            var emailBody = new DeleteUser() { GroupName = model.GroupName };

            return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
                _fromEmail,
                model.Email,
                attachments));
        }

        private string GetRoleByInitial(string initial)
        {
            switch (initial)
            {
                case "A":
                    return "Administrator";
                case "E":
                    return "Editor";
                default:
                    return String.Empty;
            }
        }
    }
}
