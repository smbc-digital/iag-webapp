namespace StockportWebappTests_Unit.Unit.Models;

public class EColourSchemeTests
{
    [Theory]
    [InlineData(EColourScheme.OS_Lilac)]
    [InlineData(EColourScheme.OS_Pink)]
    [InlineData(EColourScheme.OS_Purple)]
    [InlineData(EColourScheme.OS_Multi)]
    [InlineData(EColourScheme.OS_Teal)]
    [InlineData(EColourScheme.OS_Yellow)]
    public void IsOSStyle_WhenGivenOSStyleColourScheme_ShouldReturnTrue(EColourScheme colourScheme)
    {
        // Act
        var result = colourScheme.IsOSStyle();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(EColourScheme.None)]
    [InlineData(EColourScheme.Blue)]
    [InlineData(EColourScheme.Dark_Overlay)]
    [InlineData(EColourScheme.Green)]
    [InlineData(EColourScheme.Grey)]
    [InlineData(EColourScheme.Light_Overlay)]
    [InlineData(EColourScheme.Multi)]
    [InlineData(EColourScheme.One_Stockport_Purple)]
    [InlineData(EColourScheme.Orange)]
    [InlineData(EColourScheme.Pink)]
    [InlineData(EColourScheme.Purple)]
    [InlineData(EColourScheme.Teal)]
    public void IsOSStyle_WhenGivenNonOSStyleColourScheme_ShouldReturnFalse(EColourScheme colourScheme)
    {
        // Act
        var result = colourScheme.IsOSStyle();

        // Assert
        result.Should().BeFalse();
    }
}
