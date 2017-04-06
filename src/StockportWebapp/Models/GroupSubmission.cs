using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Validation;

namespace StockportWebapp.Models
{
    public class GroupSubmission
    {
        [Display(Name = "Group name")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Display(Name = "Location")]
        [Required]
        public string Address { get; set; }

        [ImageFileExtensionValidation]
        [FileSizeValidation]
        [Display(Name = "Group image (optional)")]
        public IFormFile Image { get; set; }

        [Display(Name ="Group description")]
        [Required]
        public string Description { get; set; }

        public List<string> Categories;

        [Required(ErrorMessage = "Select at least one group")]
        [MaxLength(255)]
        public string Category1 { get; set; }

        [Required]
        [Display(Name="Group email address")]
        [EmailAddress(ErrorMessage = "Should be a valid Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name="Group phone number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name="Group website")]
        [Url(ErrorMessage = "Should be validate Web Address")]
        public string Website { get; set; }

        public string Twitter { get; set; }
        public string Facebook { get; set; }      
    }
}
