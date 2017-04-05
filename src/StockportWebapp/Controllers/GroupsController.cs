using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly IProcessedContentRepository _processedContentRepository;
        private readonly IRepository _repository;
        private readonly IGroupRepository _groupRepository;

        public GroupsController(IProcessedContentRepository processedContentRepository, IRepository repository, IGroupRepository groupRepository)
        {
            _processedContentRepository = processedContentRepository;
            _repository = repository;
            _groupRepository = groupRepository;
        }

        [Route("/groups")]
        public async Task<IActionResult> Index()
        {            
            GroupStartPage model = new GroupStartPage();
            var response = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = response.Content as List<GroupCategory>;

            if (listOfGroupCategories != null)
            {
                model.Categories = listOfGroupCategories;
            }          

            return View(model);
        }

        [Route("/groups/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var response = await _processedContentRepository.Get<Group>(slug);

            if (!response.IsSuccessful()) return response;

            var group = response.Content as ProcessedGroup;

            return View(group);
        }

        [Route("/groups/add-a-group")]
        public async Task<IActionResult> AddAGroup()
        {
            var groupSubmission = new GroupSubmission();
            var response = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = response.Content as List<GroupCategory>;
            if (listOfGroupCategories != null)
            {
                groupSubmission.Categories = listOfGroupCategories.Select(logc => logc.Name).ToList();
            }

            return View(groupSubmission);
        }

        [HttpPost]
        [Route("/groups/add-a-group")]
        public async Task<IActionResult> AddAGroup(GroupSubmission groupSubmission)
        {
            var response = await _repository.Get<List<GroupCategory>>();
            var listOfGroupCategories = response.Content as List<GroupCategory>;
            if (listOfGroupCategories != null)
            {
                groupSubmission.Categories = listOfGroupCategories.Select(logc => logc.Name).ToList();
            }

            if (!ModelState.IsValid) return View(groupSubmission);

            var successCode = await _groupRepository.SendEmailMessage(groupSubmission);
            if (successCode == HttpStatusCode.OK) return RedirectToAction("ThankYouMessage");

            ViewBag.SubmissionError = "There was a problem submitting the group, please try again.";

            return View(groupSubmission);
        }

        [Route("/groups/thank-you-message")]
        public IActionResult ThankYouMessage()
        {
            return View();
        }
    }
}
