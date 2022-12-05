using StockportWebapp.Models;

namespace StockportWebapp.ProcessedModels
{
    public class ProcessedOrganisation : IProcessedContentType
    {
        public string Title { get; }
        public string Slug { get; }
        public string ImageUrl { get; }
        public string AboutUs { get; }
        public string Phone { get; }
        public string Email { get; }
        public List<Group> Groups { get; }
        public Volunteering Volunteering { get; }
        public Donations Donations { get; }
        public string CurrentUrl { get; private set; }

        public ProcessedOrganisation() { }

        public ProcessedOrganisation(string title, string slug, string imageUrl, string aboutUs, string phone,
            string email, List<Group> groups, Volunteering volunteering, Donations donations)
        {
            Title = title;
            Slug = slug;
            Phone = phone;
            Email = email;
            AboutUs = aboutUs;
            ImageUrl = imageUrl;
            Volunteering = volunteering;
            Groups = groups;
            Donations = donations;
        }

        internal void SetCurrentUrl(string host)
        {
            CurrentUrl = host;
        }
    }
}
