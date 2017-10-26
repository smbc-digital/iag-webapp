using System;
using System.Collections.Generic;
using System.Linq;
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

            var result = await _httpClient.GetAsync(document.Url);
            var file = await result.Content.ReadAsByteArrayAsync();

            return new FileContentResult(file, "text/document");
        }
    }
}
