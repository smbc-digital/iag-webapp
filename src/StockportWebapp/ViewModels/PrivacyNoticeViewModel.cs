namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class PrivacyNoticeViewModel
{
    public readonly ProcessedPrivacyNotice PrivacyNotice;
    public SidebarViewModel Sidebar;

    public PrivacyNoticeViewModel(ProcessedPrivacyNotice privacyNotice)
    {
        PrivacyNotice = privacyNotice;
        Sidebar = new SidebarViewModel(this);
    }

    public bool HasParentTopicWithSubItems() =>
        PrivacyNotice.ParentTopic is not null && PrivacyNotice.ParentTopic.SubItems.Any();

    public IEnumerable<SubItem> SidebarSubItems(out bool hasMoreButton)
    {
        Topic parentTopic = PrivacyNotice.ParentTopic;
        List<SubItem> sidebarSubItems = new(parentTopic.SubItems.Concat(parentTopic.SecondaryItems));
        
        hasMoreButton = sidebarSubItems.Count > 6;
        
        return sidebarSubItems.Take(6);
    }
}