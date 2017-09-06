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
        public PrimaryFilter PrimaryFilter { set; get; } = new PrimaryFilter();
        public bool GetInvolved { get; set; }

        public GroupResults() { }

        public void AddFilteredUrl(IFilteredUrl filteredUrl)
        {
            FilteredUrl = filteredUrl;
        }

        public void AddQueryUrl(QueryUrl queryUrl)
        {
            CurrentUrl = queryUrl;
        }

        public RefineByBar RefineByBar()
        {
            var bar = new RefineByBar
            {
                ShowLocation = false,
                KeepLocationQueryValues = true,
                MobileFilterText = "Filter",
                Filters = new List<RefineByFilters>()
            };

            var price = new RefineByFilters
            {
                Label = "Get involved",
                Mandatory = false,
                Name = "getinvolved",
                Items = new List<RefineByFilterItems>
                {
                    new RefineByFilterItems { Label = "Volunteering opportunities", Checked = GetInvolved, Value = "yes" }
                }
            };

            bar.Filters.Add(price);

            return bar;
        }
    }
}
