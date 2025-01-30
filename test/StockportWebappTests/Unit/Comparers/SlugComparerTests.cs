using StockportWebapp.Comparers;

namespace StockportWebappTests_Unit.Unit.Comparers;

public class SlugComparerTests
{
    public DirectoryEntry entry = new() { Slug="TestSlug" };
    public DirectoryEntry entry2 = new() { Slug="TestSlug2" };
    
    [Fact]
    public void SlugComparer_Equals_Returns_True_ForSameObject()
    {
        // Arrange 
        SlugComparer comparer = new();
        
        // Act & Assert
        Assert.True(comparer.Equals(entry, entry));
    }

        [Fact]
    public void SlugComparer_Equals_Returns_False_ForDifferentObject()
    {
        // Arrange
        SlugComparer comparer = new();
        
        // Act & Assert
        Assert.False(comparer.Equals(entry, entry2));
    }

    [Fact]
    public void SlugComparer_Equals_Returns_False_WhenObjectsNull()
    {
        // Arrange
        SlugComparer comparer = new();

        // Act & Assert
        Assert.False(comparer.Equals(entry, null));
        Assert.False(comparer.Equals(null, entry));
    }

    [Fact]
    public void SlugComparer_GetHashCode_Returns_0_WhenEntryNull()
    {
        // Arrange
        SlugComparer comparer = new();

        // Act & Assert
        Assert.Equal(0, comparer.GetHashCode(null));
    }

    [Fact]
    public void SlugComparer_GetHashCode_Returns_0_WhenEntrySlugNull()
    {
        // Arrange
        SlugComparer comparer = new();
        DirectoryEntry entry3 = new();

        // Act & Assert
        Assert.Equal(0, comparer.GetHashCode(entry3));
    }

    [Fact]
    public void SlugComparer_GetHashCode_Returns_Value_WhenEntryIsValid()
    {
        // Arrange
        SlugComparer comparer = new();
        
        // Act & Assert
        Assert.NotEqual(0, comparer.GetHashCode(entry));
    }
}