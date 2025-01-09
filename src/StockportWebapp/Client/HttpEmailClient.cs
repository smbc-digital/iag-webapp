namespace StockportWebapp.Client;

public interface IHttpEmailClient
{
    Task<HttpStatusCode> SendEmailToService(EmailMessage emailMessage);
    string GenerateEmailBodyFromHtml<T>(T details, string templateName = null);
}

[ExcludeFromCodeCoverage]
public class HttpEmailClient(ILogger<HttpEmailClient> logger,
                            IEmailBuilder emailBuilder,
                            IAmazonSimpleEmailService amazonSimpleEmailService,
                            bool sendAmazonEmails) : IHttpEmailClient
{
    private readonly IAmazonSimpleEmailService _amazonSimpleEmailService = amazonSimpleEmailService;
    private readonly ILogger<HttpEmailClient> _logger = logger;
    private readonly IEmailBuilder _emailBuilder = emailBuilder;
    private readonly bool _sendAmazonEmails = sendAmazonEmails;

    public async Task<HttpStatusCode> SendEmailToService(EmailMessage emailMessage)
    {
        if (string.IsNullOrEmpty(emailMessage.ToEmail))
        {
            _logger.LogError("ToEmail can not be null or empty. No email has been sent.");
            return HttpStatusCode.InternalServerError;
        }

        SendRawEmailResponse result = await SendEmail(emailMessage);

        return result.HttpStatusCode;
    }

    public string GenerateEmailBodyFromHtml<T>(T details, string templateName = null)
    {
        string template = !string.IsNullOrEmpty(templateName) ? templateName : typeof(T).Name;
        string layout = GetEmailTemplateForLayout();
        string body = GetEmailTemplateForBody(template);

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            string tag = $"{{{{ {property.Name.ToLower()} }}}}";
            tag = tag.Replace("\r\n", "<br />").Replace("\r", "<br />").Replace("\n", "<br />");
            
            object value = property.GetValue(details, null) is null
                ? string.Empty
                : property.GetValue(details, null);

            if (property.PropertyType.Equals(typeof(List<string>)))
                if (value is List<string> items)
                    value = string.Join(", ", items.ToArray()).Trim().TrimEnd(',');
            else
                value = value.ToString().Replace("\r\n", "<br />");

            body = body.Replace(tag, value.ToString());
        }

        string result = layout.Replace("{{ MAIN_BODY }}", body);

        return result;
    }

    private static string GetEmailTemplateForLayout()
        => new FileReader().GetStringResponseFromFile("StockportWebapp.EmailTemplates._Layout.html");

    private static string GetEmailTemplateForBody(string template)
        => new FileReader().GetStringResponseFromFile($"StockportWebapp.EmailTemplates.{template}.html");

    private async Task<SendRawEmailResponse> SendEmail(EmailMessage emailMessage)
    {
        SendRawEmailRequest sendRequest = new()
        {
            RawMessage = new RawMessage(_emailBuilder.BuildMessageToStream(emailMessage))
        };

        try
        {
            SendRawEmailResponse response = new() { HttpStatusCode = HttpStatusCode.OK };

            if (_sendAmazonEmails)
                response = await _amazonSimpleEmailService.SendRawEmailAsync(sendRequest);

            LogResponse(response);

            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError($"An error occurred trying to send an email to Amazon SES. \n{exception.Message}");
            return new SendRawEmailResponse { HttpStatusCode = HttpStatusCode.BadRequest };
        }
    }

    private void LogResponse(SendRawEmailResponse response)
    {
        if (response.HttpStatusCode == HttpStatusCode.OK)
            _logger.LogInformation($"An email was sent to Amazon SES with message id: {response.MessageId} and request id {response.ResponseMetadata?.RequestId}");
        else
            _logger.LogWarning($"There was a problem sending an email, message id: {response.MessageId} and request id: {response.ResponseMetadata?.RequestId} and status code {response.HttpStatusCode}");
    }
}