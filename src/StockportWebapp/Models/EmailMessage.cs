using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Models
{
    public class EmailMessage
    {
        public string Subject { get; }
        public string Body { get; }
        public string ServiceEmail { get; }
        public string UserEmail { get; }
        public List<IFormFile> Attachments { get; }

        public EmailMessage(string subject, string body, string serviceEmail, string userEmail, List<IFormFile> attachments)
        {
            Subject = subject;
            Body = body;
            ServiceEmail = serviceEmail;
            UserEmail = userEmail;
            Attachments = attachments;
        }
    }
}
