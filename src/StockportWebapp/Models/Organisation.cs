namespace StockportWebapp.Models;

public class Organisation
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string ImageUrl { get; set; }
    public string AboutUs { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool Volunteering { get; set; }
    public string VolunteeringText { get; set; } = "";
    public List<Group> Groups { get; set; } = new List<Group>();
    public bool Donations { get; set; }

    public Organisation() { }

    public Organisation(string title, string slug, string imageUrl, string aboutUs, string phone,
        string email, bool volunteering, string volunteeringText, bool donations)
    {
        Title = title;
        Slug = slug;
        Phone = phone;
        Email = email;
        AboutUs = aboutUs;
        ImageUrl = imageUrl;
        Volunteering = volunteering;
        VolunteeringText = volunteeringText;
        Donations = donations;
    }
}