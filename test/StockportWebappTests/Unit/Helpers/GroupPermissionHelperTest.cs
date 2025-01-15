namespace StockportWebappTests_Unit.Unit.Helpers;

public class GroupPermissionHelperTest
{
    [Theory]
    [InlineData("A", "Administrator")]
    [InlineData("E", "Editor")]
    [InlineData("a", "Administrator")]
    [InlineData("e", "Editor")]
    [InlineData("Q", "")]
    [InlineData("QWERTY", "")]
    [InlineData("", "")]
    public void ShouldReturnCorrectPermissionName(string input, string output)
    {
        // Act
        string result = GroupPermissionHelper.GetPermisison(input);

        // Assert
        Assert.Equal(output, result);
    }
}