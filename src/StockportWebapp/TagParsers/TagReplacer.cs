namespace StockportWebapp.TagParsers;

public class TagReplacer(Func<string, string> generateHtml, Regex tagRegex)
{
    private readonly Regex _tagRegex = tagRegex;
    private readonly Func<string, string> _generateHtml = generateHtml;
    private const int TagDataIndex = 1;

    public string ReplaceAllTags(string body)
    {
        string newBody = body;
        MatchCollection matches = _tagRegex.Matches(body);

        return matches
            .Cast<Match>()
            .Aggregate(newBody, (current, match) => ReplaceMatchWithGeneratedHtml(match, current, _generateHtml));
    }

    private static string ReplaceMatchWithGeneratedHtml(Match match,
                                                        string newBody,
                                                        Func<string, string> generateHtml) =>
        newBody.Replace(match.Groups[0].Value, generateHtml(match.Groups[TagDataIndex].Value.Trim()));
}