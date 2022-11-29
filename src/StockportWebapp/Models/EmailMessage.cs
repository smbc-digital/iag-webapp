namespace StockportWebapp.Models
{
    public class EmailMessage
    {
        public string Subject { get; }
        public string Body { get; }
        public string FromEmail { get; }
        public string ToEmail { get; }
        public string CcEmail { get; }
        public List<IFormFile> Attachments { get; }

        public EmailMessage(string subject, string body, string fromEmail, string toEmail, string ccEmail, List<IFormFile> attachments)
        {
            Subject = subject;
            Body = body;
            FromEmail = fromEmail;
            ToEmail = toEmail;
            CcEmail = ccEmail;
            Attachments = attachments;
            FromEmail = fromEmail;
        }

        public EmailMessage(string subject, string body, string fromEmail, string toEmail, List<IFormFile> attachments)
        {
            Subject = subject;
            Body = body;
            FromEmail = fromEmail;
            ToEmail = toEmail;
            Attachments = attachments;
            FromEmail = fromEmail;
        }
    }
}
