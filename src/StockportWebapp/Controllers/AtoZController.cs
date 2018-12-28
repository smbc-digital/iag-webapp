using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.ViewModels;
using StockportWebapp.Http;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Long)]
    public class AtoZController : Controller
    {
        private readonly IRepository _repository;

        public AtoZController(IRepository repository)
        {
            _repository = repository;
        }

        [Route("/atoz/{letter}")]
        public async Task<IActionResult> Index(string letter)
        {
            if (IsNotInTheAlphabet(letter)) return NotFound();

            var httpResponse = await _repository.Get<List<AtoZ>>(letter);


            if (httpResponse.IsNotAuthorised())
                return new HttpResponse(500, "", "Error");

            var response = new List<AtoZ>();

            if (!httpResponse.IsSuccessful())
            {
                return new HttpResponse(500, "", "Error");
            }
            else
            {
                response = httpResponse.Content as List<AtoZ>;
            }

            var model = new AtoZViewModel
            {
                Items = response,
                CurrentLetter = letter.ToUpper(),
                Breadcrumbs = new List<Crumb>()
            };

            return View(model);
        }

        private static bool IsNotInTheAlphabet(string letter)
        {
            return string.IsNullOrEmpty(letter) || letter.Length != 1 || !char.IsLetter(letter[0]);
        }
    }
}
