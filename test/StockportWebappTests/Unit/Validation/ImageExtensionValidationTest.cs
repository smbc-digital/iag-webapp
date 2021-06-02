using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using StockportWebapp.Validation;

namespace StockportWebappTests_Unit.Unit.Validation
{
    public class ImageExtensionValidationTest
    {
        [Fact]
        public void ItShouldValidateDocumentsOfTypeJpg()
        {
            var validator = new ImageFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.jpg", "test.jpg"));

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldValidateDocumentsOfTypeJpeg()
        {
            var validator = new ImageFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.jpeg", "test.jpeg"));

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldValidateDocumentsOfTypePng()
        {
            var validator = new ImageFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.png", "test.png"));

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldValidateDocumentsOfTypeGif()
        {
            var validator = new ImageFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.gif", "test.gif"));

            response.Should().BeTrue();
        }

        [Fact]
        public void ItShouldNotValidateDocumentsOfTypeExe()
        {
            var validator = new ImageFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.exe", "test.exe"));

            response.Should().BeFalse();
        }

        [Fact]
        public void ItShouldNotValidateDocumentsOfTypeVbs()
        {
            var validator = new ImageFileExtensionValidation();

            var response = validator.IsValid(new FormFile(null, 0, 0, "test.vbs", "test.vbs"));

            response.Should().BeFalse();
        }
    }
}
