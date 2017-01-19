using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using StockportWebapp.Builders;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Builders
{
    public class EmailBuilderTest
    {
        private readonly IEmailBuilder _emailBuilder;

        public EmailBuilderTest()
        {
            _emailBuilder = new EmailBuilder();
        }

        [Fact]
        public void ShouldReturnMemoryStreamForEmailWithoutAttachments()
        {
            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "serviceEmail@mail.com", "userEmail@mail.com",
                new List<IFormFile>());

            var stream = _emailBuilder.BuildMessageToStream(emailMessage);

            stream.Should().NotBeNull();

            var emailAsString = Encoding.UTF8.GetString(stream.ToArray());

            emailAsString.Should().Contain("subject");
            emailAsString.Should().Contain("body");
            emailAsString.Should().Contain("serviceEmail@mail.com");
            emailAsString.Should().Contain("userEmail@mail.com");
        }

        [Fact]
        public void ShouldReturnMemoryStreamForEmailWithAttachments()
        {
            var attachments = new List<IFormFile>();
            var attachment = File.OpenRead("TestFiles/test_attachment.txt");
            attachments.Add(new FormFile(attachment, 0, attachment.Length, "test_attachment.txt", "test_attachment.txt"));
            
            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "serviceEmail@mail.com", "userEmail@mail.com", attachments);

            var stream = _emailBuilder.BuildMessageToStream(emailMessage);

            var emailAsString = Encoding.UTF8.GetString(stream.ToArray());

            emailAsString.Should().Contain("subject");
            emailAsString.Should().Contain("body");
            emailAsString.Should().Contain("serviceEmail@mail.com");
            emailAsString.Should().Contain("userEmail@mail.com");
            emailAsString.Should().Contain("test_attachment.txt");

            attachment.Dispose();
        }

        [Fact]
        public void ShouldReturnMemoryStreamForEmailWithMultipleAttachments()
        {
            var attachments = new List<IFormFile>();

            var attachment = File.OpenRead("TestFiles/test_attachment.txt");
            var docxAttachment = File.OpenRead("TestFiles/test_document.docx");
            attachments.Add(new FormFile(attachment, 0, attachment.Length, "test_attachment.txt", "test_attachment.txt"));
            attachments.Add(new FormFile(docxAttachment, 0, docxAttachment.Length, "test_document.docx", "test_document.docx"));

            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "serviceEmail@mail.com", "userEmail@mail.com", attachments);

            var stream = _emailBuilder.BuildMessageToStream(emailMessage);

            var emailAsString = Encoding.UTF8.GetString(stream.ToArray());

            emailAsString.Should().Contain("subject");
            emailAsString.Should().Contain("body");
            emailAsString.Should().Contain("serviceEmail@mail.com");
            emailAsString.Should().Contain("userEmail@mail.com");
            emailAsString.Should().Contain("test_attachment.txt");
            emailAsString.Should().Contain("test_document.docx");

            attachment.Dispose();
            docxAttachment.Dispose();
        }

        [Fact]
        public void ShouldReturnMemoryStreamWithMultipleSenderEmails()
        {
            var attachments = new List<IFormFile>();

            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "serviceEmail@mail.com, serviceEmail2@mail.com, serviceEmail3@mail.com", "userEmail@mail.com", attachments);

            var stream = _emailBuilder.BuildMessageToStream(emailMessage);

            var emailAsString = Encoding.UTF8.GetString(stream.ToArray());

            emailAsString.Should().Contain("subject");
            emailAsString.Should().Contain("body");
            emailAsString.Should().Contain("from@mail.com");
            emailAsString.Should().Contain("serviceEmail@mail.com");
            emailAsString.Should().Contain("serviceEmail2@mail.com");
            emailAsString.Should().Contain("serviceEmail3@mail.com");
            emailAsString.Should().Contain("userEmail@mail.com");
        }
    }
}
