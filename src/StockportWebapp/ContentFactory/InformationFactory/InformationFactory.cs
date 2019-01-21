using StockportWebapp.Utils;
using System.Collections.Generic;
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
            var processedInformationList = new List<ProcessedInformationItem>();

            foreach (var item in informationList)
            {
                processedInformationList.Add(new ProcessedInformationItem
                (
                    item.Name,
                    item.Icon,
                    _markdownWrapper.ConvertToHtml(item.Text),
                    item.Link
                ));
            }

            return processedInformationList;
        }
    }
}
