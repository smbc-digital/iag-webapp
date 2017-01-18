using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Models
{
    public class EventSubmission
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public string Teaser { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name="Event date")]        
        public DateTime EventDate { get; set; }

        [Display(Name = "Start Time")]        
        public string StartTime { get; set; }

        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public List<string> Frequencylist = new List<string> {"Daily", "Weekly", "Fortnightly", "Monthly", "Yearly"};

        public string Frequency { get; set; }

        [Required]
        [MaxLength(255)]
        public string Fee { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        //[FileExtensions(Extensions = "jpg, png",ErrorMessage = "Should be an png or jpg file")]
        public IFormFile Image { get; set; }

        [Required]
        public string Description { get; set; }

        //[FileExtensions(Extensions = "docx, doc, pdf, odt", ErrorMessage = "Should be a docx, doc, pdf or odt file")]
        public IFormFile Attachment { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Submitter Email")]
        public string SubmitterEmail { get; set; }

        public EventSubmission()
        {
            
        }

        public EventSubmission(string title,  
            string teaser, DateTime eventDate, string startTime, string endTime, DateTime endDate,
            string frequency, string fee, string location, string submittedBy, IFormFile image, 
            string description, IFormFile attachment)
        {
            Title = title;
            Teaser = teaser;
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
        }
    }
}
