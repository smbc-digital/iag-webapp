namespace StockportWebappTests_Unit.Unit.Validation;

public class EmailValidationTest
{
    private readonly ProcessedServicePayPayment _processedPayment = new("title",
                                                                        "slug",
                                                                        "teaser",
                                                                        "description",
                                                                        "paymentDetailsText",
                                                                        "Reference Number",
                                                                        new List<Crumb>(),
                                                                        EPaymentReferenceValidation.None,
                                                                        "metaDescription",
                                                                        "returnUrl",
                                                                        "1233455",
                                                                        "40000000",
                                                                        "paymentDescription",
                                                                        new List<Alert>());

    [Fact]
    public void IsValid_ShouldReturnValidationResultSuccess()
    {
        // Arrange
        ServicePayPaymentSubmissionViewModel model = new()
        {
            Reference = "12346",
            Amount = "23.52",
            EmailAddress = "test@test.com",
            Name = "Test Name",
            Payment = _processedPayment
        };

        ValidationContext context = new(model);

        // Act
        bool result = Validator.TryValidateObject(model, context, new List<ValidationResult>(), true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ShouldReturnValidationResultError_If_EmailAddressDoesNotMatchRegex()
    {
        // Arrange
        ServicePayPaymentSubmissionViewModel model = new()
        {
            Reference = "12346",
            Amount = "23.52",
            EmailAddress = "test@t",
            Name = "Test Name",
            Payment = _processedPayment
        };

        ValidationContext context = new(model);

        // Act
        bool result = Validator.TryValidateObject(model, context, new List<ValidationResult>(), true);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ShouldReturnValidationResultError_If_EmailAddressEmpty()
    {
        // Arrange
        ServicePayPaymentSubmissionViewModel model = new()
        {
            Reference = "12346",
            Amount = "23.52",
            EmailAddress = string.Empty,
            Name = "Test Name",
            Payment = _processedPayment
        };

        ValidationContext context = new(model);

        // Act
        bool result = Validator.TryValidateObject(model, context, new List<ValidationResult>(), true);

        // Assert
        Assert.False(result);
    }
}