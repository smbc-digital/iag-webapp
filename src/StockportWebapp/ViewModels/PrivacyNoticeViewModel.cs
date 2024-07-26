namespace StockportWebapp.ViewModels;
[ExcludeFromCodeCoverage]
public class PrivacyNoticeViewModel
{
    public readonly ProcessedPrivacyNotice PrivacyNotice;

    public PrivacyNoticeViewModel(ProcessedPrivacyNotice privacyNotice)
    {
        PrivacyNotice = privacyNotice;
    }

    public bool HasParentTopicWithSubItems()
    {
        return PrivacyNotice.ParentTopic != null && PrivacyNotice.ParentTopic.SubItems.Any();
    }

    public bool HasSecondarySubItems()
    {
        return PrivacyNotice.ParentTopic.SecondaryItems.Any();
    }

    public IEnumerable<SubItem> SidebarSubItems(out bool hasMoreButton)
    {
        Topic parentTopic = PrivacyNotice.ParentTopic;
        List<SubItem> sidebarSubItems = new();
        sidebarSubItems.AddRange(parentTopic.SubItems);
        sidebarSubItems.AddRange(parentTopic.SecondaryItems);
        hasMoreButton = sidebarSubItems.Count > 6;
        return sidebarSubItems.Take(6);
    }
}