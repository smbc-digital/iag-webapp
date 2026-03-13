namespace StockportWebappTests_Unit.Unit.Models.Mappers;

public class CssClassMapperTests
{
    [Theory]
    [InlineData(EColourScheme.None, "-none")]
    [InlineData(EColourScheme.Blue, "-blue")]
    [InlineData(EColourScheme.Dark_Overlay, "-dark")]
    [InlineData(EColourScheme.Green, "-green")]
    [InlineData(EColourScheme.Grey, "-grey")]
    [InlineData(EColourScheme.Light_Overlay, "-light")]
    [InlineData(EColourScheme.Multi, "-multi")]
    [InlineData(EColourScheme.One_Stockport_Purple, "-one-stockport-purple")]
    [InlineData(EColourScheme.Pink, "-pink")]
    [InlineData(EColourScheme.Purple, "-purple")]
    [InlineData(EColourScheme.Teal, "-teal")]
    [InlineData(EColourScheme.OS_Lilac, "-os-lilac")]
    [InlineData(EColourScheme.OS_Pink, "-os-pink")]
    [InlineData(EColourScheme.OS_Purple, "-os-purple")]
    [InlineData(EColourScheme.OS_Teal, "-os-teal")]
    [InlineData(EColourScheme.OS_Yellow, "-os-yellow")]
    public void GetCssClass_ShouldReturnCorrectClassSubstring(EColourScheme colourScheme, string expectedResult)
    {
        // Act
        string result = CssClassMapper.GetCssClass(colourScheme);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}