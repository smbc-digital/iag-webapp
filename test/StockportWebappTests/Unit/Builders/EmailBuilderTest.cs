namespace StockportWebappTests_Unit.Unit.Builders;

public class EmailBuilderTest
{
    private readonly IEmailBuilder _emailBuilder;

     public EmailBuilderTest()
    {
        _emailBuilder = new EmailBuilder();
    }

    [Fact]
    public void BuildMessageToStream_ShouldReturnMemoryStreamForEmailWithoutAttachments()
    {
        // Arrange
        EmailMessage emailMessage = new("subject",
                                        "body",
                                        "from@mail.com",
                                        "serviceEmail@mail.com",
                                        "userEmail@mail.com",
                                        new List<IFormFile>());
        // Act
        MemoryStream stream = _emailBuilder.BuildMessageToStream(emailMessage);

        // Assert
        string emailAsString = Encoding.UTF8.GetString(stream.ToArray());
        Assert.NotNull(stream);
        Assert.Contains("subject", emailAsString);
        Assert.Contains("body", emailAsString);
        Assert.Contains("serviceEmail@mail.com", emailAsString);
        Assert.Contains("userEmail@mail.com", emailAsString);
    }

    [Fact]
    public void BuildMessageToStream_ShouldReturnMemoryStreamForEmailWithAttachments()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile.Setup(file => file.FileName).Returns("test_attachment.txt");
        mockFile.Setup(file => file.OpenReadStream().Length).Returns(5242879);
        mockFile.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

        List<IFormFile> attachments = new()
        {
            mockFile.Object
        };

        EmailMessage emailMessage = new("subject",
                                        "body",
                                        "from@mail.com",
                                        "serviceEmail@mail.com",
                                        "userEmail@mail.com",
                                        attachments);

        // Act
        MemoryStream stream = _emailBuilder.BuildMessageToStream(emailMessage);

        // Assert
        string emailAsString = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("subject", emailAsString);
        Assert.Contains("body", emailAsString);
        Assert.Contains("serviceEmail@mail.com", emailAsString);
        Assert.Contains("test_attachment.txt", emailAsString);
    }

    [Fact]
    public void BuildMessageToStream_ShouldReturnMemoryStreamForEmailWithMultipleAttachments()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile.Setup(file => file.FileName).Returns("test_attachment.txt");
        mockFile.Setup(file => file.OpenReadStream().Length).Returns(5242879);
        mockFile.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

        Mock<IFormFile> mockFile2 = new();
        mockFile2.Setup(file => file.FileName).Returns("test_document.docx");
        mockFile2.Setup(file => file.OpenReadStream().Length).Returns(5242879);
        mockFile2.Setup(file => file.OpenReadStream()).Returns(new MemoryStream());

        List<IFormFile> attachments = new()
        {
            mockFile.Object,
            mockFile2.Object
        };

        EmailMessage emailMessage = new("subject",
                                        "body",
                                        "from@mail.com",
                                        "serviceEmail@mail.com",
                                        "userEmail@mail.com",
                                        attachments);

        // Act
        MemoryStream stream = _emailBuilder.BuildMessageToStream(emailMessage);

        // Assert
        string emailAsString = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("subject", emailAsString);
        Assert.Contains("body", emailAsString);
        Assert.Contains("serviceEmail@mail.com", emailAsString);
        Assert.Contains("userEmail@mail.com", emailAsString);
        Assert.Contains("test_attachment.txt", emailAsString);
        Assert.Contains("test_document.docx", emailAsString);
    }

    [Fact]
    public void BuildMessageToStream_ShouldReturnMemoryStreamWithMultipleSenderEmails()
    {
        // Arrange
        List<IFormFile> attachments = new();
        EmailMessage emailMessage = new("subject",
                                        "body",
                                        "from@mail.com",
                                        "serviceEmail@mail.com, serviceEmail2@mail.com, serviceEmail3@mail.com",
                                        "userEmail@mail.com",
                                        attachments);

        // Act
        MemoryStream stream = _emailBuilder.BuildMessageToStream(emailMessage);

        // Assert
        string emailAsString = Encoding.UTF8.GetString(stream.ToArray());

        Assert.Contains("subject", emailAsString);
        Assert.Contains("body", emailAsString);
        Assert.Contains("from@mail.com", emailAsString);
        Assert.Contains("serviceEmail@mail.com", emailAsString);
        Assert.Contains("serviceEmail2@mail.com", emailAsString);
        Assert.Contains("serviceEmail3@mail.com", emailAsString);
        Assert.Contains("userEmail@mail.com", emailAsString);
    }
}