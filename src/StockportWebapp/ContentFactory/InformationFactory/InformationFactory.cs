using StockportWebapp.Utils;
using System.Collections.Generic;
using System.Linq;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ContentFactory.InformationFactory
{
    public class InformationFactory : IInformationFactory
    {
        private readonly MarkdownWrapper _markdownWrapper;

        public InformationFactory(MarkdownWrapper markdownWrapper)
        {
            _markdownWrapper = markdownWrapper;
        }

        public List<ProcessedInformationItem> Build(List<InformationItem> informationList)
        {
            return informationList?.Select(item => new ProcessedInformationItem
            (
                item.Name,
                item.Icon,
                _markdownWrapper.ConvertToHtml(item.Text),
                item.Link
            )).ToList();
        }
    }
}
