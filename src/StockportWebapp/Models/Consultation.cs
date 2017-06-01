using System;

namespace StockportWebapp.Models
{
    public class Consultation
    {
        public string Title { get; set; } = string.Empty;
        public DateTime ClosingDate { get; set; } = DateTime.Now.AddDays(-1);
        public string Link { get; set; } = string.Empty;

        public Consultation(string title, DateTime closingDate, string link)
        {
            Title = title;
            ClosingDate = closingDate;
            Link = link;
        }
    }
}
