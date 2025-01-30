namespace StockportWebapp.ModelBinders;

[ExcludeFromCodeCoverage]
public class DateTimeFormatConverterModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType.Equals(typeof(DateTime)) || context.Metadata.ModelType.Equals(typeof(DateTime?)))
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

        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
        if (!valueProviderResult.Equals(ValueProviderResult.None))
        {
            DateTime.TryParse(valueProviderResult.FirstValue, out DateTime dateProvided);

            if (dateProvided > DateTime.MinValue)
                return dateProvided;
        }

        return null;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext is null)
            throw new ArgumentNullException(nameof(bindingContext));

        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (!valueProviderResult.Equals(ValueProviderResult.None))
        {
            DateTime.TryParse(valueProviderResult.FirstValue, out DateTime dateProvided);

            if (dateProvided > DateTime.MinValue)
            {
                bindingContext.Result = ModelBindingResult.Success(dateProvided);
                return Task.CompletedTask;
            }
        }

        return Task.CompletedTask;
    }
}