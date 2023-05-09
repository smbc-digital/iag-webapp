namespace StockportWebapp.ContentFactory;

public class GroupHomepageFactory
{
    private readonly ISimpleTagParserContainer _tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper;

    public GroupHomepageFactory(ISimpleTagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper)
    {
        _tagParserContainer = tagParserContainer;
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedGroupHomepage Build(GroupHomepage groupHomepage)
    {
        var body = _tagParserContainer.ParseAll(groupHomepage.Body);
        var bodyHtml = _markdownWrapper.ConvertToHtml(body ?? string.Empty);

        var secondaryBody = _tagParserContainer.ParseAll(groupHomepage.SecondaryBody);
        var secondaryBodyHtml = _markdownWrapper.ConvertToHtml(secondaryBody ?? string.Empty);

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
        groupHomepage.EventBanner
        );
    }
}
