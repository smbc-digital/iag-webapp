namespace StockportWebapp.Models.Validation;

public class DocumentFileExtensionValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;

        // the field is optional, so this can be null
        if (file == null) return ValidationResult.Success;

        if (file.FileName.EndsWith(".docx") || file.FileName.EndsWith(".doc") || file.FileName.EndsWith(".pdf") || file.FileName.EndsWith(".odt"))
            return ValidationResult.Success;

        return new ValidationResult("Should be a docx, doc, pdf or odt file");
    }
}
