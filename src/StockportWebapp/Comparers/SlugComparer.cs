namespace StockportWebapp.Comparers
{
    public class SlugComparer : IEqualityComparer<ISlugComparable>
    {
        public bool Equals(ISlugComparable x, ISlugComparable y)
        {
            if (ReferenceEquals(x, y)) 
                return true;

            if (x is null || y is null)
                return false;

            return x.Slug.Equals(y.Slug);
        }

        public int GetHashCode(ISlugComparable entry)
        {
            if (entry is null) 
                return 0;

            //Get hash code for the Name field if it is not null.
            return entry.Slug is null 
                    ? 0     
                    : entry.Slug.GetHashCode();
        }
    }
}