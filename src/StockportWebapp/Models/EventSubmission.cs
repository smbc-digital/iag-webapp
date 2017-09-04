using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Validation;

namespace StockportWebapp.Models
{
    public class EventSubmission
    {
        [Required]
        [MaxLength(255)]
        [Display(Name = "Event name")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Event date")]
        public DateTime? EventDate { get; set; }

        [Required]
        [Display(Name = "Start time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime? StartTime { get; set; }

        [Required]
        [Display(Name = "End time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        [EndTimeLaterThanStartTimeValidation(otherPropertyName: "StartTime", errorMessage: "End Time should be after Start Time")]
        public DateTime? EndTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        [EndDateLaterThanStartDateValidation(otherPropertyName: "EventDate", errorMessage: "End Date should be after Start Date")]
        [EndDateGreaterThanStartDateFrequencyPeriodValidation(otherPropertyName: "EventDate", frequencyPropertyName: "Frequency", errorMessage: "End Date should be after Start Date")]
        public DateTime? EndDate { get; set; }

        public string RecurringEventYn { get; set; }
        public int Occurrences { get; set; }

        public Dictionary<string, string> Frequencylist = new Dictionary<string, string>()
        {
            { "Daily",""},
            { "Weekly",""},
            { "Fortnightly",""},
            { "Monthly Date","For example, 15th of every month"},
            { "Monthly Day","For example, on the first Friday of every month"},
            { "Yearly",""}
        };

        [Display(Name = "How often does your event occur?")]
        public string Frequency { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Price")]
        public string Fee { get; set; }

        public List<string> AvailableCategories { get; set; }
        public string CategoriesList { get; set; }

        [Required]
        public string Location { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Organiser name")]
        public string SubmittedBy { get; set; }

        [ImageFileExtensionValidation]
        [FileSizeValidation]
        [Display(Name = "Event image (optional)")]
        public IFormFile Image { get; set; }

        [Required]
        public string Description { get; set; }

        [DocumentFileExtensionValidation]
        [FileSizeValidation]
        [Display(Name = "Additional event document (optional)")]
        public IFormFile Attachment { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Organiser email address")]
        public string SubmitterEmail { get; set; }
       
        public string GroupName { get; set; }

        public string GroupSlug { get; set; }

        public string Slug { get; set; }

        public List<string> BuildCategoryList()
        {
           return new List<string> {
                "Air Raid Shelters", "Arts and crafts", "Bramall Hall", "Business", "Community and charity", "Children and families", "Dancing", "Digital skills", "Education and learning", "Fairs",
                "Food and drink", "Hat Works", "Health and wellbeing", "Libraries", "Markets", "Museums", "Music and concerts", "Open days and drop-ins", "Parks and outdoors", "Seasonal", "Sports and fitness",
                "Staircase House", "Stockport War Memorial Art Gallery", "Talks and lectures", "Town Hall", "Theatre, performance and comedy"};
        }
    }   
}
