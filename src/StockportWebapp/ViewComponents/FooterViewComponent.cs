using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly IRepository _repository;
        private readonly FeatureToggles _featureToggles;

        public FooterViewComponent(IRepository repository, FeatureToggles featureToggles)
        {
            _repository = repository;
            _featureToggles = featureToggles;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_featureToggles.DynamicFooter) return await Task.FromResult(View("Old"));

            var footerHttpResponse = await _repository.Get<Footer>();
            if (!footerHttpResponse.IsSuccessful())
            //    return await Task.FromResult(View());
                return await Task.FromResult(View("NoFooterFound"));

            var model = footerHttpResponse.Content as Footer;

            return await Task.FromResult(View(model));
        }
    }
}