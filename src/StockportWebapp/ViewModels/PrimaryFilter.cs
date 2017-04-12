using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class PrimaryFilter
    {
        public List<GroupCategory> Categories { set; get; }
        public List<string> Orders = new List<string> {"Name A-Z", "Name Z-A", "Nearest"};
    }
}