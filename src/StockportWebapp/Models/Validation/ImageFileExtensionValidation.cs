namespace StockportWebapp.Models.Validation;

public class ImageFileExtensionValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        if (file == null) return ValidationResult.Success;

        if (file.FileName.ToLower().EndsWith(".jpg") || file.FileName.ToLower().EndsWith(".jpeg") || file.FileName.ToLower().EndsWith(".png") || file.FileName.ToLower().EndsWith(".gif"))
            return ValidationResult.Success;

        return new ValidationResult("Should be an png, jpg or gif file");
    }
}
