using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class AtoZViewModel
    {
        public List<AtoZ> Items { get; set; }
        public string CurrentLetter { get; set; }
        public List<Crumb> Breadcrumbs { get; set; }
    }
}
