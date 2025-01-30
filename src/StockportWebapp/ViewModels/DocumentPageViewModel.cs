namespace StockportWebapp.ViewModels;

[ExcludeFromCodeCoverage]
public class DocumentPageViewModel(DocumentPage documentPage)
{
    public readonly DocumentPage DocumentPage = documentPage;
    public readonly string OgTitleMetaData = documentPage.Title;
    public string MetaDescription => DocumentPage.MetaDescription;

    public bool DisplayLastUpdated =>
        !DocumentPage.LastUpdated.Equals(DateTime.MaxValue);
}