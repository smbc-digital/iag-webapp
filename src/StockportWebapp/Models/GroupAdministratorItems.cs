using System.ComponentModel.DataAnnotations;

namespace StockportWebapp.Models
{
    public class GroupAdministratorItems
    {
        [Required(ErrorMessage = "You must supply an email")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must supply a permission")]
        public string Permission { get; set; } = string.Empty;
    }
}
