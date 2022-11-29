using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory.Trivia
{
    public class TriviaFactory : ITriviaFactory
    {
        private readonly MarkdownWrapper _markdownWrapper;

        public TriviaFactory(MarkdownWrapper markdownWrapper)
        {
            _markdownWrapper = markdownWrapper;
        }

        public List<ProcessedTrivia> Build(List<Models.Trivia> triviaSection)
        {
            return triviaSection?.Select(item => new ProcessedTrivia
            (
                item.Name,
                item.Icon,
                _markdownWrapper.ConvertToHtml(item.Text),
                item.Link
            )).ToList();
        }
    }
}
