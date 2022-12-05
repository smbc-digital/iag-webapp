using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ContentFactory.Trivia
{
    public interface ITriviaFactory
    {
        List<ProcessedTrivia> Build(List<Models.Trivia> triviaSection);
    }
}
