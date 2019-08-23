using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class ContactUsAreaFactory
    {
        private readonly ISimpleTagParserContainer _tagParserContainer;
        private readonly MarkdownWrapper _markdownWrapper;
        private readonly IContactUsCategoryFactory _contactUsCategoryFactory;
        

        public ContactUsAreaFactory(ISimpleTagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper, IContactUsCategoryFactory contactUsCategoryFactory)
        {
            _tagParserContainer = tagParserContainer;
            _markdownWrapper = markdownWrapper;
            _contactUsCategoryFactory = contactUsCategoryFactory;
        }

        public virtual ProcessedContactUsArea Build(ContactUsArea contactUsArea)
        {
            var processedContactUsCategories = new List<ProcessedContactUsCategory>();
            foreach (var contactUsCategory in contactUsArea.ContactUsCategories)
            {
                processedContactUsCategories.Add(_contactUsCategoryFactory.Build(contactUsCategory));
            }

            return new ProcessedContactUsArea(
                contactUsArea.Title,
                contactUsArea.Slug,
                contactUsArea.CategoriesTitle,
                contactUsArea.Breadcrumbs,
                contactUsArea.PrimaryItems,
                contactUsArea.Alerts,
                contactUsArea.InsetTexts,
                processedContactUsCategories,
                contactUsArea.MetaDescription
            );
        }
    }
}
