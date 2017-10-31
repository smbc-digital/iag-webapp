using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Services;
using StockportWebapp.Wrappers;

namespace StockportWebapp.Controllers
{
    public class DocumentsController : Controller
    {
        private IDocumentsService _documentsService;

        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [Route("documents/{groupSlug}/{assetId}")]
        public async Task<IActionResult> GetSecureDocument(string groupSlug, string assetId)
        {
            var result = await _documentsService.GetSecureDocument(assetId, groupSlug);

            if (result == null) return new NotFoundObjectResult($"No document found for assetId: {assetId}");

            return new FileContentResult(result.FileData, result.MediaType);
        }
    }
}
