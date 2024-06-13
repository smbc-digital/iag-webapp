using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.ViewModels;

public class ProfileViewModel
{
    public readonly Profile Profile;
    public readonly ProcessedSection DisplayedSection;
    public List<ProcessedTrivia> TriviaSection { get; set; }

    public ProfileViewModel(Profile profile)
    {
        Profile = profile;
    }

}