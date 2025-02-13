﻿namespace StockportWebapp.Utils;

public class GroupEmailBuilder
{
    private readonly ILogger<GroupEmailBuilder> _logger;
    private readonly IApplicationConfiguration _configuration;
    private readonly BusinessId _businessId;
    private readonly IHttpEmailClient _emailClient;
    private readonly string _fromEmail;

    public GroupEmailBuilder(
        ILogger<GroupEmailBuilder> logger,
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
        string messageSubject = $"[Group] - {groupSubmission.Name}";

        _logger.LogInformation("Sending group submission form email");

        List<IFormFile> attachments = new();
        if (groupSubmission.Image is not null)
            attachments.Add(groupSubmission.Image);

        GroupAdd emailBody = new()
        {
            Name = groupSubmission.Name,
            Location = groupSubmission.Address,
            Image = groupSubmission.Image is not null ? FileHelper.GetFileNameFromPath(groupSubmission.Image) : "-",
            Description = groupSubmission.Description,
            Email = groupSubmission.Email,
            Phone = groupSubmission.PhoneNumber,
            Website = groupSubmission.Website,
            DonationsNeeded = groupSubmission.Donations,
            Categories = groupSubmission.CategoriesList,
            VolunteeringText = groupSubmission.Volunteering ? groupSubmission.VolunteeringText : "-",
            VolunteeringNeeded = groupSubmission.Volunteering,
            DonationsText = groupSubmission.Donations ? groupSubmission.DonationsText : "-",
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
        string messageSubject = $"Delete {group.Name}";

        _logger.LogInformation("Sending group delete email");

        GroupDelete emailBody = new() { Name = group.Name };

        string emailsTosend = string.Join(",", group.GroupAdministrators.Items.Select(i => i.Email).ToList());

        if (!string.IsNullOrEmpty(emailsTosend))
        {
            _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
            _fromEmail, emailsTosend, new List<IFormFile>()));
        }
    }

    public virtual void SendEmailEventDelete(ProcessedEvents eventItem, ProcessedGroup group)
    {
        string messageSubject = $"Delete {eventItem.Title}";

        _logger.LogInformation("Sending event delete email");

        EventDelete emailBody = new() { Title = eventItem.Title };

        string emailsTosend = string.Join(",", group.GroupAdministrators.Items.Select(i => i.Email).ToList());
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
        string messageSubject = $"Archive {group.Name}";

        _logger.LogInformation("Sending group archive email");

        GroupArchive emailBody = new() { Name = group.Name };

        string emailsTosend = string.Join(",", group.GroupAdministrators.Items.Select(i => i.Email).ToList());

        if (!string.IsNullOrEmpty(emailsTosend))
        {
            _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
            _fromEmail, emailsTosend, new List<IFormFile>()));
        }
    }

    public virtual void SendEmailPublish(Group group)
    {
        string messageSubject = $"Publish {group.Name}";

        _logger.LogInformation("Sending group publish email");

        GroupPublish emailBody = new() { Name = group.Name, Slug = group.Slug };

        string emailsTosend = string.Join(",", group.GroupAdministrators.Items.Select(i => i.Email).ToList());

        if (!string.IsNullOrEmpty(emailsTosend))
        {
            _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
            _fromEmail, emailsTosend, new List<IFormFile>()));
        }
    }

    public virtual Task<HttpStatusCode> SendEmailChangeGroupInfo(ChangeGroupInfoViewModel changeGroupInfo)
    {
        string messageSubject = $"Changes to a group's information - {changeGroupInfo.GroupName}";

        _logger.LogInformation("Sending group submission form email");

        ChangeGroupInfoConfirmation emailBody = new()
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
        string messageSubject = $"Changes to a group's information - {reportGroupInfo.GroupName}";

        _logger.LogInformation("Sending group submission form email");

        ReportGroupInfoConfirmation emailBody = new()
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
        string messageSubject = $"Edit group {group.Name}";

        _logger.LogInformation("Sending edit group email");

        GroupEdit emailBody = new()
        {
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
            VolunteeringText = group.Volunteering ? group.VolunteeringText : "N/A",
            AdditionalInformation = group.AdditionalInformation
        };

        EmailMessage message = new (messageSubject,
                                    _emailClient.GenerateEmailBodyFromHtml(emailBody),
                                    _fromEmail,
                                    toEmail + "," + _configuration.GetGroupArchiveEmail(_businessId.ToString()),
                                    new List<IFormFile>());

        _emailClient.SendEmailToService(message);
    }

    public virtual Task<HttpStatusCode> SendEmailEditUser(AddEditUserViewModel model)
    {
        string messageSubject = $"[Edit User] - {model.Name}";

        _logger.LogInformation("Sending Edit User email");

        List<IFormFile> attachments = new();

        EditUser emailBody = new()
        {
            Name = model.Name,
            Role = GetRoleByInitial(model.GroupAdministratorItem.Permission),
            PreviousRole = GetRoleByInitial(model.Previousrole)
        };

        return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
            _fromEmail,
            model.GroupAdministratorItem.Email,
            attachments));
    }

    public virtual Task<HttpStatusCode> SendEmailNewUser(AddEditUserViewModel model)
    {
        string messageSubject = $"[Add New User] - {model.Name}";

        _logger.LogInformation("Sending Add New User email");

        List<IFormFile> attachments = new();

        AddUser emailBody = new()
        {
            GroupName = model.Name,
            Role = GetRoleByInitial(model.GroupAdministratorItem.Permission)
        };

        return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
            _fromEmail,
            model.GroupAdministratorItem.Email,
            attachments));
    }

    public virtual Task<HttpStatusCode> SendEmailDeleteUser(RemoveUserViewModel model)
    {
        string messageSubject = $"[Delete User]";

        _logger.LogInformation("Sending Delete User email");

        List<IFormFile> attachments = new();

        DeleteUser emailBody = new() { GroupName = model.GroupName };

        return _emailClient.SendEmailToService(new EmailMessage(messageSubject, _emailClient.GenerateEmailBodyFromHtml(emailBody),
            _fromEmail,
            model.Email,
            attachments));
    }

    private string GetRoleByInitial(string initial) =>
        initial switch
        {
            "A" => "Administrator",
            "E" => "Editor",
            _ => string.Empty,
        };
}