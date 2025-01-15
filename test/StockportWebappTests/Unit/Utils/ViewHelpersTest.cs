namespace StockportWebappTests_Unit.Unit.Utils;

public class ViewHelpersTest
{
    private readonly Mock<ITimeProvider> _timeProvider = new();

    public ViewHelpersTest() =>
        _timeProvider
            .Setup(provider => provider.Now())
            .Returns(new DateTime(2017, 02, 1));

    [Theory]
    [InlineData(10, "09:30", "Saturday 11 February at 9:30am")]
    [InlineData(0, "09:30", "Today at 9:30am")]
    [InlineData(1, "09:30", "Tomorrow at 9:30am")]
    [InlineData(10, "19:30", "Saturday 11 February at 7:30pm")]
    [InlineData(0, "12:00", "Today at 12:00pm")]
    public void FormatEventDateShouldReturnCorrectDateString(int daysOffset, string time, string expected)
    {
        // Arrange
        DateTime date = _timeProvider.Object.Now().AddDays(daysOffset);
        ViewHelpers viewHelper = new(_timeProvider.Object);

        // Act
        string result = viewHelper.FormatEventDate(date, time);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("<b>Bold Text</b>", "<b>Bold Text</b>")]
    [InlineData("<b class=\"bolder\">Bold Text</b>", "<b class=\"bolder\">Bold Text</b>")]
    [InlineData("<i>Italic Text</i>", "Italic Text")]
    [InlineData("<span class=\"fancy\">Italic Text</span>", "Italic Text")]
    [InlineData("&lt;b&gt;Bold Text&lt;/b&gt;", "Bold Text")]
    [InlineData("<script type=\"text/javscript\">alert('foo');</script>", "alert('foo');")]
    public void FormatWysiwygTextToOnlyAllowAllowedHtml(string html, string expected)
    {
        // Arrange
        ViewHelpers viewHelper = new(_timeProvider.Object);

        // Act
        string result = viewHelper.StripUnwantedHtml(html);

        // Assert
        Assert.Equal(expected, result);
    }
}