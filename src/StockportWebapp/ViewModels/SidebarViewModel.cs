namespace StockportWebapp.ViewModels;

public class SidebarViewModel
{
    public IEnumerable<SubItem> SidebarSubItems { get; set; }
    public string ParentTopicName { get; set; }
    public string ParentTopicSlug { get; set; }
    public string ParentTopicTeaser { get; set; }
    public string ParentTopicIcon { get; set; }
    public string ParentTopicImage { get; set; }
    public bool HasMoreButton { get; set; }

    public SidebarViewModel(ProfileViewModel profileViewModel)
    {
        SidebarSubItems = profileViewModel.SidebarSubItems(out bool hasMoreButton);
        ParentTopicName = profileViewModel.Profile.ParentTopic.Name;
        ParentTopicSlug = profileViewModel.Profile.ParentTopic.Slug;
        ParentTopicTeaser = profileViewModel.Profile.ParentTopic.Teaser;
        ParentTopicIcon = profileViewModel.Profile.ParentTopic.Icon;
        ParentTopicImage = profileViewModel.Profile.ParentTopic.Image;
        HasMoreButton = hasMoreButton;
    }

    public SidebarViewModel(ArticleViewModel articleViewModel)
    {
        SidebarSubItems = articleViewModel.SidebarSubItems(out bool hasMoreButton)
                           .Where(subItem => !subItem.NavigationLink.Equals(articleViewModel.Article.NavigationLink));
        ParentTopicName = articleViewModel.Article.ParentTopic?.Name;
        ParentTopicSlug = articleViewModel.Article.ParentTopic?.Slug;
        ParentTopicTeaser = articleViewModel.Article.ParentTopic?.Teaser;
        ParentTopicIcon = articleViewModel.Article.ParentTopic?.Icon;
        ParentTopicImage = articleViewModel.Article.ParentTopic?.Image;
        HasMoreButton = hasMoreButton;
    }

    public SidebarViewModel(PrivacyNoticeViewModel privacyNoticeViewModel)
    {
        SidebarSubItems = privacyNoticeViewModel.SidebarSubItems(out bool hasMoreButton)
                           .Where(subItem => !subItem.NavigationLink.Equals(privacyNoticeViewModel.PrivacyNotice.NavigationLink));
        ParentTopicName = privacyNoticeViewModel.PrivacyNotice.ParentTopic.Name;
        ParentTopicSlug = privacyNoticeViewModel.PrivacyNotice.ParentTopic.Slug;
        ParentTopicTeaser = privacyNoticeViewModel.PrivacyNotice.ParentTopic.Teaser;
        ParentTopicIcon = privacyNoticeViewModel.PrivacyNotice.ParentTopic.Icon;
        ParentTopicImage = privacyNoticeViewModel.PrivacyNotice.ParentTopic.Image;
        HasMoreButton = hasMoreButton;
    }
}