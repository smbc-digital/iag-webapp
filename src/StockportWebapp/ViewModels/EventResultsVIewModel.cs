namespace StockportWebapp.ViewModels;

public class EventResultsViewModel
{
    public string Title { get; set; }
    public List<Event> Events { get; set; } = new List<Event>();
    public QueryUrl CurrentUrl { get; private set; }
    public Pagination Pagination { get; set; }
    public IFilteredUrl FilteredUrl { get; private set; }

    public EventResultsViewModel() { }

    public void AddFilteredUrl(IFilteredUrl filteredUrl)
    {
        FilteredUrl = filteredUrl;
    }

    public void AddQueryUrl(QueryUrl queryUrl)
    {
        CurrentUrl = queryUrl;
    }
}
