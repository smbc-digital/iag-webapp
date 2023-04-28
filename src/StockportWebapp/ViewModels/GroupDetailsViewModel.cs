using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ViewModels
{
    public class GroupDetailsViewModel
    {
        public ProcessedGroup Group { get; set; }
        public string MyAccountUrl { get; set; }
        public bool ConfirmedUpToDate { get; set; }
        public bool ShouldShowAdminOptions { get; set; }
        public bool IsLoggedIn { get; set; }
        public int DaysTillStale { get; set; }
    }
}