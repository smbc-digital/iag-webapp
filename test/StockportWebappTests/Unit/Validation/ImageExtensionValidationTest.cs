namespace StockportWebappTests_Unit.Unit.Validation;

public class ImageExtensionValidationTest
{
    [Fact]
    public void ItShouldValidateDocumentsOfTypeJpg()
    {
        // Arrange
        ImageFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.jpg", "test.jpg"));

        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldValidateDocumentsOfTypeJpeg()
    {
        // Arrange
        ImageFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.jpeg", "test.jpeg"));

        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldValidateDocumentsOfTypePng()
    {
        // Arrange
        ImageFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.png", "test.png"));

        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldValidateDocumentsOfTypeGif()
    {
        // Arrange
        ImageFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.gif", "test.gif"));

        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldNotValidateDocumentsOfTypeExe()
    {
        // Arrange
        ImageFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.exe", "test.exe"));

        // Assert
        Assert.False(response);
    }

    [Fact]
    public void ItShouldNotValidateDocumentsOfTypeVbs()
    {
        // Arrange
        ImageFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.vbs", "test.vbs"));

        // Assert
        Assert.False(response);
    }
}