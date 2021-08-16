using System.Collections.Generic;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.Models
{
    public class Profile
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Subtitle { get; set; }
        public string Quote { get; set; }
        public List<InlineQuote> InlineQuotes { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }
        public IEnumerable<Crumb> Breadcrumbs { get; set; }
        public List<Alert> Alerts { get; set; }
        public string TriviaSubheading { get; set; }
        public List<ProcessedTrivia> TriviaSection { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public EventBanner EventsBanner { get; set; }

        public Profile()
        {

        }

        public Profile(string title,
            string slug,
            string subtitle,
            string quote,
            string image,
            string body,
            IEnumerable<Crumb> breadcrumbs,
            List<Alert> alerts,
            string triviaSubheading,
            List<ProcessedTrivia> triviaSection,
            List<InlineQuote> inlineQuotes,
            EventBanner eventsBanner)
        {
            Title = title;
            Slug = slug;
            Subtitle = subtitle;
            Quote = quote;
            Image = image;
            Body = body;
            Breadcrumbs = breadcrumbs;
            Alerts = alerts;
            TriviaSubheading = triviaSubheading;
            TriviaSection = triviaSection;
            InlineQuotes = inlineQuotes;
            EventsBanner = eventsBanner;
        }

    }
}
