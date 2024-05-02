namespace StockportWebapp.Comparers
{
    public class SlugComparer : IEqualityComparer<ISlugComparable>
    {
        public bool Equals(ISlugComparable x, ISlugComparable y)
        {
            if (ReferenceEquals(x, y)) 
                return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.Slug == y.Slug;
        }

        public int GetHashCode(ISlugComparable entry)
        {
            if (ReferenceEquals(entry, null)) 
                return 0;

            //Get hash code for the Name field if it is not null.
            return entry.Slug == null 
                    ? 0     
                    : entry.Slug.GetHashCode();
        }
    }
}