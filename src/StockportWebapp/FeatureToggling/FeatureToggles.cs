using System.Collections.Generic;

namespace StockportWebapp.FeatureToggling
{
    public class FeatureToggles
    {                 
        public bool GroupArchiveEmails { get; set; } = false;
        public bool NameInHeader { get; set; } = false;
        public bool EditGroupUploadImage { get; set; } = false;
        public bool GroupDetailsPage { get; set; } = false;
        public bool SiaSystem { get; set; } = false;
        public bool SemanticLayout { get; set; } = false;
        public List<string> SemanticSmartAnswer { get; set; } = new List<string>();
    }
}
