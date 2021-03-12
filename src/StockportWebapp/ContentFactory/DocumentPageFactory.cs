using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class DocumentPageFactory
    {
        private readonly MarkdownWrapper _markdownWrapper;

        public DocumentPageFactory(
            MarkdownWrapper markdownWrapper)
        {
            _markdownWrapper = markdownWrapper;
        }

        public virtual ProcessedDocumentPage Build(DocumentPage documentPage)
        {
            var aboutThisDocument = _markdownWrapper.ConvertToHtml(documentPage.AboutThisDocument);
            var requestAnAccessibleFormatContactInformation = _markdownWrapper.ConvertToHtml(documentPage.RequestAnAccessibleFormatContactInformation);
            var furtherInformation = _markdownWrapper.ConvertToHtml(documentPage.FurtherInformation);

            return new ProcessedDocumentPage(
                documentPage.Title,
                documentPage.Slug,
                documentPage.Teaser,
                documentPage.MetaDescription,
                aboutThisDocument,
                documentPage.Documents,
                requestAnAccessibleFormatContactInformation,
                furtherInformation,
                documentPage.RelatedDocuments,
                documentPage.DatePublished,
                documentPage.LastUpdated,
                documentPage.Breadcrumbs,
                documentPage.UpdatedAt);
        }
    }
}
