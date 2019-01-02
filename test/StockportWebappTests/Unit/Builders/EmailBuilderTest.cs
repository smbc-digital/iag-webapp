using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using StockportWebapp.Builders;
using StockportWebapp.Models;

namespace StockportWebappTests_Unit.Unit.Builders
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
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(o => o.FileName).Returns("test_attachment.txt");
            mockFile.Setup(o => o.OpenReadStream().Length).Returns(5242879);
            mockFile.Setup(o => o.OpenReadStream()).Returns(new MemoryStream());

            var attachments = new List<IFormFile> {mockFile.Object};

            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "serviceEmail@mail.com", "userEmail@mail.com", attachments);

            var stream = _emailBuilder.BuildMessageToStream(emailMessage);

            var emailAsString = Encoding.UTF8.GetString(stream.ToArray());

            emailAsString.Should().Contain("subject");
            emailAsString.Should().Contain("body");
            emailAsString.Should().Contain("serviceEmail@mail.com");
            emailAsString.Should().Contain("userEmail@mail.com");
            emailAsString.Should().Contain("test_attachment.txt");
        }

        [Fact]
        public void ShouldReturnMemoryStreamForEmailWithMultipleAttachments()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(o => o.FileName).Returns("test_attachment.txt");
            mockFile.Setup(o => o.OpenReadStream().Length).Returns(5242879);
            mockFile.Setup(o => o.OpenReadStream()).Returns(new MemoryStream());

            var mockFile2 = new Mock<IFormFile>();
            mockFile2.Setup(o => o.FileName).Returns("test_document.docx");
            mockFile2.Setup(o => o.OpenReadStream().Length).Returns(5242879);
            mockFile2.Setup(o => o.OpenReadStream()).Returns(new MemoryStream());

            var attachments = new List<IFormFile> { mockFile.Object, mockFile2.Object };

            var emailMessage = new EmailMessage("subject", "body", "from@mail.com", "serviceEmail@mail.com", "userEmail@mail.com", attachments);

            var stream = _emailBuilder.BuildMessageToStream(emailMessage);

            var emailAsString = Encoding.UTF8.GetString(stream.ToArray());

            emailAsString.Should().Contain("subject");
            emailAsString.Should().Contain("body");
            emailAsString.Should().Contain("serviceEmail@mail.com");
            emailAsString.Should().Contain("userEmail@mail.com");
            emailAsString.Should().Contain("test_attachment.txt");
            emailAsString.Should().Contain("test_document.docx");
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
