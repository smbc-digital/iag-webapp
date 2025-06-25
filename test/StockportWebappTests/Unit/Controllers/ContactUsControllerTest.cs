namespace StockportWebappTests_Unit.Unit.Controllers;

public class ContactUsControllerTest
{
    private readonly ContactUsController _controller;
    private readonly Mock<IHttpEmailClient> _mockEmailClient = new();
    private readonly Mock<ILogger<ContactUsController>> _mockLogger = new();
    private readonly Mock<IApplicationConfiguration> _configuration = new();
    private readonly BusinessId _businessId;
    private readonly string _userEmail = "contactme@email.com";
    private readonly ContactUsDetails _validContactDetails;
    private readonly string _userName = "name";
    private readonly string _emailSubject = "Drugs and Alcohol";
    private readonly string _emailBody = "A body";
    private readonly string _serviceEmails = "service@email.com, another@email.com";
    private const string Path = "/page-with-contact-us-form";
    private readonly string _url = $"http://page.com{Path}";
    private readonly string _title = "Title";
    private readonly Mock<IRepository> _repository = new();
    private readonly Mock<IFeatureManager> _featureManager = new();
    private readonly ContactUsId _contactUsId;

    public ContactUsControllerTest()
    {
        _configuration
            .Setup(conf => conf.GetEmailEmailFrom(It.IsAny<string>()))
            .Returns(AppSetting.GetAppSetting("businessid:Email:EmailFrom"));

        _featureManager
            .Setup(featureManager => featureManager.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        
        _businessId = new BusinessId("businessid");

        _contactUsId = new ContactUsId("name", "slug", "email", "test button text");

        _repository
            .Setup(repo => repo.Get<ContactUsId>(It.IsAny<string>(), It.IsAny<List<Query>>()))
            .ReturnsAsync(HttpResponse.Successful(200, _contactUsId));

        _controller = new ContactUsController(_repository.Object,
                                            _mockEmailClient.Object,
                                            _mockLogger.Object,
                                            _configuration.Object,
                                            _businessId,
                                            _featureManager.Object);
        
        _validContactDetails = new ContactUsDetails(_userName,
                                                    _userEmail,
                                                    _emailSubject,
                                                    _emailBody,
                                                    _serviceEmails,
                                                    _title);

        Mock<HttpRequest> request = new();
        Mock<HttpContext> context = new();
        HeaderDictionary headerDictionary = new() { { "referer", _url } };

        request.Setup(req => req.Headers).Returns(headerDictionary);
        context.Setup(con => con.Request).Returns(request.Object);

        _controller.ControllerContext = new ControllerContext(new ActionContext(context.Object, new RouteData(), new ControllerActionDescriptor()));
    }

    [Fact]
    public async Task ItSendsAnEmailToService()
    {
        // Act & Assert
        await _controller.Contact(_validContactDetails);
        _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()));
    }

    [Fact]
    public async Task ShouldNotSendEmailWhenInvalid()
    {
        // Arrange
        ContactUsDetails invalidDetails = new() { Email = "nam", Name = "name" };
        _controller.ModelState.AddModelError(string.Empty, "Error");

        // Act
        await _controller.Contact(invalidDetails);

        // Assert
        _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()), Times.Never);
    }

    [Fact]
    public async Task ShouldCreateTheEmailBodyFromContactUsDetailsForPostIfItIsValid()
    {
        // Act
        await _controller.Contact(_validContactDetails);

        // Arrange
        _mockEmailClient.Verify(client => client.SendEmailToService(It.Is<EmailMessage>(
            message => !string.IsNullOrEmpty(message.Subject)
            && message.Body.Contains(_userName)
            && message.Body.Contains(_userEmail)
            && message.Body.Contains(_emailSubject)
            && message.Body.Contains(_emailBody)
            && message.Body.Contains(_url)
        )));
    }

    [Fact]
    public async Task ShouldSendSentStatusBackInTheRedirectAsTrueIfMessageValidAndIsSentSuccessfully()
    {
        // Arrange
        _mockEmailClient
            .Setup(client => client.SendEmailToService(It.IsAny<EmailMessage>()))
            .ReturnsAsync(HttpStatusCode.OK);

        // Act
        IActionResult pageResult = await _controller.Contact(_validContactDetails);

        // Assert
        RedirectToActionResult redirectResult = pageResult as RedirectToActionResult;
        Assert.IsType<RedirectToActionResult>(pageResult);
        Assert.Equal("ThankYouMessage", redirectResult.ActionName);
    }

    [Fact]
    public async Task ShouldSendSentStatusBackInTheRedirectAsFalseIfMessageValidButIsNotSentSuccessfully()
    {
        // Arrange
        _mockEmailClient
            .Setup(client => client.SendEmailToService(It.IsAny<EmailMessage>()))
            .Returns(Task.FromResult(HttpStatusCode.BadRequest));

        // Act
        RedirectResult pageResult = await _controller.Contact(_validContactDetails) as RedirectResult; ;

        // Assert
        Assert.Contains("message=We have been unable to process the request. Please try again later.", pageResult.Url);
    }

    [Fact]
    public async Task ShouldSendSentStatusBackInTheRedirectAsFalseIfMessageInvalid()
    {
        // Arrange
        ContactUsDetails invalidDetails = new() { Email = "nam", Name = "name" };
        _controller.ModelState.AddModelError("Name", "an invalid name was provided");
        _controller.ModelState.AddModelError("Email", "an invalid email was provided");

        // Act
        RedirectResult pageResult = await _controller.Contact(invalidDetails) as RedirectResult; ;

        // Assert
        Assert.Contains("message=an invalid name was provided<br />an invalid email was provided<br />", pageResult.Url);
    }

    [Fact]
    public async Task ShouldShowAThankYouPageWithTheReferingUrlPassed()
    {
        // Arrange
        ThankYouMessageViewModel referer = new()
        {
            ReturnUrl = "this-is-a-referer"
        };

        // Act
        ViewResult pageResult = await _controller.ThankYouMessage(referer) as ViewResult; ;

        // Assert
        Assert.Equal("ThankYouMessage", pageResult.ViewName);
        Assert.Equal(referer.ReturnUrl, pageResult.Model.As<ThankYouMessageViewModel>().ReturnUrl);
    }

    [Fact]
    public async Task ShouldAddPageTitleToSubject()
    {
        // Act
        await _controller.Contact(_validContactDetails);

        // Assert
        _mockEmailClient.Verify(client => client.SendEmailToService(It.IsAny<EmailMessage>()));
    }
}