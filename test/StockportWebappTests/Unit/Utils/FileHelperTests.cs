namespace StockportWebappTests_Unit.Unit.Utils;

public class FileHelperTests
{
    [Fact]
    public void ShouldReturnFileNameWhenNoDirectoryIsInThePath()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile
            .Setup(file => file.FileName)
            .Returns("test.jpg");

        // Act
        string file = FileHelper.GetFileNameFromPath(mockFile.Object);

        // Assert
        Assert.Equal("test.jpg", file);
    }

    [Fact]
    public void ShouldReturnEmptyStringForNoFileName()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile
            .Setup(file => file.FileName)
            .Returns(string.Empty);

        // Act
        string file = FileHelper.GetFileNameFromPath(mockFile.Object);

        // Assert
        Assert.Empty(file);
    }

    [Fact]
    public void ShouldReturnEmptyForNoFile()
    {
        // Act
        string file = FileHelper.GetFileNameFromPath(null);
        
        // Assert
        Assert.Empty(file);
    }

    [Fact]
    public void ShouldReturnFileNameForWindowsPath()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile
            .Setup(file => file.FileName)
            .Returns("C:\\test\\testing\\test.jpg");

        // Act
        string file = FileHelper.GetFileNameFromPath(mockFile.Object);

        // Assert
        Assert.Equal("test.jpg", file);
    }

    [Fact]
    public void ShouldReturnFileNameForLinuxPath()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile
            .Setup(file => file.FileName)
            .Returns("C://test//testing//test.jpg");

        // Act
        string file = FileHelper.GetFileNameFromPath(mockFile.Object);

        // Assert
        Assert.Equal("test.jpg", file);
    }

    [Fact]
    public void ShouldReturnFileNameFromNetworkPath()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile
            .Setup(file => file.FileName)
            .Returns("\\test\\testing\\test.jpg");

        // Act
        string file = FileHelper.GetFileNameFromPath(mockFile.Object);

        // Assert
        Assert.Equal("test.jpg", file);
    }
}