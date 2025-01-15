namespace StockportWebappTests_Unit.Unit.Validation;

public class FileSizeValidationTest
{
    [Fact]
    public void ItShouldValidateIfTheFileSizeIsUnder5Mb()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile
            .Setup(file => file.OpenReadStream().Length)
            .Returns(5242879);
        
        mockFile
            .Setup(file => file.FileName)
            .Returns("filename");

        FileSizeValidation fileSizeValidation = new();

        // Act
        bool result = fileSizeValidation.IsValid(mockFile);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ItShouldNotValidateIfTheFileSizeIsOver5Mb()
    {
        // Arrange
        Mock<IFormFile> mockFile = new();
        mockFile
            .Setup(file => file.OpenReadStream().Length)
            .Returns(5242881);
        
        mockFile
            .Setup(file => file.FileName)
            .Returns("filename");

        FileSizeValidation fileSizeValidation = new();

        // Act
        bool result = fileSizeValidation.IsValid(mockFile.Object);

        // Assert
        Assert.False(result);
    }
}