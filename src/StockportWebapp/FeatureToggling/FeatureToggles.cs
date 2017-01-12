namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {
        public bool DynamicContactUsForm { get; set; } = false;
        public bool Search { get; set; } = false;
        public bool EventCalendar { get; set; } = false;
        public bool SecurityHeaders { get; set; } = false;
    }
}
