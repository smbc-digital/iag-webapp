using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.ViewModels
{
    public class GroupResults
    {
        public List<Group> Groups = new List<Group>();
        public Pagination Pagination { get; set; }
        public QueryUrl CurrentUrl { get; private set; }
        public IFilteredUrl FilteredUrl { get; private set; }
        public List<GroupCategory> Categories = new List<GroupCategory>();
        public PrimaryFilter PrimaryFilter { set; get; }

        public GroupResults() { }

        public void AddFilteredUrl(IFilteredUrl filteredUrl)
        {
            FilteredUrl = filteredUrl;
        }

        public void AddQueryUrl(QueryUrl queryUrl)
        {
            CurrentUrl = queryUrl;
        }
    }
}
