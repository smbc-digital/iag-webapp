namespace StockportWebappTests_Unit.Unit.Validation;

public class PaymentReferenceValidationTests
{
    [Fact]
    public void Should_ReturnSuccess_WhenValidationIsNone()
    {
        // Arrange
        var paymentSubmission = new PaymentSubmission
        {
            Amount = 10.00m,
            Payment = new ProcessedPayment("paymentTitle",
                "paymentSlug",
                "paymentTeaser",
                "paymentDescription",
                "paymentDetailsText",
                "paymentReference",
                "fund",
                "glCodeCostCentreNumber",
                new List<Crumb>(),
                EPaymentReferenceValidation.None,
                "metaDescription",
                "returnUrl",
                "catalogueId",
                "accountReference",
                "paymentDescription",
                new List<Alert>()),
            Reference = "12345"
        };
        var context = new ValidationContext(paymentSubmission);
        var results = new List<ValidationResult>();

        // Act
        var result = Validator.TryValidateObject(paymentSubmission, context, results, true);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_ReturnSuccess_WhenValidationIsNone_ForServicePayPayment()
    {
        // Arrange
        var paymentSubmission = new ServicePayPaymentSubmissionViewModel
        {
            Amount = "10.00",
            Payment = new ProcessedServicePayPayment("paymentTitle",
                "paymentSlug",
                "paymentTeaser",
                "paymentDescription",
                "paymentDetailsText",
                "paymentReference",
                new List<Crumb>(),
                EPaymentReferenceValidation.None,
                "metaDescription",
                "returnUrl",
                "catalogueId",
                "accountReference",
                "paymentDescription",
                new List<Alert>(),
                "12"),
            Reference = "12345",
            Name = "name",
            EmailAddress = "test@email.com"
        };
        var context = new ValidationContext(paymentSubmission);
        var results = new List<ValidationResult>();

        // Act
        var result = Validator.TryValidateObject(paymentSubmission, context, results, true);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(EPaymentReferenceValidation.FPN, "12345")]
    [InlineData(EPaymentReferenceValidation.FPN4or5, "1234")]
    [InlineData(EPaymentReferenceValidation.FPN4or5, "12344")]
    [InlineData(EPaymentReferenceValidation.ParkingFine, "SM30414755")]
    [InlineData(EPaymentReferenceValidation.ParkingFine, "SM3086279A")]
    [InlineData(EPaymentReferenceValidation.BusLaneAndCamera, "SM80000200")]
    [InlineData(EPaymentReferenceValidation.BusLaneAndCamera, "SM8000020A")]
    [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234567")]
    [InlineData(EPaymentReferenceValidation.CameraCar, "SM6123456A")]
    [InlineData(EPaymentReferenceValidation.BusLane, "SM81234567")]
    [InlineData(EPaymentReferenceValidation.BusLane, "SM8123456A")]
<<<<<<< sbRegex
    [InlineData(EPaymentReferenceValidation.BRS, "BRS12123")]
    [InlineData(EPaymentReferenceValidation.BRS, "brs12223")]
=======
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB12123")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "sb12223")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB1222")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "Sb1222")]
>>>>>>> local
    public void Should_ReturnSuccess_ForValidReference(EPaymentReferenceValidation referenceValidation, string reference)
    {
        // Arrange
        var paymentSubmission = new PaymentSubmission
        {
            Amount = 10.00m,
            Payment = new ProcessedPayment("paymentTitle",
                "paymentSlug",
                "paymentTeaser",
                "paymentDescription",
                "paymentDetailsText",
                "paymentReference",
                "fund",
                "glCodeCostCentreNumber",
                new List<Crumb>(),
                referenceValidation,
                "metaDescription",
                "returnUrl",
                "catalogueId",
                "accountReference",
                "paymentDescription",
                new List<Alert>()),
            Reference = reference
        };
        var context = new ValidationContext(paymentSubmission);
        var results = new List<ValidationResult>();

        // Act
        var result = Validator.TryValidateObject(paymentSubmission, context, results, true);

        // Assert
        result.Should().BeTrue();
    }


    [Theory]
    [InlineData(EPaymentReferenceValidation.FPN, "12345")]
    [InlineData(EPaymentReferenceValidation.FPN4or5, "1234")]
    [InlineData(EPaymentReferenceValidation.FPN4or5, "12344")]
    [InlineData(EPaymentReferenceValidation.ParkingFine, "SM30414755")]
    [InlineData(EPaymentReferenceValidation.ParkingFine, "SM3086279A")]
    [InlineData(EPaymentReferenceValidation.BusLaneAndCamera, "SM80000200")]
    [InlineData(EPaymentReferenceValidation.BusLaneAndCamera, "SM8000020A")]
    [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234567")]
    [InlineData(EPaymentReferenceValidation.CameraCar, "SM6123456A")]
    [InlineData(EPaymentReferenceValidation.BusLane, "SM81234567")]
    [InlineData(EPaymentReferenceValidation.BusLane, "SM8123456A")]
<<<<<<< sbRegex
    [InlineData(EPaymentReferenceValidation.BRS, "BRS12123")]
    [InlineData(EPaymentReferenceValidation.BRS, "brs12223")]
=======
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB12123")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "sb12223")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB1222")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "Sb1222")]
>>>>>>> local
    public void Should_ReturnSuccess_ForValidReference_ForServicePayPayment(EPaymentReferenceValidation referenceValidation, string reference)
    {
        // Arrange
        var paymentSubmission = new ServicePayPaymentSubmissionViewModel
        {
            Amount = "10.00",
            Payment = new ProcessedServicePayPayment("paymentTitle",
               "paymentSlug",
               "paymentTeaser",
               "paymentDescription",
               "paymentDetailsText",
               "paymentReference",
               new List<Crumb>(),
               referenceValidation,
               "metaDescription",
               "returnUrl",
               "catalogueId",
               "accountReference",
               "paymentDescription",
               new List<Alert>(),
               "12"),
            Reference = reference,
            Name = "name",
            EmailAddress = "test@email.com"
        };
        var context = new ValidationContext(paymentSubmission);
        var results = new List<ValidationResult>();

        // Act
        var result = Validator.TryValidateObject(paymentSubmission, context, results, true);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(EPaymentReferenceValidation.FPN, "1234567")]
    [InlineData(EPaymentReferenceValidation.FPN, "NOTVALID")]
    [InlineData(EPaymentReferenceValidation.FPN4or5, "124")]
    [InlineData(EPaymentReferenceValidation.FPN4or5, "123444")]
    [InlineData(EPaymentReferenceValidation.ParkingFine, "ER30414755")]
    [InlineData(EPaymentReferenceValidation.BusLaneAndCamera, "SM800002")]
    [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234")]
    [InlineData(EPaymentReferenceValidation.BusLane, "SM81234")]
    [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234A")]
    [InlineData(EPaymentReferenceValidation.BusLane, "SM81234A")]
<<<<<<< sbRegex
    [InlineData(EPaymentReferenceValidation.BRS, "BRV12123")]
    [InlineData(EPaymentReferenceValidation.BRS, "brs12223a")]
=======
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB212")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "sb12223a")]
>>>>>>> local
    public void Should_ReturnFalse_ForInvalidReference(EPaymentReferenceValidation referenceValidation, string reference)
    {
        // Arrange
        var paymentSubmission = new PaymentSubmission
        {
            Amount = 10.00m,
            Payment = new ProcessedPayment("paymentTitle",
                "paymentSlug",
                "paymentTeaser",
                "paymentDescription",
                "paymentDetailsText",
                "paymentReference",
                "fund",
                "glCodeCostCentreNumber",
                new List<Crumb>(),
                referenceValidation,
                "metaDescription",
                "returnUrl",
                "catalogueId",
                "accountReference",
                "paymentDescription",
                new List<Alert>()),
            Reference = reference
        };
        var context = new ValidationContext(paymentSubmission);
        var results = new List<ValidationResult>();

        // Act
        var result = Validator.TryValidateObject(paymentSubmission, context, results, true);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(EPaymentReferenceValidation.FPN, "1234567")]
    [InlineData(EPaymentReferenceValidation.FPN, "NOTVALID")]
    [InlineData(EPaymentReferenceValidation.FPN4or5, "123554")]
    [InlineData(EPaymentReferenceValidation.FPN4or5, "123")]
    [InlineData(EPaymentReferenceValidation.ParkingFine, "ER30414755")]
    [InlineData(EPaymentReferenceValidation.BusLaneAndCamera, "SM800002")]
    [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234")]
    [InlineData(EPaymentReferenceValidation.BusLane, "SM81234")]
    [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234A")]
    [InlineData(EPaymentReferenceValidation.BusLane, "SM81234A")]
<<<<<<< sbRegex
    [InlineData(EPaymentReferenceValidation.BRS, "BRS1012")]
    [InlineData(EPaymentReferenceValidation.BRS, "brv12223")]
=======
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB101")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "sb122263")]
>>>>>>> local
    public void Should_ReturnFalse_ForInvalidReference_ForServicePayPayment(EPaymentReferenceValidation referenceValidation, string reference)
    {
        // Arrange
        var paymentSubmission = new ServicePayPaymentSubmissionViewModel
        {
            Amount = "10.00",
            Payment = new ProcessedServicePayPayment("paymentTitle",
                "paymentSlug",
                "paymentTeaser",
                "paymentDescription",
                "paymentDetailsText",
                "paymentReference",
                new List<Crumb>(),
                referenceValidation,
                "metaDescription",
                "returnUrl",
                "catalogueId",
                "accountReference",
                "paymentDescription",
                new List<Alert>(),
                "12"),
            Reference = reference,
            Name = "name",
            EmailAddress = "test@email.com"
        };
        var context = new ValidationContext(paymentSubmission);
        var results = new List<ValidationResult>();

        // Act
        var result = Validator.TryValidateObject(paymentSubmission, context, results, true);

        // Assert
        result.Should().BeFalse();
    }
}
