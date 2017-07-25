namespace StockportWebapp.Emails.Models
{
    public class EventAdd
    {
        public string Title { get; set; }
        public string EventDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Frequency { get; set; }
        public string EndDate { get; set; }
        public string Fee { get; set; }
        public string Location { get; set; }
        public string SubmittedBy { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string AttachmentPath { get; set; }
        public string Categories { get; set; }
        public string SubmitterEmail { get; set; }
        public string GroupName { get; set; }
    }
}