namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EmailMessage
{
    public string Subject { get; }
    public string Body { get; }
    public string FromEmail { get; }
    public string ToEmail { get; }
    public string CcEmail { get; }
    public string BccEmail { get; }
    public List<IFormFile> Attachments { get; }

    public EmailMessage(string subject, string body, string fromEmail, string toEmail, string ccEmail, string bccEmail, List<IFormFile> attachments)
    {
        Subject = subject;
        Body = body;
        FromEmail = fromEmail;
        ToEmail = toEmail;
        BccEmail = bccEmail;
        CcEmail = ccEmail;
        Attachments = attachments;
        FromEmail = fromEmail;
    }
}