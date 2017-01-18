using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using MimeKit;
using StockportWebapp.Models;

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

        private MimeMessage BuildMessage(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(string.Empty, emailMessage.ServiceEmail));
            message.To.Add(new MailboxAddress(string.Empty, emailMessage.UserEmail));
            message.Subject = emailMessage.Subject;
            message.Body = BuildMessageBody(emailMessage.Body, emailMessage.Attachments).ToMessageBody();
            return message;
        }

        private BodyBuilder BuildMessageBody(string bodyContent, List<IFormFile> attachments)
        {
            var body = new BodyBuilder
            {
                HtmlBody = @"<p>" + bodyContent + "</p>",
                TextBody = bodyContent,
            };

            foreach (var file in attachments)
            {
                body.Attachments.Add(file.FileName, file.OpenReadStream());
            }

            return body;
        }
    }
}
