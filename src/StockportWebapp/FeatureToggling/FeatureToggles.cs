namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {      
        public bool TopicPageImages { get; set; } = false;
        public bool SinglePayment { get; set; } = false;
        public bool GroupStartPage { get; set; } = false;
        public bool GroupResultsPage { get; set; } = false;
        public bool GroupBreadCrumb { get; set; } = false;
        public bool GroupPrimaryFilter { get; set; } = false;
    }
}
