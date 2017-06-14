using System.Collections.Generic;

namespace StockportWebapp.ViewModels
{
    public class MultiSelect
    {
        public int Limit { get; set; }

        public string ObjectName { get; set; }

        public string ValueControlId { get; set; }

        public List<string> AvailableValues { get; set; }
    }
}
