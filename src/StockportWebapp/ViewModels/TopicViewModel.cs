namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class TopicViewModel(ProcessedTopic topic)
{
    public ProcessedTopic Topic { get; } = topic;

    public bool TopicHasImage =>
        !string.IsNullOrEmpty(Topic.Image);

    public List<Event> EventsFromApi { get; set; }
}