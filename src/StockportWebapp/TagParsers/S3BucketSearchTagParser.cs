namespace StockportWebapp.TagParsers;

public class S3BucketSearchTagParser : IDynamicTagParser<S3BucketSearch>
{
    private readonly IViewRender _viewRenderer;

    public S3BucketSearchTagParser(IViewRender viewRenderer) => _viewRenderer = viewRenderer;

    protected Regex TagRegex => new Regex("{{(Search:((.*?)\\/)*)}}", RegexOptions.Compiled);

    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string content, IEnumerable<S3BucketSearch> searches)
    {
        var matches = TagRegex.Matches(content);

        foreach (Match match in matches)
        {
            var search = searches?.FirstOrDefault();
            if (search != null)
            {
                search.SearchFolder = match.Groups[0].ToString().Replace("{{Search:", "").Replace("}}", "");
                var searchHtml = _viewRenderer.Render("S3Bucket", search);
                content = TagRegex.Replace(content, searchHtml, 1);
            }
        }
        return RemoveEmptyTags(content);
    }

    private string RemoveEmptyTags(string content) =>   
        TagRegex.Replace(content, string.Empty);
}
