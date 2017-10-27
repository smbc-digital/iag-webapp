using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Services;
using System.Net.Http;

namespace StockportWebapp.Controllers
{
    public class DocumentsController : Controller
    {
        private IDocumentsService _documentsService;
        private HttpClient _httpClient;

        public DocumentsController(IDocumentsService documentsService, HttpClient httpClient)
        {
            _documentsService = documentsService;
            _httpClient = new HttpClient();
        }

        [Route("documents/{assetId}/{groupSlug}")]
        public async Task<IActionResult> GetSecureDocument(string assetId, string groupSlug)
        {
            var document = await _documentsService.GetSecureDocument(assetId, groupSlug);

            var result = document != null ? await _httpClient.GetAsync($"https:{document.Url}") : null;

            if (result == null) return new NotFoundObjectResult($"No document found for assetId: {assetId}");

            var file = await result.Content.ReadAsByteArrayAsync();

            return new FileContentResult(file, document.MediaType);
        }
    }
}
