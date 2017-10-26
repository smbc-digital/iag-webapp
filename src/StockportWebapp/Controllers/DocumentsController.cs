using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Services;

namespace StockportWebapp.Controllers
{
    public class DocumentsController : Controller
    {
        private IDocumentsService _documentsService;

        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [Route("{businessId}/documents/{assetId}/{groupSlug}")]
        public IActionResult GetSecureDocument(string businessId, string assetId, string groupSlug)
        {
            var document = _documentsService.GetSecureDocument(businessId, assetId, groupSlug);

            return new OkObjectResult(document);
        }

    }
}
