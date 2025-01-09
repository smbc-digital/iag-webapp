using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.Services;

public interface IProfileService
{
    Task<Profile> GetProfile(string slug);
}

public class ProfileService(IRepository repository,
                            ITagParserContainer parser,
                            MarkdownWrapper markdownWrapper,
                            ITriviaFactory triviaFactory
                            ) : IProfileService
{
    private readonly IRepository _repository = repository;
    private readonly ITagParserContainer _parser = parser;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;
    private readonly ITriviaFactory _triviaFactory = triviaFactory;

    public async Task<Profile> GetProfile(string slug)
    {
        HttpResponse httpResponse = await _repository.Get<Profile>(slug);

        if(!httpResponse.IsSuccessful())
            return null;

        Profile profile = httpResponse.Content as Profile;
        List<Trivia> triviaSection = _triviaFactory.Build(profile.TriviaSection);
        profile.TriviaSection = triviaSection;
        string body = _markdownWrapper.ConvertToHtml(profile.Body ?? string.Empty);
        string processedBody = _parser.ParseAll(body ?? string.Empty, profile.Title, true, profile.InlineAlerts, null, profile.InlineQuotes, null, null);
        profile.Body = processedBody;
        
        return profile;
    }
}