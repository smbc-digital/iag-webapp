using System.Collections.Generic;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder.Maps
{
    public interface IMap<T> where T : class
    {
        T Map(IList<Answer> answers);
    }
}
