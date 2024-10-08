namespace StockportWebapp.TagParsers;

public class DocumentTagParser : IDynamicTagParser<Document>
{
    private readonly IViewRender _viewRenderer;

    public DocumentTagParser(IViewRender viewRenderer) => _viewRenderer = viewRenderer;

    protected Regex TagRegex => new Regex("{{PDF:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);
    public bool HasMatches(string content) => TagRegex.IsMatch(content);


    public string Parse(string content, IEnumerable<Document> documents, bool redesigned = false)
    {
        var matches = TagRegex.Matches(content);

        foreach (Match tagMatch in matches)
        {
            var tagDataIndex = 1;
            var fileName = tagMatch.Groups[tagDataIndex].Value;
            var document = GetDocumentMatchingFilename(documents, fileName);
            if (document != null)
                content = ReplaceTagWithHtml(content, document);
            
        }
        return RemoveEmptyTags(content);
    }

    private string ReplaceTagWithHtml(string content, Document document)
    {
        var documentHtml = _viewRenderer.Render("Document", document);
        return TagRegex.Replace(content, documentHtml, 1);
    }

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);
    

    private static Document GetDocumentMatchingFilename(IEnumerable<Document> documents, string fileName) =>
        documents?.FirstOrDefault(s => s.FileName == fileName);
}