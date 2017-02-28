using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StockportWebapp.Models;
using StockportWebapp.Utils;

namespace StockportWebapp.Validation
{
    public class MaxNumberOfCategoriesValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categories = value as List<CategoryListItem>;

            if (categories == null)
                return ValidationResult.Success;

            var selectCount = categories.Count(c => c.Selected);
            if (selectCount < 1)
                return new ValidationResult("You should select at least one category");
            if (selectCount >3)
                return new ValidationResult("You should select no more than three categories");
            return ValidationResult.Success;
        }
    }
}
