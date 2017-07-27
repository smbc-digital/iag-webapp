using System.Collections.Generic;
using StockportWebapp.Dtos;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Maps
{
    public class GenericSmartAnswersMap : IMap<GenericSmartAnswersModel>
    {
        public GenericSmartAnswersModel Map(IList<Answer> answers)
        {
            var request = new GenericSmartAnswersModel();

            return request;
        }
    }
}
