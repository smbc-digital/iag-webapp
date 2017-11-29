using StockportWebapp.Attributes;
using StockportWebapp.QuestionBuilder.Entities;
using StockportWebapp.QuestionBuilder.Validators.Entities;
using System;
using System.Linq;
using System.Reflection;

namespace StockportWebapp.QuestionBuilder.Validators
{
    public static class QuestionValidatorFactory
    {
        public static IQuestionValidator CreateQuestionValidator(IQuestion question, ValidatorData validatorData)
        {
            var validatorType = typeof(ValidatorBase).GetTypeInfo().Assembly
                .GetTypes().Where(t => typeof(ValidatorBase).IsAssignableFrom(t)).Select(t => new
                {
                    Type = t,
                    ValidatorType = t.GetTypeInfo().GetCustomAttributes<QuestionValidatorTypeAttribute>().FirstOrDefault(),
                    Value = validatorData
                })
                .FirstOrDefault(_ => _.ValidatorType != null && _.ValidatorType.Name == validatorData.Type);

            if (validatorType == null)
            {
                throw new ArgumentException(string.Format("Validator type ({0}) is not supported.", validatorData.Type));
            }
            return Activator.CreateInstance(validatorType.Type, question, validatorData.Message, validatorData.Value) as ValidatorBase;
        }
    }
}
