using Microsoft.AspNetCore.Razor.TagHelpers;
using HtmlString = Microsoft.AspNetCore.Html.HtmlString;

namespace StockportTagHelpers
{
    [HtmlTargetElement("profile", ParentTag = null)]
    public class ProfileTagHelpers : TagHelper
    {
        private const string TagName = "div";
        private const string Template = @"
            <h5 class=""profile-heading"">{0}</h5>
            <div class=""grid-25 tablet-grid-25"">
                <div class=""profile-image-mask"">
                    <img src=""{1}"" />
                </div>
            </div>
            <div class=""grid-75 tablet-grid-75"">
                <h2>{2}</h2>
                <p class=""profile-subtitle"">{3}</p>
                <p>{4}</p>
                <a class=""button button-outline button-dark button-chevron"" href=""{5}"">
                    Read More
                    <span aria-hidden=""true"" class=""fa fa-angle-right""></span>
                </a>
            </div>
            ";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var heading = context.AllAttributes["heading"].Value;
            var image = context.AllAttributes["image"].Value;
            var name = context.AllAttributes["name"].Value;
            var subtitle = context.AllAttributes["subtitle"].Value;
            var link = context.AllAttributes["link"].Value;
            var content = await output.GetChildContentAsync();
            var encodedeContent = content.GetContent();

            output.Attributes.RemoveAll("heading");
            output.Attributes.RemoveAll("image");
            output.Attributes.RemoveAll("name");
            output.Attributes.RemoveAll("subtitle");
            output.Attributes.RemoveAll("link");

            var outputHtml = string.Format(Template, heading, image, name, subtitle, encodedeContent, link);

            output.Content.SetHtmlContent(new HtmlString(outputHtml));

            output.TagName = TagName;

            await HtmlAttributes.UpdateClassesToInclude(output, "profile profile-success-story");
        }
    }
}
