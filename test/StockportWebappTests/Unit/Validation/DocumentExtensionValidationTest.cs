using FluentAssertions;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Validation;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Validation
{
    public class DocumentExtensionValidationTest
    {

        [Fact]
        public void ItShouldVaildateIfDocumentIsNull()
        {
            var validator = new DocumentFileExtensionValidation();

            var response = validator.IsValid(null);

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldValidateDocumentsOfTypeDoc()
        {
            var validator = new DocumentFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.doc", "test.doc"));

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldValidateDocumentsOfTypeDocx()
        {
            var validator = new DocumentFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.docx", "test.docx"));

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldValidateDocumentsOfTypePdf()
        {
            var validator = new DocumentFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.pdf", "test.pdf"));

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldValidateDocumentsOfTypeOdt()
        {
            var validator = new DocumentFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.odt", "test.odt"));

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldNotValidateDocumentsOfTypeExe()
        {
            var validator = new DocumentFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.exe", "test.exe"));

            response.Should().BeFalse();
        }

        [Fact]
        public void ItShouldNotValidateDocumentsOfTypeJpg()
        {
            var validator = new DocumentFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.jpg", "test.jpg"));

            response.Should().BeFalse();
        }
    }
}
