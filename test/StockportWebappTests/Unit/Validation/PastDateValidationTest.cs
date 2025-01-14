namespace StockportWebappTests_Unit.Unit.Validation;

public class PastDateValidationTest
{
    private readonly PastDateValidation _pastDateValidation = new();

    [Fact]
    public void ShouldReturnValidForPastDate()
    {
        // Act
        bool result = _pastDateValidation.IsValid(new DateTime(2016, 01, 01));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ShouldReturnInvalidForFutureDate()
    {
        // Act
        bool result = _pastDateValidation.IsValid(DateTime.MaxValue);
        
        // Assert
        Assert.False(result);
    }
}