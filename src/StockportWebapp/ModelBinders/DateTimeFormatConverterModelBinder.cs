namespace StockportWebapp.ModelBinders;

[ExcludeFromCodeCoverage]
public class DateTimeFormatConverterModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
            return new DateTimeFormatConverterModelBinder();

        return null;
    }
}

[ExcludeFromCodeCoverage]
public class DateTimeFormatConverterModelBinder : IModelBinder
{
    public object BindModel(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
            throw new ArgumentNullException(nameof(bindingContext));

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
        if (valueProviderResult != ValueProviderResult.None)
        {
            DateTime.TryParse(valueProviderResult.FirstValue, out var dateProvided);

            if (dateProvided > DateTime.MinValue)
                return dateProvided;
        }

        return null;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
            throw new ArgumentNullException(nameof(bindingContext));

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult != ValueProviderResult.None)
        {
            DateTime.TryParse(valueProviderResult.FirstValue, out var dateProvided);

            if (dateProvided > DateTime.MinValue)
            {
                bindingContext.Result = ModelBindingResult.Success(dateProvided);
                return Task.CompletedTask;
            }
        }

        return Task.CompletedTask;
    }
}