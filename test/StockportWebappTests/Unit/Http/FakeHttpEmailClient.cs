using System.Net;
using System.Threading.Tasks;
using StockportWebapp.AmazonSES;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Http
{
    public class FakeHttpEmailClient : IHttpEmailClient
    {
        public Task<HttpStatusCode> SendEmailToService(EmailMessage emailMessage)
        {
            return Task.FromResult(HttpStatusCode.OK);
        }

        public string GenerateEmailBodyFromHtml<T>(T details, string templateName = null)
        {
            return "";
        }
    }
}
