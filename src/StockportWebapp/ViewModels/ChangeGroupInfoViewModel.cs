using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockportWebapp.ViewModels
{
    public class ChangeGroupInfoViewModel
    {
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

        [Required(ErrorMessage = "A subject is required")]
        [Display(Name = "Subject")]
        [MaxLength(80, ErrorMessage = "The subject must be no more than 80 characters long")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "An enquiry message is required")]
        [Display(Name = "Enquiry")]
        [MaxLength(500, ErrorMessage = "The enquiry message must be no more than 500 characters long")]
        [StringLength(500, ErrorMessage = "Too much string")]
        public string Message { get; set; }

        public string Slug { get; set; }
        public string GroupName { get; set; }
    }
}
