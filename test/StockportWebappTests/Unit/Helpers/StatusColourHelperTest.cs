namespace StockportWebappTests_Unit.Unit.Helpers;

public class StatusColourHelperTest
{
    [Theory]
    [InlineData("Published", "green")]
    [InlineData("Archived", "red")]
    [InlineData("", "green")]
    public void ShouldReturnCorrectColour(string status, string colour)
    {
        // Act
        string returnColour = StatusColourHelper.GetStatusColour(status);

        // Assert
        Assert.Equal(colour, returnColour);
    }
}