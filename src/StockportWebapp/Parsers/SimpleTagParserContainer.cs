using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StockportWebapp.Parsers
{
    public interface ISimpleTagParserContainer
    {
        string ParseAll(string content, string title = null);
    }

    public class SimpleTagParserContainer : ISimpleTagParserContainer
    {
        private readonly List<ISimpleTagParser> _tagParsers;
        private static Regex EmptyTagRegex => new Regex("{{([£$%^&*()@<>?~#|\\'\":\\w\\s]*)}}", RegexOptions.Compiled);

        public SimpleTagParserContainer(List<ISimpleTagParser> tagParsers)
        {
            _tagParsers = tagParsers;
        }

        public string ParseAll(string content, string title = null)
        {
            return RemoveEmptyTags(_tagParsers.Aggregate(content, (c,tagParser) => tagParser.Parse(c,title)));
        }

        private static string RemoveEmptyTags(string body)
        {
            return EmptyTagRegex.Replace(body, string.Empty);
        }
    }
}