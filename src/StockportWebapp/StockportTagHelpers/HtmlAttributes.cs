using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StockportTagHelpers
{
    internal static class HtmlAttributes
    {
        internal static async Task UpdateClassesToInclude(TagHelperOutput output, string classesToAdd)
        {
            await Task.Run(() =>
            {
                if (output.Attributes.ContainsName("class"))
                {
                    output.Attributes.SetAttribute("class",
                        string.Concat(classesToAdd, " ", output.Attributes["class"].Value));
                }
                else
                {
                    output.Attributes.Add("class", classesToAdd);
                }

            });
        }
    }
}