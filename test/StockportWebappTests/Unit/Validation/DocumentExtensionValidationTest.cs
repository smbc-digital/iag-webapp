namespace StockportWebappTests_Unit.Unit.Validation;

public class DocumentExtensionValidationTest
{
    [Fact]
    public void ItShouldVaildateIfDocumentIsNull()
    {
        // Arrange
        DocumentFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(null);

        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldValidateDocumentsOfTypeDoc()
    {
        // Arrange
        DocumentFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.doc", "test.doc"));

        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldValidateDocumentsOfTypeDocx()
    {
        // Arrange
        DocumentFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.docx", "test.docx"));
        
        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldValidateDocumentsOfTypePdf()
    {
        // Arrange
        DocumentFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.pdf", "test.pdf"));

        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldValidateDocumentsOfTypeOdt()
    {
        // Arrange
        DocumentFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.odt", "test.odt"));

        // Assert
        Assert.True(response);
    }

    [Fact]
    public void ItShouldNotValidateDocumentsOfTypeExe()
    {
        // Arrange
        DocumentFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.exe", "test.exe"));
        
        // Assert
        Assert.False(response);
    }

    [Fact]
    public void ItShouldNotValidateDocumentsOfTypeJpg()
    {
        // Arrange
        DocumentFileExtensionValidation validator = new();

        // Act
        bool response = validator.IsValid(new FormFile(null, 0, 0, "test.jpg", "test.jpg"));

        // Assert
        Assert.False(response);
    }
}