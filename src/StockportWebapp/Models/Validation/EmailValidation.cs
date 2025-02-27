﻿namespace StockportWebapp.Models.Validation;

public class EmailValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        Regex emailRegex = new(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
        ServicePayPaymentSubmissionViewModel paymentSubmission = validationContext.ObjectInstance as ServicePayPaymentSubmissionViewModel;

        if (string.IsNullOrEmpty(paymentSubmission?.EmailAddress))
            return new ValidationResult("The email address is required");

        return Regex.IsMatch(paymentSubmission.EmailAddress, emailRegex.ToString())
            ? ValidationResult.Success
            : new ValidationResult("Check the email address and try again");
    }
}