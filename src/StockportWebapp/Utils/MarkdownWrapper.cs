using System.Net.NetworkInformation;

namespace StockportWebapp.Utils;

public class MarkdownWrapper
{
    public virtual string ConvertToHtml(string markdown) => Convert(markdown);

    public static string ToHtml(string markdown) => Convert(markdown);

    private static string Convert(string markdown)
    {
        string html = Markdown.ToHtml(markdown ?? string.Empty, new MarkdownPipelineBuilder().UsePipeTables().UseSoftlineBreakAsHardlineBreak().UseAdvancedExtensions().Build());
        string wrappedTableHtml = WrapTables(html);
        
        return ReplaceFigCaptionFloat(wrappedTableHtml);
    }

    private static string WrapTables(string html) => html.Replace("<table>", "<div class=\"table\">\n<table>").Replace("</table>", "</table>\n</div>");

    private static string ReplaceFigCaptionFloat(string html) => html
        .Replace("<figure>\n<figcaption>#right</figcaption>", "<figure class='image-right'>")
        .Replace("<figure>\n<figcaption>#left</figcaption>", "<figure class='image-left'>");
}