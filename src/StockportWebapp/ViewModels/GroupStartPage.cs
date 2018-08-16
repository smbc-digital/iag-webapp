using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class GroupStartPage
    {
        public List<GroupCategory> Categories = new List<GroupCategory>();
        public PrimaryFilter PrimaryFilter { set; get; }
        public string BackgroundImage { set; get; }
        public string FeaturedGroupsHeading { get; set; }
        public List<Group> FeaturedGroups { get; set; }
        public GroupCategory FeaturedGroupsCategory { get; set; }
        public GroupSubCategory FeaturedGroupsSubCategory { get; set; }
        public List<Alert> Alerts { get; set; }
        public string Body { get; set; }
        public string SecondaryBody { get; set; }

        public GroupStartPage() { }

        public GenericFeaturedItemList GenericItemList
        {
            get
            {
                var result = new GenericFeaturedItemList();
                result.Items = new List<GenericFeaturedItem>();
                foreach (var cat in Categories)
                {
                    result.Items.Add(new GenericFeaturedItem { Icon = cat.Icon, Title = cat.Name, Url = $"/groups/results?category={cat.Slug}&order=Name+A-Z" });
                }

                result.ButtonText = "View more categories";

                return result;
            }
        }
    }
}
