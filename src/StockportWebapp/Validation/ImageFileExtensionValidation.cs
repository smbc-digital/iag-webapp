using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StockportWebapp.Validation
{
    public class ImageFileExtensionValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null) return ValidationResult.Success;

            if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".png") || file.FileName.EndsWith(".gif"))
                return ValidationResult.Success;
         
            return new ValidationResult("Should be an png, jpg or gif file");
        }
    }
}
