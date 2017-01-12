using System;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Models
{
    public class EventSubmission
    {
        public string Title { get; set; }
        public string Teaser { get; set; }
        public DateTime EventDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Occurrences { get; set; }
        public string Frequency { get; set; }
        public string Fee { get; set; }
        public string Location { get; set; }
        public string SubmittedBy { get; set; }
        public IFormFile Image { get; set; }
        public string Description { get; set; }
        public IFormFile Attachment { get; set; }
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
