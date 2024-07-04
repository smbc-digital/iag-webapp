using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.Services;

public interface IProfileService
{
    Task<Profile> GetProfile(string slug);
}

public class ProfileService : IProfileService
{
    private readonly IRepository _repository;
    private readonly ITagParserContainer _parser;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly ITriviaFactory _triviaFactory;

    public ProfileService(
        IRepository repository,
        ITagParserContainer parser,
        MarkdownWrapper markdownWrapper,
        ITriviaFactory triviaFactory
    )
    {
        _repository = repository;
        _parser = parser;
        _markdownWrapper = markdownWrapper;
        _triviaFactory = triviaFactory;
    }

    public async Task<Profile> GetProfile(string slug)
    {
        HttpResponse httpResponse = await _repository.Get<Profile>(slug);

        if(!httpResponse.IsSuccessful())
            return null;

        Profile profile = httpResponse.Content as Profile;
        List<Trivia> triviaSection = _triviaFactory.Build(profile.TriviaSection);
        profile.TriviaSection = triviaSection;
        string processedBody = _parser.ParseAll(profile.Body, profile.Title, false, profile.InlineAlerts, null, profile.InlineQuotes, null, null);
        profile.Body = _markdownWrapper.ConvertToHtml(processedBody);

        return profile;
    }
}