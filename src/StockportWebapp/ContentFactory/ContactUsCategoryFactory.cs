namespace StockportWebapp.ContentFactory;

public interface IContactUsCategoryFactory
{
    ProcessedContactUsCategory Build(ContactUsCategory contactUsCategory);
}

public class ContactUsCategoryFactory(ITagParserContainer tagParserContainer,
                                    MarkdownWrapper markdownWrapper) : IContactUsCategoryFactory
{
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;
    private readonly ITagParserContainer _tagParserContainer = tagParserContainer;

    public ProcessedContactUsCategory Build(ContactUsCategory contactUsCategory)
    {
        string parsedBodyTextLeft = _markdownWrapper.ConvertToHtml(contactUsCategory.BodyTextLeft);
        parsedBodyTextLeft = _tagParserContainer.ParseAll(parsedBodyTextLeft);

        string parsedBodyTextRight = _markdownWrapper.ConvertToHtml(contactUsCategory.BodyTextRight);
        parsedBodyTextRight = _tagParserContainer.ParseAll(parsedBodyTextRight);

        return new ProcessedContactUsCategory(
            contactUsCategory.Title,
            parsedBodyTextLeft,
            parsedBodyTextRight,
            contactUsCategory.Icon
        );
    }
}