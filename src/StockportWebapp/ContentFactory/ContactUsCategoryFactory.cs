using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public interface IContactUsCategoryFactory
    {
        ProcessedContactUsCategory Build(ContactUsCategory contactUsCategory);
    }

    public class ContactUsCategoryFactory : IContactUsCategoryFactory
    {
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly ISimpleTagParserContainer _tagParserContainer;
        //private readonly IDynamicTagParser<Profile> _profileTagParser;
        private readonly IDynamicTagParser<Document> _documentTagParser;
        //private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
        //private readonly IDynamicTagParser<S3BucketSearch> _searchTagParser;
        //private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser;
        private readonly IRepository _repository;

        public ContactUsCategoryFactory(ISimpleTagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper,
            IDynamicTagParser<Document> documentTagParser, IRepository repository)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
            _documentTagParser = documentTagParser;
            _repository = repository;
        }

        public ProcessedContactUsCategory Build(ContactUsCategory contactUsCategory)
        {
            var parsedBodyTextLeft = _markdownWrapper.ConvertToHtml(contactUsCategory.BodyTextLeft);
            //parsedBodyTextLeft = _documentTagParser.Parse(parsedBodyTextLeft);
            parsedBodyTextLeft = _tagParserContainer.ParseAll(parsedBodyTextLeft);

            var parsedBodyTextRight = _markdownWrapper.ConvertToHtml(contactUsCategory.BodyTextRight);
            //parsedBodyTextRight = _documentTagParser.Parse(parsedBodyTextRight, contactUsCategory.Documents);
            parsedBodyTextRight = _tagParserContainer.ParseAll(parsedBodyTextRight);

            return new ProcessedContactUsCategory(
                contactUsCategory.Title,
                parsedBodyTextLeft,
                parsedBodyTextRight,
                contactUsCategory.Icon
            );
        }
    }
}
