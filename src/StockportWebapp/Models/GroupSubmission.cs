using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Validation;

namespace StockportWebapp.Models
{
    public class GroupSubmission
    {
        public GroupSubmission()
        {
            Suitabilities = new List<CheckBoxItem> { new CheckBoxItem { Name = "Autism", IsSelected = false }, new CheckBoxItem { Name= "Deaf or hard of hearing", IsSelected = false }, new CheckBoxItem { Name = "Dementia", IsSelected = false }, new CheckBoxItem { Name = "Learning disabilities", IsSelected =  false }, new CheckBoxItem { Name = "Mental health conditions", IsSelected = false }, new CheckBoxItem { Name = "Physical disabilities", IsSelected = false }, new CheckBoxItem  { Name =  "Visual impairments", IsSelected = false }, new CheckBoxItem { Name = "Wheelchair users", IsSelected = false } };
            AgeRanges = new List<CheckBoxItem> { new CheckBoxItem { Name = "0-2 Babies", IsSelected = false }, new CheckBoxItem { Name = "3-5 Toddlers", IsSelected = false }, new CheckBoxItem { Name = "6-11 Young children", IsSelected = false }, new CheckBoxItem { Name = "12-18 Teenagers", IsSelected = false }, new CheckBoxItem { Name = "19-30 Young adults", IsSelected = false }, new CheckBoxItem { Name = "31-50 Adults", IsSelected = false }, new CheckBoxItem { Name = "51-65 Middle aged", IsSelected = false }, new CheckBoxItem { Name = "66-80 Retirees", IsSelected = false }, new CheckBoxItem { Name = "80+ Elderly", IsSelected = false } };
        }

        public string Slug { get; set; }
 
        [Display(Name = "Enter the name of your group or service")]
        [Required]
        [StringLength(250, ErrorMessage = "Group name must be 250 characters or less in length.")]
        public string Name { get; set; }

        [Display(Name = "Enter the location of your group or service")]
        [Required(ErrorMessage = "Your location was not recognised. Make sure you choose an address from the dropdown.")]
        [StringLength(500, ErrorMessage = "Location must be 500 characters or less in length.")]
        public string Address { get; set; }

        [ImageFileExtensionValidation]
        [FileSizeValidation]
        [Display(Name = "Upload a profile image (optional)")]
        public IFormFile Image { get; set; }

        [Display(Name ="Group description")]
        [Required]
        public string Description { get; set; }

        [Display(Name="Choose one or more categories that best describe your group or service")]        
        public List<string> Categories { get; set; }
        [Required(ErrorMessage = "You must select at least one category")]
        public string CategoriesList { get; set; }
        public List<string> AvailableCategories { get; set; }

        [Display(Name="Enter an email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Required]
        public string Email { get; set; }

        [Display(Name= "Enter a phone number (optional)")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name="Enter a website address (optional)")]       
        public string Website { get; set; }

        [Display(Name = "Enter a twitter handle (optional)")]
        [RegularExpression(@"(?=.)@([A-Za-z]+[A-Za-z0-9]+)", ErrorMessage = "Enter twitter name ")]
        public string Twitter { get; set; }

        [Display(Name = "Enter a facebook url (optional)")]
        [RegularExpression(@"(?=.(?:http|https):\/\/)?(?:www.)?facebook.com\/?.*", ErrorMessage="Please enter the full facebook url ")]
        public string Facebook { get; set; }    
        
        [Display(Name="Do you have any volunteering opportunities?")]
        public bool Volunteering { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [StringLength(250, ErrorMessage = "Volunteering text must be 250 characters or less in length.")]
        public string VolunteeringText { get; set; } =
            "If you would like to find out more about being a volunteer with us, please e-mail with your interest and we’ll be in contact as soon as possible.";

        [Display(Name = "Provide additional information that only professionals and advisors can see")]
        public string AdditionalInformation { get; set; }

        [Display(Name = "Upload any additional documents")]
        public List<Document> AdditionalDocuments { get; set; }

        public List<CheckBoxItem> Suitabilities { get; set; }

        public List<CheckBoxItem> AgeRanges { get; set; }
    }
}