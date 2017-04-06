using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Models;

namespace StockportWebapp.Utils
{
    public static class GroupImageUrlHelper
    {
        public static string GetImageUrl(Group group)
        {
            string imageURL;
            if (string.IsNullOrEmpty(group.ImageUrl))
            {
                if (group.CategoriesReference.Count > 0)
                    imageURL = GetFirstCategoryThatHasAnImageUrl(group.CategoriesReference);
                else
                    imageURL = "";
            }
            else
            {
                imageURL = group.ImageUrl;
            }

            return imageURL;
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
