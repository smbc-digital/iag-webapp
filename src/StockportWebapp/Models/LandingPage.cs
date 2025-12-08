namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class LandingPage
{
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public IEnumerable<Alert> Alerts { get; set; }
    public string Teaser { get; set; }
    public string MetaDescription { get; set; }
    public MediaAsset Image { get; set; }
    public string HeaderType { get; set; }
    public MediaAsset HeaderImage { get; set; }
    public EColourScheme HeaderColourScheme { get; set; } = EColourScheme.Teal;
    public IEnumerable<ContentBlock> PageSections { get; set; }

    public bool HeaderHighlightExists =>
        PageSections?.Any(section => section.ContentType.Equals("HeaderHighlight")) is true;

    public ContentBlock FirstSection =>
        PageSections?.FirstOrDefault();

    public ContentBlock SecondSection =>
        PageSections?.Skip(1).FirstOrDefault();

    public bool IsHeaderHighlightFirst =>
        FirstSection?.ContentType.Equals("HeaderHighlight") is true;

    public bool NeedsExtraPadding(ContentBlock section)
    {
        if (section is null || PageSections is null)
            return false;

        bool isTriviaOrStatementBanner =  section.ContentType.Equals("TriviaBanner") || section.ContentType.Equals("StatementBannerScreenWidth");

        if (!isTriviaOrStatementBanner || !HeaderHighlightExists)
            return false;

        return (IsHeaderHighlightFirst && section.Equals(SecondSection)) ||
               (!IsHeaderHighlightFirst && section.Equals(FirstSection));
    }

    public bool NeedsExtraMargin(ContentBlock section)
    {
        if (section is null || PageSections is null || !HeaderHighlightExists)
            return false;

        if ((section.Equals(FirstSection) && !IsHeaderHighlightFirst && HeaderHighlightExists) || (section.Equals(SecondSection) && IsHeaderHighlightFirst))
            return true;

        return false;
    }
}