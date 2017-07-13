using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using StockportWebapp.Helpers;
using StockportWebapp.QuestionBuilder.Entities;
using Newtonsoft.Json;

namespace StockportWebapp.QuestionBuilder
{
    public static class QuestionLoader
    {
        private static Dictionary<Type, string> _questionFiles = new Dictionary<Type, string>();

        public static TQuestionStructure LoadQuestions<TQuestionStructure>(string questionSetFilename) where TQuestionStructure : IQuestionStructure, new()
        {
            var jsonQuestionSet = LoadJson<TQuestionStructure>(questionSetFilename);
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

        private static string LoadJson<TQuestionStructure>(string questionSetFilename) where TQuestionStructure : IQuestionStructure, new()
        {
            if (!_questionFiles.ContainsKey(typeof(TQuestionStructure)))
            {
                _questionFiles.Add(typeof(TQuestionStructure), File.ReadAllText($"QuestionBuilder/QuestionSets/{questionSetFilename}"));
            }

            return _questionFiles[typeof(TQuestionStructure)];
        }

        private static ImmutableDictionary<int, Page> BuildQuestionMapFromList(IList<Page> questionList)
        {
            return questionList.Aggregate(new Dictionary<int, Page>(), (outVal, pageEntry) =>
            {
                outVal.Add(pageEntry.PageId, pageEntry);
                return outVal;
            }).ToImmutableDictionary();
        }


    }
}
