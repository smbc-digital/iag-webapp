namespace StockportWebapp.Utils;

public static class GroupImageUrlHelper
{
    public static string GetImageUrl(Group group) =>
        group.CategoriesReference.Count > 0
            ? GetFirstCategoryThatHasAnImageUrl(group.CategoriesReference)
            : string.Empty;

    private static string GetFirstCategoryThatHasAnImageUrl(List<GroupCategory> groupCategories)
    {
        foreach (GroupCategory category in groupCategories.Where(category => !string.IsNullOrEmpty(category.ImageUrl)))
            return category.ImageUrl;

        return string.Empty;
    }
}