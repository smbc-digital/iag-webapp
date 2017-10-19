using StockportWebapp.ProcessedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.ViewModels
{
    public class GroupDetailsViewModel
    {
        public ProcessedGroup Group { get; set; }
        public bool ShouldShowAdditionalInformation { get; set; }
    }
}
