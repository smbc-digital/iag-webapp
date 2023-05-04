using StockportWebapp.Models.Validation;

namespace StockportWebappTests_Unit.Unit.Validation;

public class PastDateValidationTest
{
    private readonly PastDateValidation _pastDateValidation;

    public PastDateValidationTest()
    {
        _pastDateValidation = new PastDateValidation();
    }

    [Fact]
    public void ShouldReturnValidForPastDate()
    {
        var result = _pastDateValidation.IsValid(new DateTime(2016, 01, 01));

        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnInvalidForFutureDate()
    {
        var result = _pastDateValidation.IsValid(DateTime.MaxValue);

        result.Should().BeFalse();
    }
}
