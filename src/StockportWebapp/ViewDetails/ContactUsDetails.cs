using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;

namespace StockportWebapp.ViewDetails
{
    public class ContactUsDetails
    {
        public string ServiceEmailId { get; set; }
        public string ServiceEmail { get; set; }
        private const string DefaultValue = "";

        [Required(ErrorMessage = "Your name is required")]
        [Display(Name = "Name")]
        [MaxLength(80, ErrorMessage = "Your name must be no more than 80 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "An email address is required")]
        [EmailAddress(ErrorMessage = "This is not a valid email address")]
        [Display(Name = "Email address")]
        [DefaultValue(DefaultValue)]
        [MaxLength(254, ErrorMessage = "The email address must be no more than 254 characters long")]
        public string Email { get; set; }

        [HiddenInput]
        public string Title { get; set; }

        [Required(ErrorMessage = "A subject is required")]
        [Display(Name = "Subject")]
        [MaxLength(80, ErrorMessage = "The subject must be no more than 80 characters long")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "An enquiry message is required")]
        [Display(Name = "Enquiry")]
        [MaxLength(500, ErrorMessage = "The enquiry message must be no more than 500 characters long")]
        [StringLength(500, ErrorMessage = "Too much string")]
        public string Message { get; set; }


        public IEnumerable<Crumb> Breadcrumbs = new List<Crumb>();

        public ContactUsDetails()
        {
        }

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
}
