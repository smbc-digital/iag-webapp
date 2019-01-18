using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;

namespace StockportWebapp.ContentFactory.InformationFactory
{
    public interface IInformationFactory
    {
        List<ProcessedInformationItem> Build(List<InformationItem> informationList);

        ProcessedInformationItem Build(InformationItem informationPoint);
    }
}
