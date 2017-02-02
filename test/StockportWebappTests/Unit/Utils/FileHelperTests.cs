using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StockportWebapp.Utils;
using Xunit;

namespace StockportWebappTests.Unit.Utils
{
    public class FileHelperTests
    {
        [Fact]
        public void ShouldReturnFileNameWhenNoDirectoryIsInThePath()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(o => o.FileName).Returns("test.jpg");

            var file = FileHelper.GetFileNameFromPath(mockFile.Object);

            file.Should().Be("test.jpg");
        }

        [Fact]
        public void ShouldReturnEmptyStringForNoFileName()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(o => o.FileName).Returns(string.Empty);

            var file = FileHelper.GetFileNameFromPath(mockFile.Object);

            file.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldReturnEmptyForNoFile()
        {
            var file = FileHelper.GetFileNameFromPath(null);

            file.Should().Be(string.Empty);
        }

        [Fact]
        public void ShouldReturnFileNameForWindowsPath()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(o => o.FileName).Returns("C:\\test\\testing\\test.jpg");

            var file = FileHelper.GetFileNameFromPath(mockFile.Object);

            file.Should().Be("test.jpg");
        }

        [Fact]
        public void ShouldReturnFileNameForLinuxPath()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(o => o.FileName).Returns("C://test//testing//test.jpg");

            var file = FileHelper.GetFileNameFromPath(mockFile.Object);

            file.Should().Be("test.jpg");
        }

        [Fact]
        public void ShouldReturnFileNameFromNetworkPath()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(o => o.FileName).Returns("\\test\\testing\\test.jpg");

            var file = FileHelper.GetFileNameFromPath(mockFile.Object);

            file.Should().Be("test.jpg");
        }
    }
}
