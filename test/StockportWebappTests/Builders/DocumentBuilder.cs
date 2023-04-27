namespace StockportWebappTests_Unit.Builders;

public class DocumentBuilder
{
    private const string AssetId = "asset id";
    private const string Title = "title";
    private const int Size = 22;
    private const string Url = "url";
    private readonly DateTime _lastUpdated = DateTime.MinValue;
    private const string FileName = "fileName";
    private const string MediaType = "application/pdf";

    public Document Build()
    {
        return new Document(Title, Size, _lastUpdated, Url, FileName, AssetId, MediaType);
    }
}
