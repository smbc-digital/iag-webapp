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
        [RegularExpression("(?=.)@([A-Za-z]+[A-Za-z0-9_]+)", ErrorMessage = "Enter twitter name e.g. @TwitterHandle")]
        public string Twitter { get; set; }

        // js regex attribute validation doens't seem to allow "/i" for case insensitive, this is why e.g. [fF][aA] etc.
        // TODO: Find a better way of doing this if possible
        [Display(Name = "Enter a facebook url (optional)")]
        [RegularExpression(@"(((?:[hH][tT][tT][pP])(?:[sS])?(:\/\/))?(?:[wW][wW][wW].)?(([fF][aA][cC][eE][bB][oO][oO][kK].[cC][oO][mM])\/[A-Za-z0-9\$-_.+!*'()-]+))", ErrorMessage="Please enter the full facebook url e.g. www.facebook.com/yourpage")]
        public string Facebook { get; set; }    
        
        [Display(Name="Let people know you are looking for volunteering opportunities ?")]
        public bool Volunteering { get; set; }

        [Display(Name = "Let people know you are looking for donations ?")]
        public bool Donations { get; set; }

        [Display(Name = "Donations url ")]
        [RegularExpression(@"((?:[hH][tT][tT][pP])(?:[sS])?(:\/\/))(?:[wW][wW][wW].*)?.*", ErrorMessage = "The link must start with either https:// or http:// ")]
        public string DonationsUrl { get; set; } 
        

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [StringLength(250, ErrorMessage = "Volunteering text must be 250 characters or less in length.")]
        [Display(Name = "volunteering text")]
        [Required]
        public string VolunteeringText { get; set; }

        [StringLength(250, ErrorMessage = "Donation text must be 250 characters or less in length.")]
        [Display(Name = "donations text")]
        [RequiredIfDonationsCheckedOnEditGroup]
        public string DonationsText { get; set; }

        [Display(Name = "Provide additional information that only professionals and advisors can see")]
        public string AdditionalInformation { get; set; }

        [Display(Name = "Upload any additional documents")]
        public List<Document> AdditionalDocuments { get; set; }

        public List<CheckBoxItem> Suitabilities { get; set; }

        public List<CheckBoxItem> AgeRanges { get; set; }
    }
}