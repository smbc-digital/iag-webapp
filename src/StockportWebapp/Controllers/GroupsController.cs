using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Http;
using StockportWebapp.ViewModels;

namespace StockportWebapp.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = Cache.Short)]
    public class GroupsController : Controller
    {
        private readonly IProcessedContentRepository _repository;

        public GroupsController(IProcessedContentRepository repository)
        {
            _repository = repository;
        }

        [Route("/groups")]
        public async Task<IActionResult> Index()
        {            
            GroupStartPage model = new GroupStartPage();

            model.Categories = GetTemporaryHardCodedListOfCategories();

            return View(model);
        }

        public List<GroupCategory> GetTemporaryHardCodedListOfCategories()
        {
            var categories = new List<GroupCategory>();
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Sports and fitness"});
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Outdoors and Adventure" });
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Arts and Crafts" });
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Dancing" });
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Education and learning" });
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Breakfast Appreciation" });
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Badminton" });
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Computing" });
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Group Planning" });
            categories.Add(new GroupCategory() { Icon = "si-house", Name = "Group Planning" });

            return categories;
        }

        [Route("/groups/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var response = await _repository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            return View(group);
        }
    }
}
