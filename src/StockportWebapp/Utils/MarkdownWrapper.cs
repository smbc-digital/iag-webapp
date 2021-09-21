using Markdig;

namespace StockportWebapp.Utils
{
    //Todo: I didn't want to create this class but we use the markdown converter
    // statically everywhere and fixing that will take time. (Need to get rid of static method during refactor)
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
}
