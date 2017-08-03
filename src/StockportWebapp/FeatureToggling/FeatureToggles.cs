namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {      
        public bool NewHomepageLayout { get; set; } = false;
        public bool EditGroupWYSIWYG { get; set; } = false;
        public bool SmartAnswers { get; set; } = false;
        public bool PrimaryItems { get; set; }
        public bool ExportGroupEventsToCalendar { get; set; } = false;
        public bool DisplayNewEventPageFeatures { get; set; } = false;
    }
}
