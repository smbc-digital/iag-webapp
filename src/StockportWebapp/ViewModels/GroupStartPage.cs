using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.ViewModels
{
    public class GroupStartPage
    {
        public List<GroupCategory> Categories = new List<GroupCategory>();
        public PrimaryFilter PrimaryFilter { set; get; }

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
