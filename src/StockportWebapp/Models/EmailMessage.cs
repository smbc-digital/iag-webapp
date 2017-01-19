using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Models
{
    public class EmailMessage
    {
        public string Subject { get; }
        public string Body { get; }
        public string FromEmail { get; }
        public string ToEmail { get; }
        public string UserEmail { get; }
        public List<IFormFile> Attachments { get; }

        public EmailMessage(string subject, string body, string fromEmail, string toEmail, string userEmail, List<IFormFile> attachments)
        {
            Subject = subject;
            Body = body;
            FromEmail = fromEmail;
            ToEmail = toEmail;
            UserEmail = userEmail;
            Attachments = attachments;
            FromEmail = fromEmail;
        }
    }
}
