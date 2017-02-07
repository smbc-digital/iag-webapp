using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class GroupController : Controller
    {
        private readonly IProcessedContentRepository _repository;

        public GroupController(IProcessedContentRepository repository)
        {
            _repository = repository;
        }

        [Route("/group/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var response = await _repository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            return View(group);
        }
    }
}
