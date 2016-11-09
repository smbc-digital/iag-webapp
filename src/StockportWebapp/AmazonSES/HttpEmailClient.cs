using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockportWebapp.Config;
using HttpClient = System.Net.Http.HttpClient;

namespace StockportWebapp.AmazonSES
{
    public interface IHttpEmailClient
    {        
        Task<HttpStatusCode> SendEmailToService(string subject, string body, string serviceEmail, string userEmail = "");
    }

    public class HttpEmailClient : IHttpEmailClient
    {
        private readonly AmazonAuthorizationHeader _authorizationHeader;
        private readonly ILogger<HttpEmailClient> _logger;
        private readonly IEmailConfigurationBuilder _emailConfigBuilder;
        private readonly BusinessId _businessId;
        private readonly Func<HttpClient> _httpClientMaker;
        private readonly string _contentType = "application/x-www-form-urlencoded";

        private readonly Regex _successfulResponseRegex =
            new Regex(@"<MessageId>([\w|-]*)</MessageId>[\s \S]*<RequestId>([\w|-]*)</RequestId>", RegexOptions.Compiled);

        public HttpEmailClient(AmazonAuthorizationHeader authorizationHeader, Func<HttpClient> httpClientMaker, 
            ILogger<HttpEmailClient> logger, IEmailConfigurationBuilder emailConfigBuilder, BusinessId businessId)
        {
            _authorizationHeader = authorizationHeader;
            _httpClientMaker = httpClientMaker;
            _logger = logger;
            _emailConfigBuilder = emailConfigBuilder;
            _businessId = businessId;
        }

        public async Task<HttpStatusCode> SendEmailToService(string emailsubject, string emailbody, string serviceEmail,
            string userEmail = "")
        {
            var config = _emailConfigBuilder.Build(_businessId.ToString());

            if (!config.IsValid())
            {
                _logger.LogError($"The Amazon SES client configuration is not valid. {config.ValidityToString()}");
                return HttpStatusCode.InternalServerError;
            }

            if (string.IsNullOrEmpty(serviceEmail))
            {
                _logger.LogError("ServiceEmail can not be null or empty. No email has been sent.");
                return HttpStatusCode.InternalServerError;
            }
            return await SendEmail(config, emailsubject, emailbody, serviceEmail, userEmail);
            
        }

        private async Task<HttpStatusCode> SendEmail(AmazonSesClientConfiguration config, string subject, string body, string serviceEmail,
            string userEmail = "")
        {
            var now = DateTime.UtcNow;
            var dateStamp = now.ToString("yyyyMMdd");
            var amzDate = now.ToString("yyyyMMddTHHmmssZ");

            using (var request = HttpRequest(config))
            {
                var httpClient = _httpClientMaker();
                var payload = BuildRequestPayload(config, subject, body, serviceEmail, userEmail);
                var authHeaders = _authorizationHeader.Create(config, payload, dateStamp, amzDate);

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authHeaders);
                request.Headers.Add("X-Amz-Date", amzDate);
                request.Content = new StringContent(payload, Encoding.UTF8, _contentType);

                var responseMessage = await httpClient.SendAsync(request);
                var response = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.IsSuccessStatusCode)
                {
                    LogSuccessfulResponse(response);
                }
                else
                {
                    LogErrorResponse(response);
                }

                return responseMessage.StatusCode;
            }
        }

        private void LogErrorResponse(string response)
        {
            _logger.LogError($"An error occurred trying to send an email to Amazon SES. \n{response}");
        }

        private void LogSuccessfulResponse(string response)
        {
            Match match = _successfulResponseRegex.Match(response);
            if (match.Success)
            {
                _logger.LogInformation(
                    $"An email was sent to Amazon SES with message id: {match.Groups[1]} and request id {match.Groups[2]}");
            }
            else
            {
                _logger.LogWarning("Could not extract message id or request id from Amazon SES response.");
            }
        }

        private HttpRequestMessage HttpRequest(AmazonSesClientConfiguration config)
        {
            return new HttpRequestMessage
            {
                RequestUri = new Uri(config.Endpoint),
                Method = HttpMethod.Post
            };
        }

        private string BuildRequestPayload(AmazonSesClientConfiguration config, string subject, string body, string serviceEmail, string userEmail)
        {

            var serviceEmails = serviceEmail.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            return "Action=SendEmail" +
                   GenerateToAddresses(serviceEmails) +
                   (string.IsNullOrWhiteSpace(userEmail)
                       ? ""
                       : "&Destination.CcAddresses.member.1=" + Uri.EscapeDataString(userEmail)) +
                   "&Message.Subject.Data=" + Uri.EscapeDataString(subject) +
                   "&Message.Body.Text.Data=" + Uri.EscapeDataString(body) +
                   "&Source=" + Uri.EscapeDataString(config.EmailFrom);
        }

        private string GenerateToAddresses(string[] serviceEmails)
        {
            int counter = 1;
            StringBuilder sb = new StringBuilder("");
            foreach (var serviceEmail in serviceEmails)
            {
                sb.Append("&Destination.ToAddresses.member." + counter.ToString() + "=" +
                          Uri.EscapeDataString(serviceEmail.Trim()));
                counter++;
            }
            return sb.ToString();
        }
    }
}