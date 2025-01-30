namespace StockportWebappTests_Unit.Unit.Validation;

public class ReCaptchaValidationTests
{
    public Mock<IApplicationConfiguration> _config = new();
    private readonly Mock<IHttpClient> _httpClient = new();
    private ValidateReCaptchaAttribute validationMethod;
    private ModelStateDictionary modelState;
    private ActionExecutingContext actionExcecutingContext;
    private readonly Mock<IFeatureManager> _featureManager = new();

    public ReCaptchaValidationTests()
    {
        _featureManager
            .Setup(manager => manager.IsEnabledAsync("EnableReCaptchaValidation"))
            .Returns(Task.FromResult(true));
        
        _config
            .Setup(conf => conf.GetReCaptchaKey())
            .Returns(AppSetting.GetAppSetting("recaptchakey"));
    }

    [Fact]
    public void ShouldReturnValidationSuccessWhenTokenIsCorrect()
    {
        // Arrange
        SetUpParameters();
        HttpResponseMessage responseMessage = new()
        {
            Content = new StringContent("{\"success\": true,\"challenge_ts\": \"2017-05-23T15:50:16Z\",\"hostname\": \"stockportgov.local\"}")
        };

        _httpClient
            .Setup(client => client.PostRecaptchaAsync("https://www.google.com/recaptcha/api/siteverify", It.IsAny<FormUrlEncodedContent>()))
            .ReturnsAsync(responseMessage);

        // Act
        Task response = validationMethod.OnActionExecutionAsync(actionExcecutingContext, null);

        // Assert
        Assert.True(modelState.IsValid);
    }

    [Fact]
    public void ShouldReturnValidationFailureWhenTokenIsIncorrect()
    {
        // Arrange
        SetUpParameters();
        HttpResponseMessage responseMessage = new()
        {
            Content = new StringContent("{\"success\": false,\"challenge_ts\": \"2017-05-23T15:50:16Z\",\"hostname\": \"stockportgov.local\"}")
        };

        _httpClient
            .Setup(client => client.PostRecaptchaAsync("https://www.google.com/recaptcha/api/siteverify", It.IsAny<FormUrlEncodedContent>()))
            .ReturnsAsync(responseMessage);

        // Act
        Task response = validationMethod.OnActionExecutionAsync(actionExcecutingContext, null);

        // Assert
        Assert.False(modelState.IsValid);
    }

    private void SetUpParameters()
    {
        validationMethod = new(_config.Object, _httpClient.Object, _featureManager.Object);
        modelState = new();
        DefaultHttpContext httpContenxt = new();

        Dictionary<string, StringValues> formRecaptchaToken = new()
        {
            { "g-recaptcha-response", "testValue" }
        };

        httpContenxt.Request.Form = new FormCollection(formRecaptchaToken);

        ActionContext actionContext = new(
            httpContenxt,
            new Mock<RouteData>().Object,
            new Mock<ActionDescriptor>().Object,
            modelState);

        actionExcecutingContext = new(actionContext,
                                    new List<IFilterMetadata>(),
                                    new Dictionary<string, object>(),
                                    new Mock<Controller>());
    }
}