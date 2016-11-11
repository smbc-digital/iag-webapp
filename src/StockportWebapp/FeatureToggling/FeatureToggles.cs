namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {
        public bool DynamicContactUsForm { get; set; } = false;
        public bool TitleRedesign { get; set; } = false;
        public bool BusinessIdFromRequest { get; set; } = false;
        public bool LiveChat { get; set; } = false;
        public bool NewsCategory { get; set; } = false;
    }
}
