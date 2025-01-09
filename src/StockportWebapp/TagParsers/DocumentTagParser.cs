namespace StockportWebapp.TagParsers;

public class DocumentTagParser(IViewRender viewRenderer) : IDynamicTagParser<Document>
{
    private readonly IViewRender _viewRenderer = viewRenderer;

    protected Regex TagRegex => new("{{PDF:(\\s*[/a-zA-Z0-9][^}]+)}}", RegexOptions.Compiled);
    public bool HasMatches(string content) => TagRegex.IsMatch(content);

    public string Parse(string content, IEnumerable<Document> documents, bool redesigned = false)
    {
        MatchCollection matches = TagRegex.Matches(content);

        foreach (Match tagMatch in matches)
        {
            Document document = GetDocumentMatchingFilename(documents, tagMatch.Groups[1].Value);

            if (document is not null)
                content = ReplaceTagWithHtml(content, document);
        }

        return RemoveEmptyTags(content);
    }

    private string ReplaceTagWithHtml(string content, Document document) =>
        TagRegex.Replace(content, _viewRenderer.Render("Document", document), 1);

    private string RemoveEmptyTags(string content) =>
        TagRegex.Replace(content, string.Empty);
    
    private static Document GetDocumentMatchingFilename(IEnumerable<Document> documents, string fileName) =>
        documents?.FirstOrDefault(s => s.FileName.Equals(fileName));
}