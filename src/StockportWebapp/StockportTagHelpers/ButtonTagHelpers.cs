using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StockportTagHelpers
{
    [HtmlTargetElement("stock-button", ParentTag = null)]
    public class DefaultButtonTagHelper : TagHelper
    {
        private const string CLASSES = "button-default";
        private const string TAG_NAME = "button";
        private const string ANCHOR_TAG_NAME = "a";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.AllAttributes.ContainsName("as-link"))
            {
                output.TagName = ANCHOR_TAG_NAME;
                output.Attributes.RemoveAll("as-link");
            }
            else
            {
                output.TagName = TAG_NAME;
            }
            await HtmlAttributes.UpdateClassesToInclude(output, CLASSES);
        }

    }

    [HtmlTargetElement("stock-button-loading", ParentTag = null)]
    public class LoadingButtonTagHelper : TagHelper
    {
        private const string CLASSES = "button-default button-loading";
        private const string TAG_NAME = "button";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = TAG_NAME;
            await HtmlAttributes.UpdateClassesToInclude(output, CLASSES);
        }
    }
}