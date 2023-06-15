using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StockportWebapp.ModelBinders
{
    public class DateTimeFormatConverterModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
            {

                return new DateTimeFormatConverterModelBinder();
            }

            return null;
        }
    }

    public class DateTimeFormatConverterModelBinder : IModelBinder
    {

        public object BindModel(ModelBindingContext bindingContext)
        {

            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
            if (valueProviderResult != ValueProviderResult.None)
            {
                DateTime.TryParse(valueProviderResult.FirstValue, out var DateProvided);

                if (DateProvided > DateTime.MinValue)
                    return DateProvided;
            }

            return null;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                DateTime.TryParse(valueProviderResult.FirstValue, out var DateProvided);

                if (DateProvided > DateTime.MinValue)
                {
                    bindingContext.Result = ModelBindingResult.Success(DateProvided);
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }
    }
}