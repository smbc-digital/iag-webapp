using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class GroupSearch
    {
        public string Category { get; set; }
        public List<string> SubCategories { get; set; } = new List<string>();
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Order { get; set; }
        public string Location { get; set; } = "Stockport";
        public string Tag { get; set; }
        public string KeepTag { get; set; }
        public string GetInvolved { get; set; } = string.Empty;
    }
}