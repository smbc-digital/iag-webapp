using StockportWebapp.Models;
using StockportWebapp.Parsers;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
    public class GroupCategoryFactory
    { 
        public GroupCategoryFactory()
        {        
        }

        public virtual ProcessedGroupCategory Build(GroupCategory groupCategory)
        {
            return new ProcessedGroupCategory(
                groupCategory.Name,
                groupCategory.Slug,
                groupCategory.Icon,
                groupCategory.ImageUrl
                );
        }
    }
}
