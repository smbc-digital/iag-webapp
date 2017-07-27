using StockportWebapp.Enums;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Maps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.ViewModels;

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

        protected BaseQuestionController(IHttpContextAccessor httpContextAccessor, QuestionLoader questionLoader)
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
            Structure = questionLoader.LoadQuestions<GenericSmartAnswersQuestions>(slug, ref Title).Structure;
            Slug = slug;
        }

        [HttpPost]
        public IActionResult Index(Page page)
        {
            if (page.Questions.Count > 0)
            {
                page = ProcesssPage(page);
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

            var action = RunBehaviours(page);             
                  
            if(!page.ShouldCache)
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
        public Task<IActionResult> SubmitAnswers(Page page)
        {
            var results = GetMappedResult(page.GetCombinedAnswers());            
            return ProcessResults(results, page.Endpoint);
        }

        public abstract Task<IActionResult> ProcessResults(T result, string endpointName);

        public IActionResult DefaultBehaviour(int currentPageId)
        {
            Page nextPage = GetPage(currentPageId + 1);
            return View(nextPage);
        }

        public IActionResult RunBehaviours(Page page)
        {
            var allAnswers = page.GetCombinedAnswers();
            IBehaviour behaviour = null;

            if (page.Behaviours != null)
            {
                // Find the behaviour that should trigger, if any...
                behaviour = page.Behaviours.FirstOrDefault(b =>
                {
                    return b.ShouldTrigger(allAnswers);
                });
            }

            if (behaviour != null)
            {
                if (behaviour.BehaviourType == EQuestionType.Redirect)
                {
                    return Redirect(behaviour.Value);
                }
                if (behaviour.BehaviourType == EQuestionType.RedirectToActionController)
                {
                    return RedirectToAction("Article", "Article", new { articleSlug = behaviour.Value });
                }
                if (behaviour.BehaviourType == EQuestionType.RedirectToAction)
                {
                    return RedirectToAction(behaviour.Value);
                }
                page = GetPage(Convert.ToInt32(behaviour.Value));
            }
            else
            {
                if (!page.IsLastPage)
                {
                    page = GetNextPage(page.PageId);
                }
            }

            page.AddAnswers(allAnswers);

            var result = new SmartAnswerViewModel();
            result.Page = page;
            result.Slug = Slug;
            result.Title = Title;
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
