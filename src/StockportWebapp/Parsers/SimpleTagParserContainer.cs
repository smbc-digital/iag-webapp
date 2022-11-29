using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public interface ISimpleTagParserContainer
    {
        string ParseAll(string content, string title = null, bool removeEmptytags = true);
    }

    public class SimpleTagParserContainer : ISimpleTagParserContainer
    {
        private readonly List<ISimpleTagParser> _tagParsers;
        private static Regex EmptyTagRegex => new Regex("{{([£$%^&*()@<>?~#|\\'\":\\w\\s]*)}}", RegexOptions.Compiled);

        public SimpleTagParserContainer(List<ISimpleTagParser> tagParsers)
        {
            _tagParsers = tagParsers;
        }

        public string ParseAll(string content, string title = null, bool removeEmptytags = true)
        {
            var parsedContent = _tagParsers.Aggregate(content, (c, tagParser) => tagParser.Parse(c, title));
            if (removeEmptytags)
            {
                parsedContent = RemoveEmptyTags(parsedContent);
            }

            return parsedContent;
        }

        private static string RemoveEmptyTags(string body)
        {
            return EmptyTagRegex.Replace(body, string.Empty);
        }
    }
}