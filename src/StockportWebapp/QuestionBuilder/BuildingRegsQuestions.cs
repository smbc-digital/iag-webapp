using System.Collections.Immutable;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder
{
    public class BuildingRegsQuestions : IBuildingRegsQuestions
    {
        public ImmutableDictionary<int, Page> Structure { get; set; }
    }
}
