namespace StockportWebapp.Models.Groups;

public class GroupAdvisor
{
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public IEnumerable<string> Groups { get; set; }
    public bool HasGlobalAccess { get; set; }
}
