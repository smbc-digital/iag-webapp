namespace StockportWebapp.Utils;

public class MarkdownWrapper
{
    public virtual string ConvertToHtml(string markdown)
    {
        return Convert(markdown);
    }

    public static string ToHtml(string markdown)
    {
        return Convert(markdown);
    }

    private static string Convert(string markdown)
    {
        var html = Markdown.ToHtml(markdown ?? string.Empty, new MarkdownPipelineBuilder().UsePipeTables().UseSoftlineBreakAsHardlineBreak().Build());
        return WrapTables(html);
    }

    private static string WrapTables(string html)
    {
        return html.Replace("<table>", "<div class=\"table\">\n<table>").Replace("</table>", "</table>\n</div>");
    }
}
