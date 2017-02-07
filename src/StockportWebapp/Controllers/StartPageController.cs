using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class StartPageController : Controller
    {
        private readonly IRepository _repository;

        public StartPageController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("/start/{slug}")]
        public async Task<IActionResult> Index(string slug)
        {
            var response = await _repository.Get<StartPage>(slug);

            if (!response.IsSuccessful()) return response;

            var startPage = response.Content as StartPage;

            return View(startPage);
        }
    }
}
