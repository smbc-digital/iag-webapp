namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class DocumentPageViewModel
{
    public readonly DocumentPage DocumentPage;
    public readonly string OgTitleMetaData;
    public string MetaDescription => DocumentPage.MetaDescription;

    public DocumentPageViewModel(DocumentPage documentPage)
    {
        DocumentPage = documentPage;
        OgTitleMetaData = documentPage.Title;
    }

    public bool DisplayLastUpdated => !DocumentPage.LastUpdated.Equals(DateTime.MaxValue);
}