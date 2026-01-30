namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EmbeddedPartial
{
    public string PartialName { get; set; }
    public ContentBlock Model { get; set; }

    public EmbeddedPartial(ContentBlock model)
    {
        if (string.IsNullOrWhiteSpace(model.ContentType))
            throw new InvalidOperationException("Cannot create EmbeddedPartial: ContentBlock.ContentType is null or empty");

        PartialName = model.ContentType;
        Model = model;
    }
}