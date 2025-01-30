using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.TagParsers;

public class ProfileTagParser(IViewRender viewRenderer) : IDynamicTagParser<Profile>
{
    private readonly IViewRender _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{PROFILE:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string content, IEnumerable<Profile> profiles, bool redesigned)
    {
        MatchCollection matches = TagRegex.Matches(content);
        
        foreach (Match match in matches)
        {
            string profileSlug = match.Groups[1].Value;
            Profile profile = GetProfileMatchingSlug(profiles, profileSlug);
            ProfileViewModel viewModel = new(profile)
            {
                Redesigned = redesigned
            };

            if (profile is not null)
                content = TagRegex.Replace(content, _viewRenderer.Render("Profile", viewModel), 1);
        }
        
        return RemoveEmptyTags(content);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);

    private static Profile GetProfileMatchingSlug(IEnumerable<Profile> profiles, string slug) =>
        profiles?.FirstOrDefault(_ => _.Slug.Equals(slug));
}