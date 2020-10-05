using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using StockportWebapp.Enums;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.ViewModels;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Validation
{
    public class EmailValidationTest
    {
        private readonly ProcessedServicePayPayment _processedPayment = new ProcessedServicePayPayment("title", "slug", "teaser", "description", "paymentDetailsText",
            "Reference Number", new List<Crumb>(), EPaymentReferenceValidation.None,
            "metaDescription", "returnUrl", "1233455", "40000000", "paymentDescription", new List<Alert>(), "20.65");

        [Fact]
        public void IsValidShouldReturnValidationResultSuccess()
        {
            var model = new ServicePayPaymentSubmissionViewModel
            {
                Reference = "12346",
                Amount = 23.52m,
                EmailAddress = "test@test.com",
                Name = "Test Name",
                Payment = _processedPayment
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            var result = Validator.TryValidateObject(model, context, results, true);

            result.Should().Be(true);
        }

        [Fact]
        public void IsValidShouldReturnValidationResultErrorIfEmailAddressDoesNotMatchRegex()
        {
            var model = new ServicePayPaymentSubmissionViewModel
            {
                Reference = "12346",
                Amount = 23.52m,
                EmailAddress = "test@t",
                Name = "Test Name",
                Payment = _processedPayment
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            var result = Validator.TryValidateObject(model, context, results, true);

            result.Should().Be(false);
        }

        [Fact]
        public void IsValidShouldReturnValidationResultErrorIfEmailAddressEmpty()
        {
            var model = new ServicePayPaymentSubmissionViewModel
            {
                Reference = "12346",
                Amount = 23.52m,
                EmailAddress = string.Empty,
                Name = "Test Name",
                Payment = _processedPayment
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            var result = Validator.TryValidateObject(model, context, results, true);

            result.Should().Be(false);
        }
    }
}
