using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.TagParsers;

public class ProfileTagParser : IDynamicTagParser<Profile>
{
    private readonly IViewRender _viewRenderer;

    public ProfileTagParser(IViewRender viewRenderer) => _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{PROFILE:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string content, IEnumerable<Profile> profiles)
    {
        MatchCollection matches = TagRegex.Matches(content);

        foreach (Match match in matches)
        {
            int tagDataIndex = 1;
            string profileSlug = match.Groups[tagDataIndex].Value;
            Profile profile = GetProfileMatchingSlug(profiles, profileSlug);
            ProfileViewModel viewModel = new(profile);
            if (profile is not null)
            {
                string profileHtml = string.IsNullOrEmpty(profile.Body)
                    ? _viewRenderer.Render("ProfileWithoutBody", viewModel)
                    : _viewRenderer.Render("Profile", viewModel);

                content = TagRegex.Replace(content, profileHtml, 1);
            }
        }
        
        return RemoveEmptyTags(content);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);

    private static Profile GetProfileMatchingSlug(IEnumerable<Profile> profiles, string slug) =>
        profiles?.FirstOrDefault(s => s.Slug.Equals(slug));
}