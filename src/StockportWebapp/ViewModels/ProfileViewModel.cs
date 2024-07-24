using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class ProfileViewModel
{
    public readonly Profile Profile;
    public bool Redesigned;
    public ProfileViewModel(Profile profile)
    {
        Profile = profile;
    }
}