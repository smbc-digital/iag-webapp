namespace StockportWebappTests_Unit.Unit.Validation;

public class RequiredIfEmailValidationTests
{
    private const string ErrorMessage = "Invalid email address";
    
    [Theory]
    [InlineData("valid.email@example.com", "ServicePay", false)]
    [InlineData("invalid-email", "ServicePay", true)]
    [InlineData("", "ServicePay", true)]
    [InlineData(null, "ServicePay", true)]
    [InlineData("valid.email@example.com", "OtherType", false)]
    public void ShouldValidateEmailCorrectly(string email, string paymentType, bool shouldFail)
    {
        // Arrange
        RequiredIfEmailValidation validationAttribute = new(EPaymentSubmissionType.ServicePayPayment, ErrorMessage, "ServicePay");
        ProcessedPayment processedPayment = new()
        {
            PaymentType = paymentType
        };

        PaymentSubmission paymentSubmission = new() { Payment = payment };
        ValidationContext validationContext = new(paymentSubmission);

        // Act
        var result = validationAttribute.GetValidationResult(email, validationContext);

        // Assert
        if (shouldFail)
        {
            Assert.NotNull(result);
            Assert.Equal(ErrorMessage, result.ErrorMessage);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Fact]
    public void ShouldNotValidateIfNotServicePayPayment()
    {
        // Arrange
        var validationAttribute = new RequiredIfEmailValidation(EPaymentSubmissionType.OtherPaymentType, ErrorMessage, "ServicePay");
        var payment = new Payment { PaymentType = "ServicePay" };
        var paymentSubmission = new PaymentSubmission { Payment = payment };
        var validationContext = new ValidationContext(paymentSubmission);

        // Act
        var result = validationAttribute.GetValidationResult("invalid-email", validationContext);

        // Assert
        Assert.Null(result);
    }
}