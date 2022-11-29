using FluentAssertions;
using Moq;
using StockportWebapp.Validation;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Validation
{
    public class FileSizeValidationTest
    {
        [Fact]
        public void ItShouldValidateIfTheFileSizeIsUnder5Mb()
        {
            var file = new Mock<IFormFile>();
            file.Setup(o => o.OpenReadStream().Length).Returns(5242879);
            file.Setup(o => o.FileName).Returns("filename");

            var fileSizeValidation = new FileSizeValidation();
            var result = fileSizeValidation.IsValid(file);

            result.Should().Be(true);
        }

        [Fact]
        public void ItShouldNotValidateIfTheFileSizeIsOver5Mb()
        {
            var file = new Mock<IFormFile>();
            file.Setup(o => o.OpenReadStream().Length).Returns(5242881);
            file.Setup(o => o.FileName).Returns("filename");

            var fileSizeValidation = new FileSizeValidation();
            var result = fileSizeValidation.IsValid(file.Object);

            result.Should().Be(false);
        }
    }
}
