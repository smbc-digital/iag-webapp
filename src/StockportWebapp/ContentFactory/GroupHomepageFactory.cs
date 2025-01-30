namespace StockportWebapp.ContentFactory;

public class GroupHomepageFactory(ITagParserContainer tagParserContainer,
                                MarkdownWrapper markdownWrapper)
{
    private readonly ITagParserContainer _tagParserContainer = tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;

    public virtual ProcessedGroupHomepage Build(GroupHomepage groupHomepage)
    {
        string body = _tagParserContainer.ParseAll(groupHomepage.Body);
        string bodyHtml = _markdownWrapper.ConvertToHtml(body ?? string.Empty);

        string secondaryBody = _tagParserContainer.ParseAll(groupHomepage.SecondaryBody);
        string secondaryBodyHtml = _markdownWrapper.ConvertToHtml(secondaryBody ?? string.Empty);

        return new ProcessedGroupHomepage(
            groupHomepage.Title,
            groupHomepage.MetaDescription,
            groupHomepage.BackgroundImage,
            groupHomepage.FeaturedGroupsHeading,
            groupHomepage.FeaturedGroups,
            groupHomepage.FeaturedGroupsCategory,
            groupHomepage.FeaturedGroupsSubCategory,
            groupHomepage.Alerts,
            groupHomepage.BodyHeading,
            bodyHtml,
            groupHomepage.SecondaryBodyHeading,
            secondaryBodyHtml,
            groupHomepage.EventBanner);
    }
}