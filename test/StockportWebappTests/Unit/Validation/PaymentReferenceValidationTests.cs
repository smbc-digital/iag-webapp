namespace StockportWebappTests_Unit.Unit.Validation;

public class PaymentReferenceValidationTests
{
    [Fact]
    public void Should_ReturnSuccess_WhenValidationIsNone()
    {
        // Arrange
        PaymentSubmission paymentSubmission = new()
        {
            Amount = "10.00",
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

        ValidationContext context = new(paymentSubmission);

        // Act
        bool result = Validator.TryValidateObject(paymentSubmission, context, new List<ValidationResult>(), true);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Should_ReturnSuccess_WhenValidationIsNone_ForServicePayPayment()
    {
        // Arrange
        ServicePayPaymentSubmissionViewModel paymentSubmission = new()
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
                new List<Alert>()),
            Reference = "12345",
            Name = "name",
            EmailAddress = "test@email.com"
        };

        ValidationContext context = new(paymentSubmission);

        // Act
        bool result = Validator.TryValidateObject(paymentSubmission, context, new List<ValidationResult>(), true);

        // Assert
        Assert.True(result);
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
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB12123")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "sb12223")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB1222")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "Sb1222")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "ABC\\12345")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "AB0\\12345")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "PS/123456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "ps/358456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "ps/sk13pj")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pS/sk1 3pj")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pS/anything")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "DC/123456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "dc/456789")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "ENQ/000456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "enq/112366")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T12123")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T0011")]
    public void Should_ReturnSuccess_ForValidReference(EPaymentReferenceValidation referenceValidation, string reference)
    {
        // Arrange
        PaymentSubmission paymentSubmission = new()
        {
            Amount = "10.00",
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

        ValidationContext context = new(paymentSubmission);

        // Act
        bool result = Validator.TryValidateObject(paymentSubmission, context, new List<ValidationResult>(), true);

        // Assert
        Assert.True(result);
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
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB12123")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "sb12223")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB1222")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "Sb1222")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "ABC\\12345")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "AB0\\12345")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "PS/123456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "ps/358456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "ps/sk13pj")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pS/sk1 3pj")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pS/anything")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "DC/123456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "dc/456789")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "ENQ/000456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "enq/112366")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T12123")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T1234")]
    public void Should_ReturnSuccess_ForValidReference_ForServicePayPayment(EPaymentReferenceValidation referenceValidation, string reference)
    {
        // Arrange
        ServicePayPaymentSubmissionViewModel paymentSubmission = new()
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
               new List<Alert>()),
            Reference = reference,
            Name = "name",
            EmailAddress = "test@email.com"
        };

        ValidationContext context = new(paymentSubmission);

        // Act
        bool result = Validator.TryValidateObject(paymentSubmission, context, new List<ValidationResult>(), true);

        // Assert
        Assert.True(result);
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
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB212")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "sb12223a")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "ABC\\123456")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "AB\\12345")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "123\\12345")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "A1B\\12345")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "AB0\\A2345")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "/anything")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pS123456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pS/")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pSanything000")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "DC/12345")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "dc/4567892")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "ENQ/00045")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "enq/1123664")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "tT123")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T2345A")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T234")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T234446")]
    public void Should_ReturnFalse_ForInvalidReference(EPaymentReferenceValidation referenceValidation, string reference)
    {
        // Arrange
        PaymentSubmission paymentSubmission = new()
        {
            Amount = "10.00",
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

        ValidationContext context = new(paymentSubmission);

        // Act
        bool result = Validator.TryValidateObject(paymentSubmission, context, new List<ValidationResult>(), true);

        // Assert
        Assert.False(result);
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
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "SB101")]
    [InlineData(EPaymentReferenceValidation.StockportBereavementInvoice, "sb122263")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "ABC\\123456")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "AB\\12345")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "123\\12345")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "A1B\\12345")]
    [InlineData(EPaymentReferenceValidation.ParkingPermit, "AB0\\A2345")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "/anything")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pS123456")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pS/")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "pSanything000")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "DC/12345")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "dc/4567892")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "ENQ/00045")]
    [InlineData(EPaymentReferenceValidation.PlanningApplication, "enq/1123664")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "t12345")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "t1234")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T012356")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T012")]
    [InlineData(EPaymentReferenceValidation.StockportEstatesInvoices, "T0005t")]
    
    public void Should_ReturnFalse_ForInvalidReference_ForServicePayPayment(EPaymentReferenceValidation referenceValidation, string reference)
    {
        // Arrange
        ServicePayPaymentSubmissionViewModel paymentSubmission = new()
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
                new List<Alert>()),
            Reference = reference,
            Name = "name",
            EmailAddress = "test@email.com"
        };

        ValidationContext context = new(paymentSubmission);

        // Act
        bool result = Validator.TryValidateObject(paymentSubmission, context, new List<ValidationResult>(), true);

        // Assert
        Assert.False(result);
    }
}