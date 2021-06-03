using System.Collections.Generic;
using System.Threading.Tasks;
using StockportWebapp.Config;
using StockportWebapp.ContentFactory;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Repositories
{
    public interface IDocumentPageRepository
    {
        Task<HttpResponse> Get(string slug = "");
    }

    public class DocumentPageRepository : IDocumentPageRepository
    {
        private readonly DocumentPageFactory _documentPageFactory;
        private readonly IHttpClient _httpClient;
        private readonly IStubToUrlConverter _urlGenerator;
        private readonly Dictionary<string, string> authenticationHeaders;
        private readonly IApplicationConfiguration _config;

        public DocumentPageRepository(IStubToUrlConverter urlGenerator, IHttpClient httpClient, DocumentPageFactory documentPageFactory, IApplicationConfiguration config)
        {
            _urlGenerator = urlGenerator;
            _httpClient = httpClient;
            _documentPageFactory = documentPageFactory;
            _config = config;
            authenticationHeaders = new Dictionary<string, string> { { "Authorization", _config.GetContentApiAuthenticationKey() }, { "X-ClientId", _config.GetWebAppClientId() } };
        }

        public async Task<HttpResponse> Get(string slug = "")
        {
            var url = _urlGenerator.UrlFor<DocumentPage>(slug);
            var httpResponse = await _httpClient.Get(url, authenticationHeaders);

            if (!httpResponse.IsSuccessful())
                return httpResponse;

            var model = HttpResponse.Build<DocumentPage>(httpResponse);
            var documentPage = (DocumentPage)model.Content;
            var processedModel = _documentPageFactory.Build(documentPage);

            return HttpResponse.Successful(200, processedModel);
        }
    }
}
