namespace StockportWebapp.Models.Validation;

public class FileSizeValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        IFormFile file = value as IFormFile;

        if (file is null)
            return ValidationResult.Success;
            
        if (file.OpenReadStream().Length < 5242880)
            return ValidationResult.Success;

        return new ValidationResult("File needs to be under 5mb");
    }
}