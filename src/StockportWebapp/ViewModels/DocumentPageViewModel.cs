using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ViewModels
{
    public class DocumentPageViewModel
    {
        public readonly ProcessedDocumentPage DocumentPage;
        public readonly string OgTitleMetaData;
        public string MetaDescription => DocumentPage.MetaDescription;

        public DocumentPageViewModel(ProcessedDocumentPage documentPage)
        {
            DocumentPage = documentPage;
            OgTitleMetaData = documentPage.Title;
        }
    }
}
