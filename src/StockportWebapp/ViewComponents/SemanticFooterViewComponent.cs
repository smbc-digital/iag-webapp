using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.ViewComponents
{
    public class SemanticFooterViewComponent : ViewComponent
    {
        private readonly IRepository _repository;
        private readonly ILogger<SemanticFooterViewComponent> _logger;

        public SemanticFooterViewComponent(IRepository repository, ILogger<SemanticFooterViewComponent> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            _logger.LogInformation("Call to retrieve the footer");

            var footerHttpResponse = await _repository.Get<Footer>();

            if (!footerHttpResponse.IsSuccessful()) return await Task.FromResult(View("NoFooterFound"));

            var model = footerHttpResponse.Content as Footer;

            return await Task.FromResult(View("Semantic", model));
        }
    }
}