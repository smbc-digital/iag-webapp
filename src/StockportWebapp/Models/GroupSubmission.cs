using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Validation;

namespace StockportWebapp.Models
{
    public class GroupSubmission
    {
        [Display(Name = "Group name")]
        [Required]
        [StringLength(255, ErrorMessage = "Group name must be 250 characters or less in length.")]
        public string Name { get; set; }

        [Display(Name = "Group meeting location")]
        [Required]
        [StringLength(500, ErrorMessage = "Location must be 500 characters or less in length.")]
        public string Address { get; set; }

        [ImageFileExtensionValidation]
        [FileSizeValidation]
        [Display(Name = "Group image (optional)")]
        public IFormFile Image { get; set; }

        [Display(Name ="Group description")]
        [Required]
        public string Description { get; set; }

        [Display(Name="Categories")]
        public List<string> Categories { get; set; }
        public string CategoriesList { get; set; }
        public List<string> AvailableCategories { get; set; }

        [Required]
        [Display(Name="Group email address")]
        [EmailAddress(ErrorMessage = "Should be a valid Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name="Group phone number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name="Group website")]       
        public string Website { get; set; }

        public string Twitter { get; set; }
        public string Facebook { get; set; }    
        
        [Display(Name="Do you have any volunteering oppurtunities?")]
        public bool Volunteering { get; set; }
    }
}
