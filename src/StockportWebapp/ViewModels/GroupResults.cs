namespace StockportWebapp.ViewModels;

[Obsolete("Groups is being replaced by directories/directory entries")]
[ExcludeFromCodeCoverage(Justification = "Obsolete")]
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
    public string OrganisationName { get; set; }

    public GroupResults() { }

    public void AddFilteredUrl(IFilteredUrl filteredUrl) =>
        FilteredUrl = filteredUrl;

    public void AddQueryUrl(QueryUrl queryUrl) =>
        CurrentUrl = queryUrl;

    public RefineByBar RefineByBar()
    {
        RefineByBar bar = new()
        {
            ShowLocation = false,
            KeepLocationQueryValues = true,
            MobileFilterText = "Filter",
            Filters = new List<RefineByFilters>()
        };

        RefineByFilters subCategories = new()
        {
            Label = "Subcategories",
            Mandatory = false,
            Name = "subcategories",
            Items = new List<RefineByFilterItems>()
        };

        if (AvailableSubCategories is not null && AvailableSubCategories.Any())
        {
            IEnumerable<GroupSubCategory> distinctSubcategories = AvailableSubCategories
                                                                    .GroupBy(c => c.Slug)
                                                                    .Select(c => c.First());

            foreach (GroupSubCategory cat in distinctSubcategories.OrderBy(c => c.Name))
            {
                subCategories.Items.Add(new RefineByFilterItems
                    {
                        Label = cat.Name,
                        Checked = SubCategories.Any(c => c.ToLower().Equals(cat.Slug.ToLower())),
                        Value = cat.Slug
                    });
            }

            bar.Filters.Add(subCategories);
        }

        RefineByFilters getInvolved = new()
        {
            Label = "Get involved",
            Mandatory = false,
            Name = "getinvolved",
            Items = new List<RefineByFilterItems>
            {
                new()
                    {
                        Label = "Volunteering opportunities",
                        Checked = GetInvolved,
                        Value = "yes"
                    }
            }
        };

        bar.Filters.Add(getInvolved);

        if (!string.IsNullOrEmpty(KeepTag) || !string.IsNullOrEmpty(Tag))
        {
            RefineByFilters organisation = new()
            {
                Label = "Organisation",
                Mandatory = false,
                Name = "tag",
                Items = new List<RefineByFilterItems>
                {
                    new()
                    {
                        Label = OrganisationName,
                        Checked = !string.IsNullOrEmpty(Tag),
                        Value = KeepTag
                    }
                }
            };

            bar.Filters.Add(organisation);
        }

        return bar;
    }
}