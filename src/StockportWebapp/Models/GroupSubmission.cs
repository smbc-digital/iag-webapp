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
        public string Slug { get; set; }
 
        [Display(Name = "Group name")]
        [Required]
        [StringLength(250, ErrorMessage = "Group name must be 250 characters or less in length.")]
        public string Name { get; set; }

        [Display(Name = "Group meeting location")]
        [Required(ErrorMessage = "Your meeting place was not recognised. Make sure you choose an address from the dropdown.")]
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
        [Required(ErrorMessage = "You must select at least one category")]
        public string CategoriesList { get; set; }
        public List<string> AvailableCategories { get; set; }

        [OneOutOfTwoFieldValidation(propertyName1: "Email", propertyName2: "PhoneNumber", ErrorMessage = "The group email address or group phone number field is required")]
        [Display(Name="Group email address")]
        [EmailAddress(ErrorMessage = "Should be a valid Email Address")]
        public string Email { get; set; }

        [OneOutOfTwoFieldValidation(propertyName1: "Email", propertyName2: "PhoneNumber", ErrorMessage = "The group email address or group phone number field is required")]
        [Display(Name="Group phone number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name="Group website")]       
        public string Website { get; set; }

        [Display(Name = "Twitter (optional)")]
        public string Twitter { get; set; }

        [Display(Name = "Facebook (optional)")]
        public string Facebook { get; set; }    
        
        [Display(Name="Do you have any volunteering opportunities?")]
        public bool Volunteering { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [StringLength(250, ErrorMessage = "Volunteering text must be 250 characters or less in length.")]
        public string VolunteeringText { get; set; } =
            "If you would like to find out more about being a volunteer with us, please e-mail with your interest and we’ll be in contact as soon as possible.";

        [Display(Name = "Provide addiitional information that only professionals and advisors can see")]
        public string AdditionalInformation { get; set; }

        [Display(Name = "Upload any additional documents")]
        public List<Document> AdditionalDocuments { get; set; }
    }
}
