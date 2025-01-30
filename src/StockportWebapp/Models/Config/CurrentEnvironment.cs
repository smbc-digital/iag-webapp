namespace StockportWebapp.Models.Config;

public class CurrentEnvironment(string name)
{
    public string Name { get; } = name;
}