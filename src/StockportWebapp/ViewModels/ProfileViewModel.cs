using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class ProfileViewModel
{
    public readonly Profile Profile;
    public bool Redesigned;
    public SidebarViewModel Sidebar;

    public ProfileViewModel(Profile profile)
    {
        Profile = profile;

        if (profile is not null && profile.ParentTopic is not null)
            Sidebar = new SidebarViewModel(this);
    }

    public bool HasParentTopicWithSubItems() =>
        Profile.ParentTopic is not null && Profile.ParentTopic.SubItems.Any();

    public IEnumerable<SubItem> SidebarSubItems(out bool hasMoreButton)
    {
        Topic parentTopic = Profile.ParentTopic;
        List<SubItem> sidebarSubItems = new();

        if (parentTopic is not null)
        {
            sidebarSubItems.AddRange(parentTopic.SubItems);
            sidebarSubItems.AddRange(parentTopic.SecondaryItems);
        }

        hasMoreButton = sidebarSubItems.Count > 6;
        return sidebarSubItems.Take(6);
    }        
}