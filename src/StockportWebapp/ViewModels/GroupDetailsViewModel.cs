using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ViewModels
{
    public class GroupDetailsViewModel
    {
        public ProcessedGroup Group { get; set; }
        public bool ShouldShowAdditionalInformation { get; set; }
        public bool UserHasAccessToAdditionalInformation { get; set; }
        public string MyAccountUrl { get; set; }
    }
}