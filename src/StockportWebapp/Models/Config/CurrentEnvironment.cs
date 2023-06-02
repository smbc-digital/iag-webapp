namespace StockportWebapp.Models.Config;

public class CurrentEnvironment
{
    public string Name { get; }

    public CurrentEnvironment(string name)
    {
        Name = name;
    }
}
