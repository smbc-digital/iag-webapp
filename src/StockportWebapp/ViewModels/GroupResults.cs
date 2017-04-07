using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class GroupResults
    {
        public List<Group> Groups = new List<Group>();
        public List<GroupCategory> Categories = new List<GroupCategory>();

        public GroupResults() { }

    }
}
