using System.Collections.Generic;
using System.Linq;
using StockportWebapp.QuestionBuilder.Validators;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

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
        public Page(
                int pageId,
                string analyticsEvent = "",
                string description = "",
                IList<Question> questions = null,
                IList<Behaviour> behaviours = null,
                bool isLastPage = false,
                bool shouldCache = true)
        {
            PageId = pageId;
            AnalyticsEvent = analyticsEvent;
            Description = description;
            Behaviours = behaviours;
            IsLastPage = isLastPage;
            Questions = questions;
            ShouldCache = shouldCache;
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
            set { _questions = value; }
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
        public string Description { get; }
        public IList<Behaviour> Behaviours { get; }
        public bool IsLastPage { get; set; }
        public string ButtonText { get; set; }
        public string Endpoint { get; set; }
        public string Action => IsLastPage ? "submitanswers" : null;
        public bool DisplayNextButton => !Questions.Any(_ => _.QuestionType == null || _.QuestionType.Equals("redirect"));
        public string ButtonCssClass => IsLastPage ? "button-loading" : "";
        public bool ShouldCache { get; set; }

        public void AddAnswers(List<Answer> answers)
        {
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
            return Questions.Select(q => new Answer
            {
                QuestionId = q.QuestionId,
                Response = q.Response
            }).ToList();
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