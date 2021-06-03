using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StockportWebapp.QuestionBuilder.Validators;

namespace StockportWebapp.QuestionBuilder.Entities
{
    // Because we are deserialising Page, and we also want to mock Questions (so we want to use IQuestion)
    // We need to be able to specify a concrete type to deserialize
    // But we also need to use Interfaces for mocking.
    // To do this we added the TQuestion type parameter extending IQuestion
    // This allows us to specify the concrete class when required
    // But also use mocks where appropriate.
    public class Page
    {

        public Page()
        {
        }

        [JsonConstructor]
        public Page(int pageId, string analyticsEvent = "", string description = "", IList<Question> questions = null, 
            IList<Behaviour> behaviours = null, bool isLastPage = false, bool shouldCache = true, bool hideBackButton = false, InlineAlert alert = null, string typeformUrl = "")
        {
            PageId = pageId;
            AnalyticsEvent = analyticsEvent;
            Questions = questions;
            Description = description;
            Behaviours = behaviours;
            IsLastPage = isLastPage;
            ShouldCache = shouldCache;
            HideBackButton = hideBackButton;
            Alert = alert;
            TypeformUrl = typeformUrl;
        }

        public int PageId { get; set; }

        private IList<Question> _questions;
        public IList<Question> Questions
        {
            get
            {
                _questions = _questions ?? new List<Question>();
                return _questions;
            }
            set => _questions = value;
        }

        private IList<Answer> _previousAnswers;
        public IList<Answer> PreviousAnswers
        {
            get
            {
                _previousAnswers = _previousAnswers ?? new List<Answer>();
                return _previousAnswers;
            }
            set { _previousAnswers = value; }
        }

        public string PreviousAnswersJson { get; set; }
        public string AnalyticsEvent { get; set; }
        public string Description { get; set; }
        public IList<Behaviour> Behaviours { get; }
        public bool IsLastPage { get; set; }
        public string ButtonText { get; set; }
        public string Endpoint { get; set; }
        public string Action => IsLastPage ? "submitanswers" : null;
        public bool DisplayNextButton => !Questions.Any(_ => _.QuestionType == null || _.QuestionType.Equals("redirect"));
        public string ButtonCssClass => IsLastPage ? "button-loading" : "";
        public bool HideBackButton { get; set; }
        public InlineAlert Alert { get; set; }
        public bool ShouldCache { get; set; }
        public string TypeformUrl { get; set; }

        public void AddAnswers(List<Answer> answers)
        {
            var answersList = new List<Answer>();
           
            answers.ToList().ForEach(a =>
            {
                var existingAnswer = PreviousAnswers.FirstOrDefault(p => p.QuestionId == a.QuestionId);
                if (existingAnswer != null)
                {
                    existingAnswer.Response = a.Response;
                }
                else
                {
                    PreviousAnswers.Add(a);
                }
            });
        }

        public List<Answer> GetCurrentAnswers()
        {
            var answersList = new List<Answer>();

            foreach (var question in Questions)
            {
                if (question.QuestionType == "checkbox")
                {
                    var checkboxResponses = question.Response.Split(',');
                    foreach (var response in checkboxResponses)
                    {
                        answersList.Add(new Answer
                        {
                            QuestionId = response,
                            Response = "true",
                            QuestionText = response
                        });
                    }
                }
                else
                {
                    answersList.Add(new Answer
                    {
                        QuestionId = question.QuestionId,
                        Response = question.Response,
                        QuestionText = question.Label
                    });
                }
            }

            return answersList;
        }
       

        /// <summary>
        /// Gets a combined list of current and previous answers
        /// </summary>
        /// <returns></returns>
        public List<Answer> GetCombinedAnswers()
        {
            return PreviousAnswers.Concat(GetCurrentAnswers()).ToList();
        }

        public void ValidateQuestions()
        {
            Questions.ToList().ForEach(q => q.Validate());
        }

        public List<ValidationResult> GetValidationResults()
        {
            return Questions.Select(q => q.ValidationResult).ToList();
        }

        public bool HasValidationErrors()
        {
            return GetValidationResults().Any(_ => !_.IsValid);
        }

        public void Reset()
        {
            PreviousAnswers = new List<Answer>();
            Questions.ToList().ForEach(q => q.Response = string.Empty);
        }
    }
}