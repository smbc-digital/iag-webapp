using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.TagParsers;

public class ProfileTagParser : IDynamicTagParser<Profile>
{
    private readonly IViewRender _viewRenderer;

    public ProfileTagParser(IViewRender viewRenderer) => _viewRenderer = viewRenderer;

    protected Regex TagRegex => new Regex("{{PROFILE:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string content, IEnumerable<Profile> profiles)
    {
        var matches = TagRegex.Matches(content);

        foreach (Match match in matches)
        {
            var tagDataIndex = 1;
            var profileSlug = match.Groups[tagDataIndex].Value;
            var profile = GetProfileMatchingSlug(profiles, profileSlug);
            if (profile != null)
            {
                var profileHtml = string.IsNullOrEmpty(profile.Body)
                    ? _viewRenderer.Render("ProfileWithoutBody", profile)
                    : _viewRenderer.Render("Profile", profile);

                content = TagRegex.Replace(content, profileHtml, 1);
            }
        }
        return RemoveEmptyTags(content);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);

    private Profile GetProfileMatchingSlug(IEnumerable<Profile> profiles, string slug) =>
        profiles?.FirstOrDefault(s => s.Slug == slug);    
}