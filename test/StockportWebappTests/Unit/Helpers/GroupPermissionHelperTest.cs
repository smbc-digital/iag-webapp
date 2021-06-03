using FluentAssertions;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Helpers
{
    public class GroupPermissionHelperTest
    {
        public GroupPermissionHelperTest()
        {
        }

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
            // Arrange

            // Act
            var result = GroupPermissionHelper.GetPermisison(input);

            // Assert
            result.Should().Be(output);
        }
    }
}
