using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ViewModels
{
    public class ArticleViewModel
    {
        public readonly ProcessedArticle Article;
        public readonly ProcessedSection DisplayedSection;
        public int DisplayedSectionIndex => IndexForDisplayedSection() + 1;
        public readonly bool HideLastUpdated;
        public readonly bool ShouldShowArticleSummary;
        public readonly bool ShouldShowCanonicalLink;
        public readonly string OgTitleMetaData;
        public string MetaDescription => string.IsNullOrEmpty(DisplayedSection?.MetaDescription) ? Article.MetaDescription : DisplayedSection.MetaDescription;

        public ArticleViewModel(ProcessedArticle article)
        {
            Article = article;
            DisplayedSection = FirstOrNull(article.Sections);
            HideLastUpdated = Article.HideLastUpdated;
            ShouldShowArticleSummary = true;
            ShouldShowCanonicalLink = false;
            OgTitleMetaData = Article.Title;
        }

        public ArticleViewModel(ProcessedArticle article, string sectionSlug)
        {
            Article = article;
            DisplayedSection = GetSectionOrThrowSectionNotFound(sectionSlug);
            ShouldShowArticleSummary = (Article.Sections.First().Slug == DisplayedSection.Slug);
            OgTitleMetaData = string.Concat(Article.Title, !string.IsNullOrEmpty(DisplayedSection.Title) ? " - " : "", DisplayedSection.Title);
            HideLastUpdated = Article.HideLastUpdated;
        }

        private ProcessedSection GetSectionOrThrowSectionNotFound(string sectionSlug)
        {
            var section = Article.Sections.FirstOrDefault(x=> x.Slug == sectionSlug);

            if (section == null)
            {
                throw new SectionDoesNotExistException($"Section with slug: {sectionSlug} does not exist");
            }
            return section;
        }

        private static ProcessedSection FirstOrNull(IEnumerable<ProcessedSection> sections)
        {
            if (sections != null && sections.Any())
                return sections.FirstOrDefault();
            return null;
        }     

        public ProcessedSection NextSection()
        {
            var displaySectionIndex = IndexForDisplayedSection();
            var nextSectionIndex = displaySectionIndex + 1;
            return nextSectionIndex < Article.Sections.Count() ? Article.Sections.ElementAt(nextSectionIndex) : null;
        }

        public ProcessedSection PreviousSection()
        {
            var displaySectionIndex = IndexForDisplayedSection();
            var previousSectionIndex = displaySectionIndex - 1;
            return previousSectionIndex >= 0 ? Article.Sections.ElementAt(previousSectionIndex) : null;
        }

        public bool ShouldShowNextSectionButton()
        {
            return NextSection() != null;
        }

        public bool ShouldShowPreviousSectionButton()
        {
            return PreviousSection() != null;
        }

        public bool HasParentTopicWithSubItems()
        {
            return Article.ParentTopic != null && Article.ParentTopic.SubItems.Any();
        }

        public bool HasSecondaryOrTertiarySubItems()
        {
            return Article.ParentTopic.SecondaryItems.Any() || Article.ParentTopic.TertiaryItems.Any();
        }

        private int IndexForDisplayedSection()
        {
            const int firstIndex = 0;
            for (var i = 0; i < Article.Sections.Count(); i++)
            {
                var section = Article.Sections.ElementAt(i);
                if (section == DisplayedSection)
                {
                    return i;
                }
            }
            return firstIndex;
        }

        public IEnumerable<SubItem> SidebarSubItems(out bool hasMoreButton)
        {
            var parentTopic = Article.ParentTopic;
            var sidebarSubItems = new List<SubItem>();
            sidebarSubItems.AddRange(parentTopic.SubItems);
            sidebarSubItems.AddRange(parentTopic.SecondaryItems);
            hasMoreButton = sidebarSubItems.Count > 6;
            return sidebarSubItems.Take(6);
        }
    }
}
