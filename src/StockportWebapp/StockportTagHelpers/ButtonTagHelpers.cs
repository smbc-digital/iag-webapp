using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StockportTagHelpers
{
    [HtmlTargetElement("stock-button", ParentTag = null)]
    public class DefaultButtonTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context.AllAttributes.ContainsName("as-link"))
            {
                output.TagName = "a";
                output.Attributes.RemoveAll("as-link");
            }
            else
            {
                output.TagName = "button";
            }

            await HtmlAttributes.UpdateClassesToInclude(output, "button-default");
        }
    }

    [HtmlTargetElement("stock-button-loading", ParentTag = null)]
    public class LoadingButtonTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            await HtmlAttributes.UpdateClassesToInclude(output, "button-default button-loading");
        }
    }
}