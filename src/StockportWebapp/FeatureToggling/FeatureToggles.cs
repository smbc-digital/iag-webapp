namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {
        public bool DynamicContactUsForm { get; set; } = false;
        public bool LiveChat { get; set; } = false;
        public bool DynamicFooter { get; set; } = false;
        public bool LegacyUrlRedirects { get; set; } = false;
        public bool Search { get; set; } = false;
    }
}
