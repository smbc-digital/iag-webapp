﻿namespace StockportWebapp.ViewModels;

public class ArticleViewModel
{
    public readonly ProcessedArticle Article;
    public readonly ProcessedSection DisplayedSection;
    public int DisplayedSectionIndex => IndexForDisplayedSection() + 1;
    public readonly bool HideLastUpdated;
    public readonly bool ShouldShowArticleSummary;
    public readonly bool ShouldShowCanonicalLink;
    public readonly string OgTitleMetaData;
    public SidebarViewModel Sidebar;
    public string MetaDescription => 
        string.IsNullOrEmpty(DisplayedSection?.MetaDescription) 
            ? Article.MetaDescription 
            : DisplayedSection.MetaDescription;

    public bool ArticleWithSection =>
        Article.Sections is not null && Article.Sections.Any() && DisplayedSection is not null;

    public bool ArticleHasImage =>
        !string.IsNullOrEmpty(Article.Image);

    public ArticleViewModel(ProcessedArticle article)
    {
        Article = article;
        DisplayedSection = FirstOrNull(article.Sections);
        HideLastUpdated = Article.HideLastUpdated;
        ShouldShowArticleSummary = true;
        ShouldShowCanonicalLink = false;
        OgTitleMetaData = Article.Title;
        Sidebar = new SidebarViewModel(this);
    }

    public ArticleViewModel(ProcessedArticle article, string sectionSlug)
    {
        Article = article;
        DisplayedSection = GetSectionOrThrowSectionNotFound(sectionSlug);
        ShouldShowArticleSummary = Article.Sections.First().Slug.Equals(DisplayedSection.Slug);
        OgTitleMetaData = string.Concat(Article.Title, !string.IsNullOrEmpty(DisplayedSection.Title)
            ? " - "
            : string.Empty, DisplayedSection.Title);
        HideLastUpdated = Article.HideLastUpdated;
        Sidebar = new SidebarViewModel(this);
    }

    private ProcessedSection GetSectionOrThrowSectionNotFound(string sectionSlug) => 
        Article.Sections?.FirstOrDefault(_ => _.Slug.Equals(sectionSlug))
            ?? throw new SectionDoesNotExistException($"Section with slug: {sectionSlug} does not exist");

    private static ProcessedSection FirstOrNull(IEnumerable<ProcessedSection> sections) =>
        sections is not null && sections.Any()
            ? sections.FirstOrDefault()
            : null;

    public ProcessedSection NextSection()
    {
        int nextSectionIndex = IndexForDisplayedSection() + 1;
        return nextSectionIndex < Article.Sections.Count()
            ? Article.Sections.ElementAt(nextSectionIndex)
            : null;
    }

    public ProcessedSection PreviousSection()
    {
        int previousSectionIndex = IndexForDisplayedSection() - 1;
        return previousSectionIndex >= 0
            ? Article.Sections.ElementAt(previousSectionIndex)
            : null;
    }

    public bool ShouldShowNextSectionButton() => 
        NextSection() is not null;

    public bool ShouldShowPreviousSectionButton() => 
        PreviousSection() is not null;

    public bool HasParentTopicWithSubItems() =>
        Article.ParentTopic is not null && Article.ParentTopic.SubItems.Any();

    public bool HasRelatedContentWithSubItems() =>
        Article.RelatedContent is not null && Article.RelatedContent.Any();

    public bool HasSecondarySubItems() => 
        Article.ParentTopic.SecondaryItems.Any();

    private int IndexForDisplayedSection()
    {
        const int firstIndex = 0;
        for (int i = 0; i < Article.Sections.Count(); i++)
        {
            ProcessedSection section = Article.Sections.ElementAt(i);
            if (section.Equals(DisplayedSection))
                return i;
        }
        return firstIndex;
    }

    public IEnumerable<SubItem> SidebarSubItems(out bool hasMoreButton)
    {
        Topic parentTopic = Article.ParentTopic;
        List<SubItem> sidebarSubItems = new();

        if (parentTopic is not null)
        {
            sidebarSubItems.AddRange(parentTopic.SubItems);
            sidebarSubItems.AddRange(parentTopic.SecondaryItems);
        }

        hasMoreButton = sidebarSubItems.Count > 6;
        
        return sidebarSubItems.Take(6);
    }

    public List<SubItem> GetItemsToDisplay(List<SubItem> relatedItems)
    {
        if (relatedItems is null || relatedItems.Count < 3)
            return relatedItems;

        int numberItemsToDisplay = relatedItems.Count / 3 * 3;

        return relatedItems.Take(numberItemsToDisplay).ToList();
    }

    public NavCardList PrimaryItems => new()
    {
        Items = Article.Events.Select(subItem => new NavCard(
            subItem.Title, 
            GenerateEventDetailUrl(subItem.Slug, subItem.EventDate),
            subItem.Teaser, 
            subItem.ThumbnailImageUrl, 
            subItem.ImageUrl,
            string.Empty,
            EColourScheme.Teal,
            subItem.EventDate,
            subItem.StartTime
        )).ToList()
    };

    private string GenerateEventDetailUrl(string slug, DateTime eventDate) =>
        $"/events/{slug}?date={eventDate:yyyy-MM-dd}";
}