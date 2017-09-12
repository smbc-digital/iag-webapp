using System.Collections.Generic;
using StockportWebapp.Models;
using StockportWebapp.Utils;
using System.Linq;

namespace StockportWebapp.ViewModels
{
    public class GroupResults
    {
        public List<Group> Groups = new List<Group>();
        public Pagination Pagination { get; set; }
        public QueryUrl CurrentUrl { get; private set; }
        public IFilteredUrl FilteredUrl { get; private set; }
        public List<GroupCategory> Categories = new List<GroupCategory>();
        public List<GroupSubCategory> AvailableSubCategories = new List<GroupSubCategory>();
        public List<string> SubCategories = new List<string>();
        public string Tag { get; set; } = string.Empty;
        public string KeepTag { get; set; } = string.Empty;
        public PrimaryFilter PrimaryFilter { set; get; } = new PrimaryFilter();
        public bool GetInvolved { get; set; }
        public string OrganisationName {
            get
            {
                var firstGroup = Groups.First(g => g.Organisation?.Slug == KeepTag);
                if (firstGroup == null || firstGroup.Organisation == null)
                {
                    return string.Empty;
                }
                else
                {
                    return firstGroup.Organisation.Title;
                }
            }
        }

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

            var subCategories = new RefineByFilters
            {
                Label = "Subcategories",
                Mandatory = false,
                Name = "subcategories",
                Items = new List<RefineByFilterItems>()
            };

            if (AvailableSubCategories != null && AvailableSubCategories.Any())
            {
                var distinctSubcategories = AvailableSubCategories.GroupBy(c => c.Slug).Select(c => c.First());
            
                foreach (var cat in distinctSubcategories.OrderBy(c => c.Name))
                {
                    subCategories.Items.Add(new RefineByFilterItems { Label = cat.Name, Checked = SubCategories.Any(c => c.ToLower() == cat.Slug.ToLower()), Value = cat.Slug });
                }

                bar.Filters.Add(subCategories);
            }           

            var getInvolved = new RefineByFilters
            {
                Label = "Get involved",
                Mandatory = false,
                Name = "getinvolved",
                Items = new List<RefineByFilterItems>
                {
                    new RefineByFilterItems { Label = "Volunteering opportunities", Checked = GetInvolved, Value = "yes" }
                }
            };

            bar.Filters.Add(getInvolved);

            if (!string.IsNullOrEmpty(KeepTag) || !string.IsNullOrEmpty(Tag))
            {
                var organisation = new RefineByFilters
                {
                    Label = "Organisation",
                    Mandatory = false,
                    Name = "tag",
                    Items = new List<RefineByFilterItems>
                    {
                        new RefineByFilterItems { Label = OrganisationName, Checked = !string.IsNullOrEmpty(Tag), Value = KeepTag }
                    }
                };

                bar.Filters.Add(organisation);
            }

            return bar;
        }
    }
}
