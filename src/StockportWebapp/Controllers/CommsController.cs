using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

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

            return View(commsHomepage);
        }
    }
}
