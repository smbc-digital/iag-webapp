using System.Net;
using System.Threading.Tasks;

namespace StockportWebapp.AmazonSES
{
    public class NoopHttpEmailClient:IHttpEmailClient
    {
        public Task<HttpStatusCode> SendEmailToService(string subject, string body, string serviceEmail, string userEmail = "")
        {
            return Task.FromResult(HttpStatusCode.BadRequest);
        }
    }
}