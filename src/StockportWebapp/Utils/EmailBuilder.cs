namespace StockportWebapp.Utils;

public interface IEmailBuilder
{
    MemoryStream BuildMessageToStream(EmailMessage emailMessage);
}

public class EmailBuilder : IEmailBuilder
{
    public MemoryStream BuildMessageToStream(EmailMessage emailMessage)
    {
        MemoryStream stream = new();
        BuildMessage(emailMessage).WriteTo(stream);

        return stream;
    }

    private static MimeMessage BuildMessage(EmailMessage emailMessage)
    {
        MimeMessage message = new();

        string[] toEmails = emailMessage.ToEmail.Split([","], StringSplitOptions.RemoveEmptyEntries);

        foreach (string email in toEmails)
        {
            message.To.Add(new MailboxAddress(string.Empty, email.Trim()));
        }

        message.From.Add(new MailboxAddress(string.Empty, emailMessage.FromEmail));
        message.Bcc.Add(new MailboxAddress(string.Empty, emailMessage.BccEmail));

        if (!string.IsNullOrEmpty(emailMessage.CcEmail))
            message.Cc.Add(new MailboxAddress(string.Empty, emailMessage.CcEmail));

        message.Subject = emailMessage.Subject;
        message.Body = BuildMessageBody(emailMessage.Body, emailMessage.Attachments).ToMessageBody();

        return message;
    }

    private static BodyBuilder BuildMessageBody(string bodyContent, List<IFormFile> attachments)
    {
        BodyBuilder body = new()
        {
            HtmlBody = @"<p>" + bodyContent + "</p>",
            TextBody = bodyContent,
        };

        if (bodyContent.Contains("<plaintext>"))
        {
            int start = bodyContent.IndexOf("<plaintext>");
            int end = bodyContent.IndexOf("</plaintext>") + "</plaintext>".Length;
            string plaintext = bodyContent.Substring(start, end - start);

            bodyContent = bodyContent.Remove(start, end - start);
            plaintext = plaintext.Remove(0, "<plaintext>".Length);
            plaintext = plaintext.Remove(plaintext.IndexOf("</plaintext>"), "</plaintext>".Length);
            body.HtmlBody = bodyContent;
            body.TextBody = plaintext;
        }

        if (attachments is not null)
        {
            foreach (IFormFile file in attachments)
            {
                body.Attachments.Add(FileHelper.GetFileNameFromPath(file), file.OpenReadStream());
            }
        }

        return body;
    }
}