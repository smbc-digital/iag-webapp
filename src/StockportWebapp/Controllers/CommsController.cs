using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class CommsController : Controller
    {
        private readonly IRepository _repository;

        public CommsController(IRepository repository)
        {
            _repository = repository;
        }

        [Route("/news-room")]
        public async Task<IActionResult> Index()
        {
            var response = await _repository.Get<CommsHomepage>(queries: new List<Query>());

            var commsHomepage = response.Content as CommsHomepage;
            var latestNewsResponse = await _repository.GetLatest<List<News>>(1);
            var latestNews = latestNewsResponse.Content as List<News>;

            var viewModel = new CommsHomepageViewModel
            {
                Homepage = commsHomepage,
                LatestNews = latestNews?.FirstOrDefault()
            };

            return View(viewModel);
        }
    }
}
