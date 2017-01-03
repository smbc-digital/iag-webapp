namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {
        public bool DynamicContactUsForm { get; set; } = false;
        public bool LiveChat { get; set; } = false;
        public bool Search { get; set; } = false;
        public bool EventCalendar { get; set; } = false;
        public virtual bool SecurityHeaders { get; set; } = false;
    }
}
