﻿namespace StockportWebapp.ViewModels;

public class ContactUsDetails
{
    public string ServiceEmailId { get; set; }
    public string ServiceEmail { get; set; }
    private const string DefaultValue = "";

    [Required(ErrorMessage = "Enter your name")]
    [Display(Name = "Name")]
    [MaxLength(80, ErrorMessage = "Your name must be no more than 80 characters long")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Enter an email address")]
    [EmailAddress(ErrorMessage = "This is not a valid email address")]
    [Display(Name = "Email address")]
    [DefaultValue(DefaultValue)]
    [MaxLength(254, ErrorMessage = "The email address must be no more than 254 characters long")]
    public string Email { get; set; }

    [HiddenInput]
    public string Title { get; set; }

    [Required(ErrorMessage = "Enter the subject of your enquiry")]
    [Display(Name = "Subject")]
    [MaxLength(80, ErrorMessage = "The subject must be no more than 80 characters long")]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Tell us about your enquiry")]
    [Display(Name = "Enquiry")]
    [MaxLength(500, ErrorMessage = "The enquiry must be no more than 500 characters long")]
    [StringLength(500, ErrorMessage = "Too much string")]
    public string Message { get; set; }

    public IEnumerable<Crumb> Breadcrumbs = new List<Crumb>();

    public ContactUsDetails()
    { }

    public ContactUsDetails(string serviceEmailId, string title)
    {
        ServiceEmailId = serviceEmailId;
        Title = title;
    }

    public ContactUsDetails(string name, string email, string message, string subject, string serviceEmailId, string title)
    {
        Name = name;
        Email = email;
        Message = message;
        Subject = subject;
        ServiceEmailId = serviceEmailId;
        Title = title;
    }
}