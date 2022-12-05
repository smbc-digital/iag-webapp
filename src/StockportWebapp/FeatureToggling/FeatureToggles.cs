namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {
        public bool GroupArchiveEmails { get; set; } = false;
        public bool NameInHeader { get; set; } = false;
        public bool EditGroupUploadImage { get; set; } = false;
        public bool SiaSystem { get; set; } = false;
        public bool SemanticLayout { get; set; } = false;
        public bool ContactUsArea { get; set; } = false;
        public bool SemanticInlineAlert { get; set; } = false;
        public bool Newsroom { get; set; } = false;
        public bool LeafletMap { get; set; } = false;
        public bool CivicaPay { get; set; } = false;
        public bool ReciteMeTrial { get; set; } = false;
    }
}