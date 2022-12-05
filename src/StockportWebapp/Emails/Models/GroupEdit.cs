namespace StockportWebapp.Emails.Models
{
    public class GroupEdit
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Categories { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public List<string> Suitabilities { get; set; }
        public List<string> AgeRanges { get; set; }
        public string Volunteering { get; set; }
        public string VolunteeringText { get; set; }
        public bool DonationsNeeded { get; set; }
        public string DonationsText { get; set; }
        public string DonationUrl { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
