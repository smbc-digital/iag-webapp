namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {
        public bool EventCalendar { get; set; } = false;
        public bool SecurityHeaders { get; set; } = false;
        public bool EventSubmission { get; set; } = false;
        public bool LatestEventsHomepage { get; set; } = false;
        public bool MyAccountHeaderButton { get; set; } = false;
    }
}
