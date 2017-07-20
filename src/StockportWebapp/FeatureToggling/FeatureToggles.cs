namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {
        public bool GroupManagement { get; set; } = false;
        public bool ManageYourGroupButton { get; set; } = false;
        public bool ContactUsIds { get; set; } = false;
        public bool ExportGroupToPdf { get; set; } = false;
        public bool NewHomepageLayout { get; set; } = false;
        public bool EditGroupWYSIWYG { get; set; } = false;
        public bool PrimaryItems { get; set; }
        public bool ExportGroupEventsToCalendar { get; set; } = false;
        public bool DisplayNewEventPageFeatures { get; set; } = false;
    }
}
