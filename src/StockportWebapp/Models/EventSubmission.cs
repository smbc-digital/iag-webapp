using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Models
{
    public class EventSubmission
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Teaser { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Occurrences { get; set; }
        public string Frequency { get; set; }
        [Required]
        public string Fee { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string SubmittedBy { get; set; }
        [FileExtensions(Extensions = "jpg,png",ErrorMessage = "Should be an png or jpg file")]
        public IFormFile Image { get; set; }
        [Required]
        public string Description { get; set; }
        [FileExtensions(Extensions = "docx,doc,pdf,odt", ErrorMessage = "Should be a docx, doc, pdf or odt file")]
        public IFormFile Attachment { get; set; }
        [Required]
        [EmailAddress]
        public string SubmitterEmail { get; set; }

        public EventSubmission()
        {
            
        }

        public EventSubmission(string title, 
            string teaser, DateTime eventDate, string startTime, string endTime, int occurrences, 
            string frequency, string fee, string location, string submittedBy, IFormFile image, 
            string description, IFormFile attachment)
        {
            Title = title;
            Teaser = teaser;
            EventDate = eventDate;
            StartTime = startTime;
            EndTime = endTime;
            Occurrences = occurrences;
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
