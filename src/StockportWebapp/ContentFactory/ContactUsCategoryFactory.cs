using SharpKml.Base;

namespace StockportWebapp.ContentFactory;

public interface IContactUsCategoryFactory
{
    ProcessedContactUsCategory Build(ContactUsCategory contactUsCategory);
}

public class ContactUsCategoryFactory : IContactUsCategoryFactory
{
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly ITagParserContainer _tagParserContainer;

    public ContactUsCategoryFactory(ITagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper)
    {
        _tagParserContainer = tagParserContainer;
        _markdownWrapper = markdownWrapper;
    }

    public ProcessedContactUsCategory Build(ContactUsCategory contactUsCategory)
    {
        var parsedBodyTextLeft = _markdownWrapper.ConvertToHtml(contactUsCategory.BodyTextLeft);
        parsedBodyTextLeft = _tagParserContainer.ParseAll(parsedBodyTextLeft);

        var parsedBodyTextRight = _markdownWrapper.ConvertToHtml(contactUsCategory.BodyTextRight);
        parsedBodyTextRight = _tagParserContainer.ParseAll(parsedBodyTextRight);

        return new ProcessedContactUsCategory(
            contactUsCategory.Title,
            parsedBodyTextLeft,
            parsedBodyTextRight,
            contactUsCategory.Icon
        );
    }
}
