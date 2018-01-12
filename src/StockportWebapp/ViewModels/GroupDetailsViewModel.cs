using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ViewModels
{
    public class GroupDetailsViewModel
    {
        public ProcessedGroup Group { get; set; }
        public bool ShouldShowAdditionalInformation { get; set; }
        public bool ShouldShowAdditionalInfoLink { get; set; }
        public bool UserHasAccessToAdditionalInformation { get; set; }
        public string MyAccountUrl { get; set; }
        public bool ConfirmedUpToDate { get; set; }
        public bool ShouldShowAdminOptions { get; set; }
        public bool IsLoggedIn { get; set; }
        public int DaysTillStale { get; set; }
    }
}