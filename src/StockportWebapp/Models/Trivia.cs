namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Trivia(string title, string icon, string body, string link, string statistic = null, string statisticSubheading = null)
{
    public string Title { get; set; } = title;
    public string Icon { get; set; } = icon;
    public string Body { get; set; } = body;
    public string Link { get; set; } = link;
    public string Statistic { get; set; } = statistic;
    public string StatisticSubheading { get; set; } = statisticSubheading;
}