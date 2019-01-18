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
                processedInformationList.Add(Build(item));
            }


            return processedInformationList;
        }

        public ProcessedInformationItem Build(InformationItem informationPoint)
        {
            var processedInformationPoint = new ProcessedInformationItem
            (
                informationPoint.Name,
                informationPoint.Icon,
                _markdownWrapper.ConvertToHtml(informationPoint.Text),
                informationPoint.Link
            );

            return processedInformationPoint;
        }
    }
}
