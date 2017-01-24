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
        [Display(Name="Event date")]        
        public DateTime? EventDate { get; set; }

        [Display(Name = "Start time (optional)")]        
        public string StartTime { get; set; }

        [Display(Name = "End time (optional)")]
        public string EndTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End date (optional)")]
        public DateTime? EndDate { get; set; }

        public List<string> Frequencylist = new List<string> {"Daily", "Weekly", "Fortnightly", "Monthly", "Yearly"};

        [Display(Name = "How often does your event occur? (optional)")]
        public string Frequency { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Price")]
        public string Fee { get; set; }

        [Required]
        public string Location { get; set; }

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

        public EventSubmission() { }

        public EventSubmission(string title, DateTime eventDate, string startTime, string endTime, 
            DateTime endDate, string frequency, string fee, string location, string submittedBy, 
            IFormFile image, string description, IFormFile attachment, string submitterEmail)
        {
            Title = title;
            EventDate = eventDate;
            StartTime = startTime;
            EndTime = endTime;
            EndDate = endDate;     
            Frequency = frequency;
            Fee = fee;
            Location = location;
            SubmittedBy = submittedBy;
            Image = image;
            Description = description;
            Attachment = attachment;
            SubmitterEmail = submitterEmail;
        }
    }
}
