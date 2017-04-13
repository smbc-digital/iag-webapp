using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class GroupStartPage
    {
        public List<GroupCategory> Categories = new List<GroupCategory>();
        public PrimaryFilter PrimaryFilter { set; get; }

        public GroupStartPage() { }
        
    }
}
