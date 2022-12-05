using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public class TagReplacer
    {
        private readonly Regex _tagRegex;
        private readonly Func<string, string> _generateHtml;
        private const int TagDataIndex = 1;

        public TagReplacer(Func<string, string> generateHtml, Regex tagRegex)
        {
            _generateHtml = generateHtml;
            _tagRegex = tagRegex;
        }

        public string ReplaceAllTags(string body)
        {
            var newBody = body;
            var matches = _tagRegex.Matches(body);

            return matches.Cast<Match>()
                .Aggregate(newBody, (current, match) => ReplaceMatchWithGeneratedHtml(match, current, _generateHtml));
        }

        private static string ReplaceMatchWithGeneratedHtml(Match match, string newBody,
            Func<string, string> generateHtml)
        {
            var tagData = match.Groups[TagDataIndex].Value;
            var tagToHtml = generateHtml(tagData.Trim());
            return newBody.Replace(match.Groups[0].Value, tagToHtml);
        }
    }
}