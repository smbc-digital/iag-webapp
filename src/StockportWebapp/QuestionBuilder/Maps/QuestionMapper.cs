using System.Collections.Generic;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Maps
{
    public class QuestionMapper<T> where T : class
    {
        private readonly IMap<T> _mapper;

        public QuestionMapper(IMap<T> map)
        {
            _mapper = map;
        }

        public T MapFromAnswers(IList<Answer> answers)
        {
            return _mapper.Map(answers);
        }
    }
}
