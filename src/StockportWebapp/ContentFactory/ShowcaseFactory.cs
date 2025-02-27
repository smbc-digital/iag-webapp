﻿namespace StockportWebapp.ContentFactory;

public class ShowcaseFactory(ITagParserContainer tagParserContainer,
                            MarkdownWrapper markdownWrapper,
                            ITriviaFactory triviaFactory)
{
    private readonly ITagParserContainer _tagParserContainer = tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;
    private readonly ITriviaFactory _triviaFactory = triviaFactory;

    public virtual ProcessedShowcase Build(Showcase showcase)
    {
        string body = _tagParserContainer.ParseAll(showcase.Body);
        showcase.Body = _markdownWrapper.ConvertToHtml(body ?? string.Empty);

        Video video = showcase.Video;
        if (video is not null)
            video.VideoEmbedCode = _tagParserContainer.ParseAll(video.VideoEmbedCode);

        FieldOrder fields = showcase.FieldOrder;

        if (!fields.Items.Any())
        {
            fields.Items.Add("Primary Items");
            fields.Items.Add("Secondary Items");
            fields.Items.Add("Featured Items");
            fields.Items.Add("News");
            fields.Items.Add("Events");
            fields.Items.Add("Profile");
            fields.Items.Add("Profiles");
            fields.Items.Add("Social Media");
            fields.Items.Add("Body");
            fields.Items.Add("Video");
            fields.Items.Add("Trivia");
        }

        return new ProcessedShowcase(
            showcase.Title,
            showcase.Slug,
            showcase.Teaser,
            showcase.MetaDescription,
            showcase.Subheading,
            showcase.EventCategory,
            showcase.EventsCategoryOrTag,
            showcase.EventSubheading,
            showcase.NewsSubheading,
            showcase.NewsCategoryTag,
            showcase.NewsCategoryOrTag,
            showcase.BodySubheading,
            showcase.Body,
            showcase.NewsArticle,
            showcase.HeroImageUrl,
            showcase.SecondaryItems,
            showcase.Breadcrumbs,
            showcase.SocialMediaLinksSubheading,
            showcase.SocialMediaLinks,
            showcase.Events,
            showcase.EmailAlertsTopicId,
            showcase.EmailAlertsText,
            showcase.Alerts,
            showcase.PrimaryItems,
            showcase.FeaturedItemsSubheading,
            showcase.FeaturedItems,
            showcase.Profile,
            showcase.Profiles,
            showcase.CallToActionBanner,
            fields,
            showcase.Icon,
            showcase.TriviaSubheading,
            _triviaFactory.Build(showcase.TriviaSection),
            showcase.ProfileHeading,
            showcase.ProfileLink,
            showcase.EventsReadMoreText,
            video,
            showcase.SpotlightBanner
        );
    }
}