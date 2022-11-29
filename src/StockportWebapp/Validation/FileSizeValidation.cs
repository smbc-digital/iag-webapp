using System.ComponentModel.DataAnnotations;

namespace StockportWebapp.Validation
{
    public class FileSizeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null) return ValidationResult.Success;
            if (file.OpenReadStream().Length < 5242880) return ValidationResult.Success;

            return new ValidationResult("File needs to be under 5mb");
        }
    }
}
