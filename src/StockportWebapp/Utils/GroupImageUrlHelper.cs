using System.Collections.Generic;
using StockportWebapp.Models;

namespace StockportWebapp.Utils
{
    public static class GroupImageUrlHelper
    {
        public static string GetImageUrl(Group group)
        {
            return group.CategoriesReference.Count > 0 ? GetFirstCategoryThatHasAnImageUrl(group.CategoriesReference) : "";
        }

        private static string GetFirstCategoryThatHasAnImageUrl(List<GroupCategory> groupCategories)
        {
            foreach (GroupCategory category in groupCategories)
            {
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    return category.ImageUrl;
                }
            }

            return "";
        }
    }
}
