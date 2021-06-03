using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ViewModels
{
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

        public bool HasSecondaryOrTertiarySubItems()
        {
            return PrivacyNotice.ParentTopic.SecondaryItems.Any() || PrivacyNotice.ParentTopic.TertiaryItems.Any();
        }
        
        public IEnumerable<SubItem> SidebarSubItems(out bool hasMoreButton)
        {
            var parentTopic = PrivacyNotice.ParentTopic;
            var sidebarSubItems = new List<SubItem>();
            sidebarSubItems.AddRange(parentTopic.SubItems);
            sidebarSubItems.AddRange(parentTopic.SecondaryItems);
            hasMoreButton = sidebarSubItems.Count > 6;
            return sidebarSubItems.Take(6);
        }
    }
}
