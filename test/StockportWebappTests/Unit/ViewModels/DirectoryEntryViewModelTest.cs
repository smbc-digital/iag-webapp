using SharpKml.Dom;
using StockportWebapp.Model;

namespace StockportWebappTests_Unit.Unit.ViewModels;

public class DirectoryEntryViewModelTest
{
    [Fact]
    public void HighlightedFilters_ReturnsCorrectFilters()
    {
        // Arrange
        List<FilterTheme> themes = new()
        {
            new()
            {
                Filters = new List<Filter>
                {
                    new() { Highlight = true },
                    new() { Highlight = false },
                    new() { Highlight = true }
                }
            },
            new()
            {
                Filters = new List<Filter>
                {
                    new() { Highlight = false },
                    new() { Highlight = true }
                }
            }
        };

        DirectoryEntryViewModel directoryViewModel = new("slug", new())
        {
            DirectoryEntry = new DirectoryEntry
            {
                Themes = themes
            }
        };

        // Act
        IEnumerable<Filter> highlightedFilters = directoryViewModel.HighlightedFilters;

        // Assert
        Assert.NotNull(highlightedFilters);
        Assert.Equal(3, highlightedFilters.Count());
    }

    [Fact]
    public void HighlightedFilters_EmptyThemes_ReturnsEmptyList()
    {
        // Arrange
        DirectoryEntryViewModel directoryViewModel = new("slug", new()){
            DirectoryEntry = new DirectoryEntry
            {
                Themes = new List<FilterTheme>()
            }
        };

        // Act
        IEnumerable<Filter> highlightedFilters = directoryViewModel.HighlightedFilters;

        // Assert
        Assert.NotNull(highlightedFilters);
        Assert.Empty(highlightedFilters);
    }

    [Fact]
    public void HighlightedFilters_NullThemes_ReturnsNull()
    {
        // Arrange
        DirectoryEntryViewModel directoryViewModel = new("slug", new()){
            DirectoryEntry = new DirectoryEntry
            {
                Themes = null
            }
        };

        // Act
        IEnumerable<Filter> highlightedFilters = directoryViewModel.HighlightedFilters;

        // Assert
        Assert.Null(highlightedFilters);
    }

    [Fact]
    public void HighlightedFilters_NoHighlightedFilters_ReturnsEmptyList()
    {
        // Arrange
        List<FilterTheme> themes = new()
        {
            new()
            {
                Filters = new List<Filter>
                {
                    new() { Highlight = false },
                    new() { Highlight = false }
                }
            },
            new()
            {
                Filters = new List<Filter>
                {
                    new() { Highlight = false },
                    new() { Highlight = false }
                }
            }
        };

        DirectoryEntryViewModel directoryViewModel = new("slug", new()){
            DirectoryEntry = new DirectoryEntry
            {
                Themes = themes
            }
        };

        // Act
        IEnumerable<Filter> highlightedFilters = directoryViewModel.HighlightedFilters;

        // Assert
        Assert.NotNull(highlightedFilters);
        Assert.Empty(highlightedFilters);
    }

    [Theory]
    [InlineData(null, null, null, null, null, false)]
    [InlineData("", "", "", "", "", false)]
    [InlineData("facebook", null, null, null, null, true)]
    [InlineData(null, "twitter", null, null, null, true)]
    [InlineData(null, null, "youtube", null, null, true)]
    [InlineData(null, null, null, "instagram", null, true)]
    [InlineData(null, null, null, null, "linkedIn", true)]
    [InlineData("facebook", "twitter", "youtube", "instagram", "linkedIn", true)]
    public void DisplaySocials_ReturnsCorrectValue(string facebook, string twitter, string youtube, string instagram, string linkedIn, bool expected)
    {
        // Arrange
        var viewModel = new DirectoryEntryViewModel
        {
            DirectoryEntry = new DirectoryEntry
            {
                Facebook = facebook,
                Twitter = twitter,
                Youtube = youtube,
                Instagram = instagram,
                LinkedIn = linkedIn
            }
        };

        // Act
        bool displaySocials = viewModel.DisplaySocials;

        // Assert
        Assert.Equal(expected, displaySocials);
    }

    [Theory]
    [InlineData(null, null, false)]
    [InlineData("", "", false)]
    [InlineData("1234567890", null, true)]
    [InlineData(null, "test@example.com", true)]
    [InlineData("1234567890", "test@example.com", true)]
    public void HasPrimaryContact_ReturnsCorrectValue(string phoneNumber, string email, bool expected)
    {
        // Arrange
        DirectoryEntryViewModel viewModel = new()
        {
            DirectoryEntry = new DirectoryEntry
            {
                PhoneNumber = phoneNumber,
                Email = email
            }
        };

        // Act
        bool hasPrimaryContact = viewModel.HasPrimaryContact;

        // Assert
        Assert.Equal(expected, hasPrimaryContact);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("https://example.com", true)]
    public void DisplayContactUs_ReturnsCorrectValue(string website, bool expected)
    {
        // Arrange
        DirectoryEntryViewModel viewModel = new()
        {
            DirectoryEntry = new DirectoryEntry
            {
                Website = website   
            }
        };

        // Act
        bool displayContactUs = viewModel.DisplayContactUs;

        // Assert
        Assert.Equal(expected, displayContactUs);
    }

    [Fact]
    public void IsPinned_CorrectlySet()
    {
        // Act
        DirectoryEntryViewModel viewModel = new("test", new(), Enumerable.Empty<Crumb>(), new MapDetails(), true);
        
        // Assert
        Assert.True(viewModel.IsPinned);
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0, 2.4555, false)]
    [InlineData(4.5222, 0, false)]
    [InlineData(4.5222, 2.4555, true)]
    public void ShowMapPin_ReturnsCorrectValue(double lat, double lon, bool expected)
    {
        // Arrange
        DirectoryEntryViewModel viewModel = new()
        {
            DirectoryEntry = new DirectoryEntry
            {
                MapPosition = new MapPosition(){
                    Lat = lat,
                    Lon = lon
                }
            }
        };

        // Act
        bool showMapPin = viewModel.ShowMapPin;

        // Assert
        Assert.Equal(expected, showMapPin);
    }

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(0, 2.4555, false)]
    [InlineData(4.5222, 0, false)]
    [InlineData(4.5222, 2.4555, true)]
    public void DisplayMap_ReturnsCorrectValue(double lat, double lon, bool expected)
    {
        // Arrange
        MapPosition mapPosition = new() {
            Lat = lat,
            Lon = lon
        };

        DirectoryEntryViewModel viewModel = new()
        {
            DirectoryEntry = new DirectoryEntry
            {
                MapPosition = new MapPosition(){
                    Lat = lat,
                    Lon = lon
                }
            },
            MapDetails = new MapDetails(){
                MapPosition = mapPosition
            }
        };

        // Act
        bool displayMap = viewModel.DisplayMap;

        // Assert
        Assert.Equal(expected, displayMap);
    }

    [Theory]
    [InlineData("<p>This is a <b>test</b> address</p>", "This is a test address")]
    [InlineData("<div>This is another <i>example</i> address</div>", "This is another example address")]
    [InlineData("This is a plain text address", "This is a plain text address")]
    [InlineData("", "")]
    public void AddressWithoutTags_RemovesHtmlTags(string address, string expected)
    {
        // Arrange
        DirectoryEntryViewModel viewModel = new()
        {
            DirectoryEntry = new DirectoryEntry
            {
                Address = address
            }
        };

        // Act
        string addressWithoutTags = viewModel.AddressWithoutTags;

        // Assert
        Assert.Equal(expected, addressWithoutTags);
    }

    [Theory]
    [InlineData(1.2345, 2.3456, "Name", "Teaser", true, 1, "http://example.com", "position: { lat: 1.2345, lng: 2.3456 }, title: \"Name\", content: \"<div class='google-map--padding'><h3 class='h-m'>Name</h3><p class='body'>Teaser</p><hr/><a href='http://example.com' class='btn btn_small btn--width-25 btn--chevron-forward btn--chevron-bold'><span class='btn_text'>View Name</span></a></div>\", isPinned: true, mapPinIndex: 1")]
    [InlineData(0, 0, "", "", false, 0, "", "position: { lat: 0, lng: 0 }, title: \"\", content: \"<div class='google-map--padding'><h3 class='h-m'></h3><p class='body'></p><hr/><a href='' class='btn btn_small btn--width-25 btn--chevron-forward btn--chevron-bold'><span class='btn_text'>View </span></a></div>\", isPinned: false, mapPinIndex: 0")]
    [InlineData(-1.5, 3.7, "Test Name", "Test Teaser", true, 2, "http://test.com", "position: { lat: -1.5, lng: 3.7 }, title: \"Test Name\", content: \"<div class='google-map--padding'><h3 class='h-m'>Test Name</h3><p class='body'>Test Teaser</p><hr/><a href='http://test.com' class='btn btn_small btn--width-25 btn--chevron-forward btn--chevron-bold'><span class='btn_text'>View Test Name</span></a></div>\", isPinned: true, mapPinIndex: 2")]
    [InlineData(10.123, -20.987, "Long Name", "Long Teaser", false, 5, "http://long.com", "position: { lat: 10.123, lng: -20.987 }, title: \"Long Name\", content: \"<div class='google-map--padding'><h3 class='h-m'>Long Name</h3><p class='body'>Long Teaser</p><hr/><a href='http://long.com' class='btn btn_small btn--width-25 btn--chevron-forward btn--chevron-bold'><span class='btn_text'>View Long Name</span></a></div>\", isPinned: false, mapPinIndex: 5")]
    public void ToString_ReturnsCorrectString(double lat,
                                            double lon,
                                            string name,
                                            string teaser,
                                            bool isPinned,
                                            int mapPinIndex,
                                            string url,
                                            string expected)
    {
        // Arrange
        DirectoryEntryViewModel viewModel = new()
        {
            DirectoryEntry = new DirectoryEntry
            {
                MapPosition = new(){
                    Lat = lat,
                    Lon = lon
                },
                Name = name,
                Teaser = teaser,
            },
            IsPinned = isPinned,
            MapPinIndex = mapPinIndex
        };

        // Act
        string result = viewModel.ToString(url);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToKmlPlacemark_ConstructsPlacemarkCorrectly()
    {
        // Arrange
        DirectoryEntryViewModel viewModel = new()
        {
            DirectoryEntry = new DirectoryEntry
            {
                MapPosition = new(){
                    Lat = 1.2345,
                    Lon = 2.3456
                },
                Name = "Test name",
                Teaser = "Test teaser",
            },
        };

        // Act
        Placemark placemark = viewModel.ToKmlPlacemark("pinnedStyle");
        Placemark placemark2 = viewModel.ToKmlPlacemark();

        // Assert
        Assert.NotNull(placemark);
        Assert.NotNull(placemark.Geometry);
        Assert.IsType<Point>(placemark.Geometry);
        Assert.IsType<Description>(placemark.Description);
        Assert.NotNull(placemark.Description);
        Assert.NotNull(placemark.PhoneNumber);
        Assert.NotNull(placemark.Address);
        Assert.NotNull(placemark.AtomLink);
        Assert.IsType<SharpKml.Dom.Atom.Link>(placemark.AtomLink);
        Assert.Null(placemark2.StyleUrl);
        Assert.NotNull(placemark.StyleUrl);
        Assert.IsType<Uri>(placemark.StyleUrl);
    }
}