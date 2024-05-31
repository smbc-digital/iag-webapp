namespace StockportWebapp.ViewModels;

[Obsolete("Groups is being replaced by directories/directory entries")]
[ExcludeFromCodeCoverage(Justification = "Obsolete")]
public class GroupDetailsViewModel
{
    public ProcessedGroup Group { get; set; }
    public string MyAccountUrl { get; set; }
    public bool ConfirmedUpToDate { get; set; }
    public bool ShouldShowAdminOptions { get; set; }
    public bool IsLoggedIn { get; set; }
    public int DaysTillStale { get; set; }
}