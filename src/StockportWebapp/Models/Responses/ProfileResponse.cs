namespace StockportWebapp.Models.Responses;

public class ProfileResponse
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Teaser { get; set; }
    public string Subtitle { get; set; }
    public string Quote { get; set; }
    public List<InlineQuote> InlineQuotes { get; set; }
    public string Image { get; set; }
    public string Body { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public List<Alert> Alerts { get; set; }
    public string TriviaSubheading { get; set; }
    public List<Trivia> TriviaSection { get; set; }
    public string Subject { get; set; }
    public string Author { get; set; }
    public EventBanner EventsBanner { get; set; }
}