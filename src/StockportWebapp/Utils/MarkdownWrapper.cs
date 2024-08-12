namespace StockportWebapp.Utils;

public class MarkdownWrapper
{
    public virtual string ConvertToHtml(string markdown) => Convert(markdown);

    public static string ToHtml(string markdown) => Convert(markdown);

    private static string Convert(string markdown)
    {
        string html = Markdown.ToHtml(markdown ?? string.Empty, new MarkdownPipelineBuilder().UsePipeTables().UseSoftlineBreakAsHardlineBreak().UseAdvancedExtensions().Build());
        return WrapTables(html);
    }

    private static string WrapTables(string html) => html.Replace("<table>", "<div class=\"table\">\n<table>").Replace("</table>", "</table>\n</div>");
}