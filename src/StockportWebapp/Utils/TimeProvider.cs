namespace StockportWebapp.Utils;

public interface ITimeProvider
{
    DateTime Now();
    DateTime Today();
}

[ExcludeFromCodeCoverage]
public class TimeProvider : ITimeProvider
{
    public DateTime Now()
    {
        return DateTime.UtcNow;
    }
    public DateTime Today()
    {
        return DateTime.Today;
    }
}
