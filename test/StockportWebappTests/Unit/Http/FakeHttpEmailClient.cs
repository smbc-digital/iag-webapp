namespace StockportWebappTests_Unit.Unit.Http;

public class FakeHttpEmailClient : IHttpEmailClient
{
    public Task<HttpStatusCode> SendEmailToService(EmailMessage emailMessage) =>
        Task.FromResult(HttpStatusCode.OK);

    public string GenerateEmailBodyFromHtml<T>(T details, string templateName = null) =>
        string.Empty;
}