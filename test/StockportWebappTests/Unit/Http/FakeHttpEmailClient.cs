using System.Net;
using System.Threading.Tasks;
using StockportWebapp.AmazonSES;

namespace StockportWebappTests.Unit.Http
{
    public class FakeHttpEmailClient : IHttpEmailClient
    {
        public Task<HttpStatusCode> SendEmailToService(string subject, string body, string serviceEmail, string userEmail = "")
        {
            return Task.FromResult(HttpStatusCode.OK);
        }
    }
}
