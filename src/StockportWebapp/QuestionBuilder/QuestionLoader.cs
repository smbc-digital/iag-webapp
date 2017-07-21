using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using StockportWebapp.Helpers;
using StockportWebapp.QuestionBuilder.Entities;
using Newtonsoft.Json;
using StockportWebapp.Models;
using StockportWebapp.Repositories;

namespace StockportWebapp.QuestionBuilder
{
    public class QuestionLoader
    {
        private readonly IRepository _repository;
        private Dictionary<Type, string> _questionFiles = new Dictionary<Type, string>();

        public QuestionLoader(IRepository repository)
        {
            _repository = repository;
        }


        public TQuestionStructure LoadQuestions<TQuestionStructure>(string slug) where TQuestionStructure : IQuestionStructure, new()
        {
            var jsonQuestionSet = LoadJson<TQuestionStructure>(slug);
            var questionList = JsonConvert.DeserializeObject<IList<Page>>(jsonQuestionSet, new JsonConverter[]
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

        public string LoadJson<TQuestionStructure>(string questionSetFilename) where TQuestionStructure : IQuestionStructure, new()
        {
            var smart = _repository.Get<SmartAnswersSpike>("building-regs").Result.Content as SmartAnswersSpike;

            return smart.Questionjson;
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
