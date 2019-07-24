using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using StockportWebapp.Enums;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Validation
{
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
                    "parisReference",
                    "fund",
                    "glCodeCostCentreNumber",
                    new List<Crumb>(),
                    EPaymentReferenceValidation.None),
                Reference = "12345"
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
        [InlineData(EPaymentReferenceValidation.ParkingFine, "SM30414755")]
        [InlineData(EPaymentReferenceValidation.BusLaneAndCamera, "SM80000200")]
        [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234567")]
        [InlineData(EPaymentReferenceValidation.BusLane, "SM81234567")]
        [InlineData(EPaymentReferenceValidation.CameraCar, "SM6123456A")]
        [InlineData(EPaymentReferenceValidation.BusLane, "SM8123456A")]
        public void Should_ReturnSuccess(EPaymentReferenceValidation referenceValidation, string reference)
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
                    "parisReference",
                    "fund",
                    "glCodeCostCentreNumber",
                    new List<Crumb>(),
                    referenceValidation),
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
        [InlineData(EPaymentReferenceValidation.FPN, "1234567")]
        [InlineData(EPaymentReferenceValidation.FPN, "NOTVALID")]
        [InlineData(EPaymentReferenceValidation.ParkingFine, "ER30414755")]
        [InlineData(EPaymentReferenceValidation.BusLaneAndCamera, "SM800002")]
        [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234")]
        [InlineData(EPaymentReferenceValidation.BusLane, "SM81234")]
        [InlineData(EPaymentReferenceValidation.CameraCar, "SM61234A")]
        [InlineData(EPaymentReferenceValidation.BusLane, "SM81234A")]
        public void Should_ReturnFalse(EPaymentReferenceValidation referenceValidation, string reference)
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
                    "parisReference",
                    "fund",
                    "glCodeCostCentreNumber",
                    new List<Crumb>(),
                    referenceValidation),
                Reference = reference
            };
            var context = new ValidationContext(paymentSubmission);
            var results = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(paymentSubmission, context, results, true);

            // Assert
            result.Should().BeFalse();
        }
    }
}
