using StockportWebapp.Extensions;

namespace StockportWebappTests_Unit.Unit.Extensions;

public class DirectoryExtensionsTests
{
    [Fact]
    public void GetKmlForList_ShouldReturnValidKml()
    {
        // Arrange
        List<DirectoryEntry> directoryEntries = new()
        {
            new(){
                Slug = "slug",
                Title = "Title",
                Body = "Body",
                Teaser = "Teaser",
                MetaDescription = "MetaDescription",
                Themes = new List<FilterTheme>(),
                Directories = new List<MinimalDirectory>(),
                Branding = new List<GroupBranding>(),
                MapPosition = new MapPosition(),
                PhoneNumber = "phone number",
                Email = "email",
                Website = "website",
                Twitter = "twitter",
                Facebook = "facebook",
                Address = "address",
            }
        };

        // Act
        string result = directoryEntries.GetKmlForList();

        // Assert
        Assert.NotEmpty(result);
        // Add more asserts once the method works as intended and does not use hard-coded values
    }
}
