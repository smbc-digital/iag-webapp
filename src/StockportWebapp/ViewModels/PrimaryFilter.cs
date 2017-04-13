using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class PrimaryFilter
    {
        public string Category { get; set; }
        public List<GroupCategory> Categories { set; get; } 
        public string Order { get; set; }
        public List<string> Orders = new List<string> {"Name A-Z", "Name Z-A", "Nearest"};
    }
}