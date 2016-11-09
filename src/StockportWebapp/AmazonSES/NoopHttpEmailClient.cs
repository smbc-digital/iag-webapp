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

        //TODO: Remove this with the old contact page and the DynamicContactUsForm toggle
        public Task<HttpStatusCode> SendEmailDeprecated(string subject, string body, string userEmail = "")
        {
            return Task.FromResult(HttpStatusCode.BadRequest);
        }
    }
}