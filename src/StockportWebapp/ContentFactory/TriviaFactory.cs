namespace StockportWebapp.ContentFactory;

public interface ITriviaFactory
{
    List<ProcessedTrivia> Build(List<Trivia> triviaSection);
}

public class TriviaFactory : ITriviaFactory
{
    private readonly MarkdownWrapper _markdownWrapper;

    public TriviaFactory(MarkdownWrapper markdownWrapper)
    {
        _markdownWrapper = markdownWrapper;
    }

    public List<ProcessedTrivia> Build(List<Trivia> triviaSection)
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
