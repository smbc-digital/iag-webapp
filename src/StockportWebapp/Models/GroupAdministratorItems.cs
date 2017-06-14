using System.ComponentModel.DataAnnotations;

namespace StockportWebapp.Models
{
    public class GroupAdministratorItems
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must supply a role")]
        public string Permission { get; set; } = string.Empty;
    }
}
