namespace StockportWebapp.Models;

public sealed class EmbeddedPartial(string partialName, object model)
{
    public string PartialName { get; } = partialName;
    public object Model { get; } = model;
}