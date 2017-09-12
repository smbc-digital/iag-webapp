namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {      
        public bool NewHomepageLayout { get; set; } = false;       
        public bool SmartAnswers { get; set; } = false;
        public bool PrimaryItems { get; set; }        
        public bool DisplayNewEventPageFeatures { get; set; } = false;
        public bool GroupFavourites { get; set; } = false;
        public bool GroupFilterBar { get; set; } = false;
        public bool GroupHomepage { get; set; } = false;
    }
}
