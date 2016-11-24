using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly IRepository _repository;

        public FooterViewComponent(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerHttpResponse = await _repository.Get<Footer>();

            if (!footerHttpResponse.IsSuccessful()) return await Task.FromResult(View("NoFooterFound"));

            var model = footerHttpResponse.Content as Footer;

            return await Task.FromResult(View(model));
        }
    }
}