using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using StockportWebapp.Utils;
using StockportWebapp.QuestionBuilder.Entities;
using Newtonsoft.Json;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.QuestionBuilder
{
    public class QuestionLoader
    {
        private readonly IRepository _repository;

        public QuestionLoader(IRepository repository)
        {
            _repository = repository;
        }


        public TQuestionStructure LoadQuestions<TQuestionStructure>(string slug, ref string title) where TQuestionStructure : IQuestionStructure, new()
        {
            var smartAnswer = LoadJson<TQuestionStructure>(slug);

            if (smartAnswer == null) return new TQuestionStructure();

            title = smartAnswer.Title;

            var questionList = JsonConvert.DeserializeObject<IList<Page>>(smartAnswer.Questionjson, new JsonConverter[]
            {
                new GenericJsonConverter<IQuestion, Question>(),
                new GenericJsonConverter<IBehaviour, Behaviour>() 
            });

            var questionStructure = new TQuestionStructure
            {
                Structure = BuildQuestionMapFromList(questionList)
            };

            return questionStructure;
        }

        public SmartAnswers LoadJson<TQuestionStructure>(string questionSetFilename) where TQuestionStructure : IQuestionStructure, new()
        {
            var smart = _repository.Get<SmartAnswers>(questionSetFilename).Result.Content;

            var question = smart as SmartAnswers;

            return question;
        }

        private ImmutableDictionary<int, Page> BuildQuestionMapFromList(IList<Page> questionList)
        {
            return questionList.Aggregate(new Dictionary<int, Page>(), (outVal, pageEntry) =>
            {
                outVal.Add(pageEntry.PageId, pageEntry);
                return outVal;
            }).ToImmutableDictionary();
        }


    }
}
