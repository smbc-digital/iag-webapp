namespace StockportWebappTests_Unit.Unit.AmazonSES;

public class HttpEmailClientTest
{
    private readonly Mock<ILogger<HttpEmailClient>> _mockLogger = new();
    private readonly Mock<IEmailBuilder> _emailBuilder = new();
    private readonly Mock<IAmazonSimpleEmailService> _amazonEmailService = new();

    [Fact]
    public async Task ItShouldReturnA500AndLogItIfTheServiceEmailIsNullOrEmpty()
    {
        // Arrange
        HttpEmailClient emailClient = new(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, true);
        EmailMessage emailMessage = new("subject", "body", string.Empty, string.Empty, "user@email.com", "bcc@email.com", new List<IFormFile>());

        // Act
        HttpStatusCode httpStatusCode = await emailClient.SendEmailToService(emailMessage);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, httpStatusCode);
        LogTesting.Assert(_mockLogger, LogLevel.Error, "ToEmail can not be null or empty. No email has been sent.");
    }

    [Fact]
    public async Task ItShouldReturnA200IfTheStatusCodeIsOkAndLogTheResponse()
    {
        // Arrange
        _amazonEmailService
            .Setup(service => service.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SendRawEmailResponse { HttpStatusCode = HttpStatusCode.OK, MessageId = "test", ResponseMetadata = new ResponseMetadata { RequestId = "test" } });

        HttpEmailClient emailClient = new(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, true);
        EmailMessage emailMessage = new("subject", "body", "from@mail.com", "service@mail.com", "user@email.com", "bcc@email.com", new List<IFormFile>());

        // Act
        HttpStatusCode httpStatusCode = await emailClient.SendEmailToService(emailMessage);

        // Assert
        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        LogTesting.Assert(_mockLogger, LogLevel.Information, "An email was sent to Amazon SES with message id: test and request id test");
    }


    [Fact]
    public async Task ItShouldReturnA500IfTheEmailServiceThrowsAnException()
    {
        // Arrange
        _amazonEmailService
            .Setup(service => service.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SendRawEmailResponse { HttpStatusCode = HttpStatusCode.InternalServerError, MessageId = "test", ResponseMetadata = new ResponseMetadata { RequestId = "test" } });

        HttpEmailClient emailClient = new(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, true);
        EmailMessage emailMessage = new("subject", "body", "from@mail.com", "service@mail.com", "user@email.com", "bcc@emai.com", new List<IFormFile>());

        // Act
        HttpStatusCode httpStatusCode = await emailClient.SendEmailToService(emailMessage);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, httpStatusCode);
        LogTesting.Assert(_mockLogger, LogLevel.Warning, $"There was a problem sending an email, message id: test and request id: test and status code {HttpStatusCode.InternalServerError}");
    }

    [Fact]
    public async Task ItShouldReturnABadRequestIfAmazonSesReturnsAnException()
    {
        // Arrange
        _amazonEmailService
            .Setup(service => service.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("There was an error"));

        HttpEmailClient emailClient = new(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, true);
        EmailMessage emailMessage = new("subject", "body", "from@mail.com", "service@mail.com", "user@email.com", "bcc@email.com", new List<IFormFile>());

        // Act
        HttpStatusCode httpStatusCode = await emailClient.SendEmailToService(emailMessage);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, httpStatusCode);
        LogTesting.Assert(_mockLogger, LogLevel.Error, "An error occurred trying to send an email to Amazon SES. \nThere was an error");
    }

    [Fact]
    public async Task ItShouldReturnAOk_When_SendAmazonEmail_SetToFalse()
    {
        // Arrange
        HttpEmailClient emailClient = new(_mockLogger.Object, _emailBuilder.Object, _amazonEmailService.Object, false);
        EmailMessage emailMessage = new("subject", "body", "from@mail.com", "service@mail.com", "user@email.com", "bcc@email.com", new List<IFormFile>());

        // Act
        HttpStatusCode httpStatusCode = await emailClient.SendEmailToService(emailMessage);

        // Assert
        Assert.Equal(HttpStatusCode.OK, httpStatusCode);
        _amazonEmailService.Verify(amazonEmailService => amazonEmailService.SendRawEmailAsync(It.IsAny<SendRawEmailRequest>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}