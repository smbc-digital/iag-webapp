using StockportWebapp.Utils;
using System;

namespace StockportWebapp.Models
{
    public class InsetText
    {
        public string Title { get; }
        public string SubHeading { get; }
        public string Body { get; }
        public string Colour { get; }
        public string Slug { get; }

        public InsetText(string title, string subHeading, string body, string colour, string slug)
        {
            Title = title;
            SubHeading = subHeading;
            Body = MarkdownWrapper.ToHtml(body);
            Colour = colour;
            Slug = slug;
        }
    }

    public class NullInsetText : InsetText
    {
        public NullInsetText() : base(string.Empty, string.Empty, string.Empty, string.Empty, String.Empty) { }
    }

    public static class Colour
    {
        public const string Grey = "Grey";
        public const string Amber = "Amber";
    }
}
