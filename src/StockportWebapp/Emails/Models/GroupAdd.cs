using System.Collections.Generic;

namespace StockportWebapp.Emails.Models
{
    public class GroupAdd
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public bool VolunteeringNeeded{ get; set; }
        public string VolunteeringText { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public List<string> Suitabilities { get; set; }
        public List<string> AgeRanges { get; set; }
        public bool DonationsNeeded { get; set; }
        public string  DonationsText { get; set; }
        public string  DonationUrl { get; set; }
    }
}
