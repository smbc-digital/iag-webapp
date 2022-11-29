using MimeKit;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Builders
{
    public interface IEmailBuilder
    {
        MemoryStream BuildMessageToStream(EmailMessage emailMessage);
    }

    public class EmailBuilder : IEmailBuilder
    {
        public MemoryStream BuildMessageToStream(EmailMessage emailMessage)
        {
            var stream = new MemoryStream();
            BuildMessage(emailMessage).WriteTo(stream);
            return stream;
        }

        private static MimeMessage BuildMessage(EmailMessage emailMessage)
        {
            var message = new MimeMessage();

            var toEmails = emailMessage.ToEmail.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var email in toEmails)
            {
                message.To.Add(new MailboxAddress(string.Empty, email.Trim()));
            }

            message.From.Add(new MailboxAddress(string.Empty, emailMessage.FromEmail));

            if (!string.IsNullOrEmpty(emailMessage.CcEmail))
            {
                message.Cc.Add(new MailboxAddress(string.Empty, emailMessage.CcEmail));
            }

            message.Subject = emailMessage.Subject;
            message.Body = BuildMessageBody(emailMessage.Body, emailMessage.Attachments).ToMessageBody();
            return message;
        }

        private static BodyBuilder BuildMessageBody(string bodyContent, List<IFormFile> attachments)
        {
            var body = new BodyBuilder
            {
                HtmlBody = @"<p>" + bodyContent + "</p>",
                TextBody = bodyContent,
            };

            if (bodyContent.Contains("<plaintext>"))
            {
                var start = bodyContent.IndexOf("<plaintext>");
                var end = bodyContent.IndexOf("</plaintext>") + "</plaintext>".Length;
                var plaintext = bodyContent.Substring(start, end - start);
                bodyContent = bodyContent.Remove(start, end - start);
                plaintext = plaintext.Remove(0, "<plaintext>".Length);
                plaintext = plaintext.Remove(plaintext.IndexOf("</plaintext>"), "</plaintext>".Length);
                body.HtmlBody = bodyContent;
                body.TextBody = plaintext;
            }

            if (attachments != null)
            {
                foreach (var file in attachments)
                {
                    body.Attachments.Add(FileHelper.GetFileNameFromPath(file), file.OpenReadStream());
                }
            }

            return body;
        }
    }
}
