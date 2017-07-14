using System.Collections.Immutable;
using StockportWebapp.QuestionBuilder.Entities;

namespace StockportWebapp.QuestionBuilder
{
    public class BuildingRegsGarageQuestions : IBuildingRegsGarageQuestions
    {
        public ImmutableDictionary<int, Page> Structure { get; set; }
    }
}
