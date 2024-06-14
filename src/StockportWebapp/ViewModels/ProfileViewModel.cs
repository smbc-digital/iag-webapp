using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class ProfileViewModel
{
    public readonly Profile Profile;
    public ProfileViewModel(Profile profile)
    {
        Profile = profile;
    }
}