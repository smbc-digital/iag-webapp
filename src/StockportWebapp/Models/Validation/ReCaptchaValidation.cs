namespace StockportWebapp.Models.Validation;

public class ValidateReCaptchaAttribute : ActionFilterAttribute
{
    public const string ReCaptchaModelErrorKey = "ReCaptcha";
    private const string RecaptchaResponseTokenKey = "g-recaptcha-response";
    private const string ApiVerificationEndpoint = "https://www.google.com/recaptcha/api/siteverify";
    private readonly string _reCaptchaSecret;
    private readonly IHttpClient _httpClient;
    private readonly IFeatureManager _featureManager;

    public ValidateReCaptchaAttribute(IApplicationConfiguration config, IHttpClient httpClient, IFeatureManager featureManager = null)
    {
        _reCaptchaSecret = config.GetReCaptchaKey().ToString();
        _httpClient = httpClient;
        _featureManager = featureManager;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if(await _featureManager.IsEnabledAsync("EnableReCaptchaValidation")){
            await DoReCaptchaValidation(context);
        }

        await base.OnActionExecutionAsync(context, next);
    }

    private async Task DoReCaptchaValidation(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.HasFormContentType)
        {
            AddModelError(context, "No reCaptcha Token Found");
            return;
        }

        string token = context.HttpContext.Request.Form[RecaptchaResponseTokenKey];

        if (string.IsNullOrWhiteSpace(token))
        {
            AddModelError(context, "Verify you are not a robot by selecting the verification box above the \"submit\" button");
        }
        else
        {
            await ValidateRecaptcha(context, token);
        }
    }

    private static void AddModelError(ActionExecutingContext context, string error)
    {
        context.ModelState.AddModelError(ReCaptchaModelErrorKey, error.ToString());
    }

    private async Task ValidateRecaptcha(ActionExecutingContext context, string token)
    {
        var content = new FormUrlEncodedContent(new[]
        {
                new KeyValuePair<string, string>("secret", _reCaptchaSecret),
                new KeyValuePair<string, string>("response", token)
            });
        HttpResponseMessage response = await _httpClient.PostRecaptchaAsync(ApiVerificationEndpoint, content);
        string json = await response.Content.ReadAsStringAsync();
        var reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(json);
        if (reCaptchaResponse == null)
        {
            AddModelError(context, "Unable To Read Response From Server");
        }
        else if (!reCaptchaResponse.success)
        {
            AddModelError(context, "Invalid reCaptcha");
        }
    }
}

public class ReCaptchaResponse
{
    public bool success { get; set; }
    public string challenge_ts { get; set; }
    public string hostname { get; set; }
    public string[] errorcodes { get; set; }
}
