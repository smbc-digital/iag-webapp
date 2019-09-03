using StockportWebapp.Enums;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Maps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockportWebapp.Http;
using StockportWebapp.ViewModels;
using StockportWebapp.FeatureToggling;
using StockportWebapp.Utils;

namespace StockportWebapp.QuestionBuilder
{
    public abstract class BaseQuestionController<T, M> : Controller, IQuestionNavigator
        where T : class, IMappable
        where M : IMap<T>
    {
        protected IDictionary<int, Page> Structure { get; set; }
        protected IHttpContextAccessor HttpContextAccessor;
        protected string Slug;
        protected string Title;
        private readonly IHttpClient _client;
        private readonly FeatureToggles _featureToggles;
        private readonly IConfiguration _config;
        private readonly ILogger<BaseQuestionController<T, M>> _logger;
        private readonly ISmartAnswerStringHelper SmartAnswerStringHelper;

        protected BaseQuestionController(IHttpContextAccessor httpContextAccessor, QuestionLoader questionLoader, FeatureToggles featureToggles, IHttpClient client, IConfiguration config, ILogger<BaseQuestionController<T, M>> logger, ISmartAnswerStringHelper smartAnswerStringHelper)
        {

            var slug = string.Empty;
            var url = httpContextAccessor.HttpContext.Request.Path.ToString();
            if (url.IndexOf("smart/") > 0)
            {
                var start = url.IndexOf("smart/") + 6;
                var end = url.IndexOf("?", start) > start ? url.IndexOf("?", start) : url.Length;
                slug = url.Substring(start, end - start);
                slug = slug.Replace("/validate", string.Empty);
            }
            else
            {
                slug = url;
            }

            HttpContextAccessor = httpContextAccessor;
            _client = client;
            _featureToggles = featureToggles;
            _config = config;
            _logger = logger;
            Structure = questionLoader.LoadQuestions<GenericSmartAnswersQuestions>(slug, ref Title).Structure;
            Slug = slug;
            SmartAnswerStringHelper = smartAnswerStringHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Index(Page page)
        {
            if (page.Questions.Count > 0)
            {
                var tempJson = page.PreviousAnswersJson;
                page = ProcesssPage(page);

                if (tempJson != null)
                {
                    page.PreviousAnswers = JsonConvert.DeserializeObject<IList<Answer>>(tempJson);
                    page.PreviousAnswersJson = tempJson;
                }

                page.ValidateQuestions();

                if (page.HasValidationErrors())
                {
                    var model = new SmartAnswerViewModel
                    {
                        Page = page,
                        Slug = Slug,
                        Title = Title
                    };

                    return View(model);
                }
            }

            var action = await RunBehaviours(page);

            if (!page.ShouldCache)
            {
                AddNoCacheHeaders(HttpContextAccessor);
            }

            ResetFormElements();
            return action;
        }

        private void AddNoCacheHeaders(IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext.Response.Headers.Add("Cache-Control", "no-store, must-revalidate, max-age=0");
            httpContextAccessor.HttpContext.Response.Headers.Add("Pragma", "no-cache");
        }

        [HttpPost]
        [Route("validate")]
        public JsonResult Validate(Page page)
        {
            page = ProcesssPage(page);
            page.ValidateQuestions();
            return new JsonResult(page.GetValidationResults());
        }

        [HttpPost]
        [Route("submitanswers")]
        public IActionResult SubmitAnswers(Page page)
        {
            var results = GetMappedResult(page.GetCombinedAnswers());
            return ProcessResults(results, page.Endpoint);
        }

        public abstract IActionResult ProcessResults(T result, string endpointName);

        public IActionResult DefaultBehaviour(int currentPageId)
        {
            Page nextPage = GetPage(currentPageId + 1);
            return View(nextPage);
        }

        public async Task<IActionResult> RunBehaviours(Page page)
        {
            SmartAnswerStringHelper smartAnswerStringHelper = new SmartAnswerStringHelper();

            var allAnswers = page.GetCombinedAnswers();

            IBehaviour behaviour = null;

            if (page.Behaviours != null)
            {
                // Find the behaviour that should trigger, if any...
                behaviour = page.Behaviours.FirstOrDefault(b => b.ShouldTrigger(allAnswers));
            }

            if (behaviour != null)
            {
                page.AddAnswers(allAnswers);
                page.PreviousAnswersJson = JsonConvert.SerializeObject(page.PreviousAnswers);

                switch (behaviour.BehaviourType)
                {
                    case EQuestionType.Redirect:
                        return Redirect(behaviour.Value);
                    case EQuestionType.RedirectToActionController:
                        return RedirectToAction("Article", "Article", new { articleSlug = behaviour.Value });
                    case EQuestionType.RedirectToAction:
                        return RedirectToAction(behaviour.Value);
                    case EQuestionType.GoToSummary:
                        ViewData["page"] = page;
                        ViewData["pageTitle"] = Title;
                        return View("Summary");
                    case EQuestionType.GoToPage:
                        page = GetPage(Convert.ToInt32(behaviour.Value));
                        page.AddAnswers(allAnswers);
                        break;
                    case EQuestionType.HandOffData:
                        _logger.LogInformation("------Before config");
                        var authenticationKey = _config["DTSHandOffAuthenticationKey"];
                        _logger.LogInformation($"------Authentication key: {authenticationKey}");

                        _logger.LogInformation($"------{behaviour.Value}");
                        try
                        {
                            var guid = await _client.PostAsyncMessage($"{behaviour.Value}", new StringContent(page.PreviousAnswersJson, Encoding.UTF8, "application/json"), new Dictionary<string, string> { { "DTSHandOffAuthenticationKey", authenticationKey } });
                            _logger.LogInformation($"------{guid ?? null}");
                            if (string.IsNullOrEmpty(guid.Content.ReadAsStringAsync().Result))
                            {
                                _logger.LogInformation($"Guid not set");
                            }
                            else
                            {
                                //_logger.LogInformation($"Redirect url ==== {behaviour.Value}date?guid={JsonConvert.DeserializeObject(guid.Content.ReadAsStringAsync().Result)}");
                                return Redirect($"{behaviour.RedirectValue}?guid={JsonConvert.DeserializeObject(guid.Content.ReadAsStringAsync().Result)}");
                            }
                                
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation($"------{e}");
                            throw;
                        }
                        break;
                }
            }
            else
            {
                if (!page.IsLastPage)
                {
                    page = GetNextPage(page.PageId);
                }
                page.AddAnswers(allAnswers);
            }

            var result = new SmartAnswerViewModel
            {
                Page = page,
                Slug = Slug,
                Title = Title
            };

            result.Page.PreviousAnswersJson = JsonConvert.SerializeObject(page.PreviousAnswers);

            result.Page.Description = SmartAnswerStringHelper.DescriptionTextParser(result.Page.Description, result.Page.PreviousAnswers);

            if (_featureToggles.SemanticLayout && _featureToggles.SemanticSmartAnswer.Contains(result.Slug))
            {
                return View("Semantic/Index", result);
            }
            return View(result);
        }

        public Page GetPage(int pageId)
        {
            var page = new Page();

            if (Structure.ContainsKey(pageId))
            {
                page = Structure[pageId];
                //page.Reset();
            }

            return page;
        }

        public Page GetNextPage(int currentPageId)
        {
            return GetPage(currentPageId + 1);
        }

        private Page ProcesssPage(Page pageToProcess)
        {
            var fullPage = GetPage(pageToProcess.PageId);
            pageToProcess.Questions.ToList().ForEach(postedQuestion =>
            {
                var fullQuestion = fullPage.Questions.FirstOrDefault(x => x.QuestionId == postedQuestion.QuestionId);
                fullQuestion.Response = postedQuestion.Response;
            });
            fullPage.PreviousAnswers = pageToProcess.PreviousAnswers;

            return fullPage;
        }

        private void ResetFormElements()
        {
            ModelState.Clear();
        }

        private T GetMappedResult(IList<Answer> answers)
        {
            var mapper = new QuestionMapper<T>(Activator.CreateInstance<M>());
            return mapper.MapFromAnswers(answers);
        }

    }
}

