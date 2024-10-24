using StockportWebapp.Models.Mappers;

namespace StockportWebappTests_Unit.Unit.Models.Mappers;

public class CssClassMapperTests
{
    [Theory]
    [InlineData(EColourScheme.None, "-none")]
    [InlineData(EColourScheme.Blue, "-blue")]
    [InlineData(EColourScheme.Blue_Light, "-blue-light")]
    [InlineData(EColourScheme.Dark_Overlay, "-dark")]
    [InlineData(EColourScheme.Green, "-green")]
    [InlineData(EColourScheme.Green_Light, "-green-light")]
    [InlineData(EColourScheme.Grey, "-grey")]
    [InlineData(EColourScheme.Grey_Light, "-grey-light")]
    [InlineData(EColourScheme.Light_Overlay, "-light")]
    [InlineData(EColourScheme.Multi, "-multi")]
    [InlineData(EColourScheme.One_Stockport_Purple, "-one-stockport-purple")]
    [InlineData(EColourScheme.One_Stockport_Purple_Light, "-one-stockport-purple-light")]
    [InlineData(EColourScheme.Orange, "-orange")]
    [InlineData(EColourScheme.Pink, "-pink")]
    [InlineData(EColourScheme.Pink_Light, "-pink-light")]
    [InlineData(EColourScheme.Purple, "-purple")]
    [InlineData(EColourScheme.Purple_Light, "-purple-light")]
    [InlineData(EColourScheme.Teal, "-teal")]
    [InlineData(EColourScheme.Teal_Light, "-teal-light")]
    public void GetCssClass_ShouldReturnCorrectClassSubstring(EColourScheme colourScheme, string expectedResult)
    {
        // Act
        string result = CssClassMapper.GetCssClass(colourScheme);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(EColourScheme.None, "-none")]
    [InlineData(EColourScheme.Blue, "-blue")]
    [InlineData(EColourScheme.Blue_Light, "-blue")]
    [InlineData(EColourScheme.Dark_Overlay, "-dark")]
    [InlineData(EColourScheme.Green, "-green")]
    [InlineData(EColourScheme.Green_Light, "-green")]
    [InlineData(EColourScheme.Grey, "-grey")]
    [InlineData(EColourScheme.Grey_Light, "-grey")]
    [InlineData(EColourScheme.Light_Overlay, "-light")]
    [InlineData(EColourScheme.Multi, "-multi")]
    [InlineData(EColourScheme.One_Stockport_Purple, "-one-stockport-purple")]
    [InlineData(EColourScheme.One_Stockport_Purple_Light, "-one-stockport-purple")]
    [InlineData(EColourScheme.Orange, "-orange")]
    [InlineData(EColourScheme.Pink, "-pink")]
    [InlineData(EColourScheme.Pink_Light, "-pink")]
    [InlineData(EColourScheme.Purple, "-purple")]
    [InlineData(EColourScheme.Purple_Light, "-purple")]
    [InlineData(EColourScheme.Teal, "-teal")]
    [InlineData(EColourScheme.Teal_Light, "-teal")]
    public void GetBoldCssClass_ShouldReturnCorrectClassSubstring(EColourScheme colourScheme, string expectedResult)
    {
        // Act
        string result = CssClassMapper.GetBoldCssClass(colourScheme);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
