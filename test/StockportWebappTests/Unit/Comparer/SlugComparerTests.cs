using StockportWebapp.Comparers;

namespace StockportWebappTests_Unit.Unit.Comparers;

public class SlugComparerTests
{
    public DirectoryEntry entry = new() { Slug="TestSlug" };
    public DirectoryEntry entry2 = new() { Slug="TestSlug2" };
    
    [Fact]
    public void SlugComparer_Equals_Returns_True_ForSameObject()
    {
        SlugComparer comparer = new SlugComparer();
        var result = comparer.Equals(entry, entry);

        Assert.True(result);
    }

        [Fact]
    public void SlugComparer_Equals_Returns_False_ForDifferentObject()
    {
        SlugComparer comparer = new SlugComparer();
        var result = comparer.Equals(entry, entry2);

        Assert.False(result);
    }

    [Fact]
    public void SlugComparer_Equals_Returns_False_WhenObjectsNull()
    {
        SlugComparer comparer = new SlugComparer();

        Assert.False(comparer.Equals(entry, null));
        Assert.False(comparer.Equals(null, entry));
    }

    [Fact]
    public void SlugComparer_GetHashCode_Returns_0_WhenEntryNull()
    {
        SlugComparer comparer = new SlugComparer();
        Assert.Equal(0, comparer.GetHashCode(null));
    }

    [Fact]
    public void SlugComparer_GetHashCode_Returns_0_WhenEntrySlugNull()
    {
        SlugComparer comparer = new SlugComparer();
        var entry3 = new DirectoryEntry();
        Assert.Equal(0, comparer.GetHashCode(entry3));
    }

    [Fact]
    public void SlugComparer_GetHashCode_Returns_Value_WhenEntryIsValid()
    {
        SlugComparer comparer = new SlugComparer();
        var entry3 = new DirectoryEntry();
        Assert.NotEqual(0, comparer.GetHashCode(entry));
    }
}