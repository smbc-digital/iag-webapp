namespace StockportWebapp.ContentFactory;

public interface ITriviaFactory
{
    List<Trivia> Build(List<Trivia> triviaSection);
}

public class TriviaFactory : ITriviaFactory
{
    private readonly MarkdownWrapper _markdownWrapper;

    public TriviaFactory(MarkdownWrapper markdownWrapper) =>
        _markdownWrapper = markdownWrapper;

    public List<Trivia> Build(List<Trivia> triviaSection) => 
        triviaSection?.Select(item => new Trivia (
            item.Name,
            item.Icon,
            _markdownWrapper.ConvertToHtml(item.Body),
            item.Link
        )).ToList();
}