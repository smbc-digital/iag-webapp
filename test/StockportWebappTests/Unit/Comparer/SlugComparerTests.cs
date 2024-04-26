using StockportWebapp.Comparers;

namespace StockportWebappTests_Unit.Unit.Comparers;

public class SlugComparerTests
{
    [Fact]
    public void SlugComparer_Equals_Returns_True_ForSameObject()
    {
        SlugComparer comparer = new SlugComparer();
        DirectoryEntry entry = new DirectoryEntry() { Slug="TestSlug" };
        var result = comparer.Equals(entry, entry);

        Assert.True(result);
    }

        [Fact]
    public void SlugComparer_Equals_Returns_False_ForDifferentObject()
    {
        SlugComparer comparer = new SlugComparer();
        DirectoryEntry entry = new DirectoryEntry() { Slug="TestSlug" };
        DirectoryEntry entry2 = new DirectoryEntry() { Slug="TestSlug2" };
        var result = comparer.Equals(entry, entry2);

        Assert.False(result);
    }

    [Fact]
    [InlineData]
    public void SlugComparer_Equals_Returns_False_ForDifferentObject()
    {
        SlugComparer comparer = new SlugComparer();
        DirectoryEntry entry = new DirectoryEntry() { Slug="TestSlug" };
        DirectoryEntry entry2 = new DirectoryEntry() { Slug="TestSlug2" };
        var result = comparer.Equals(entry, entry2);

        Assert.False(result);
    }
}