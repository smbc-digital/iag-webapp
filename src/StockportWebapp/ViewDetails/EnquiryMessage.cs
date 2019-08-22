using System.ComponentModel.DataAnnotations;

namespace StockportWebapp.ViewDetails
{
    public class EnquiryMessage
    {
        private const string DefaultValue = "";

        [Required(ErrorMessage = "An enquiry message is required")]
        [Display(Name = "Enquiry")]
        [MaxLength(500, ErrorMessage = "The enquiry message must be no more than 500 characters long")]
        [StringLength(500, ErrorMessage = "Too many characters")]
        public string Message { get; set; }

        public EnquiryMessage (string message)
        {
            Message = message;
        }
    }
}
