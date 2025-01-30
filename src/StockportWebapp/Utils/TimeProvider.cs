namespace StockportWebapp.Utils;

public interface ITimeProvider
{
    DateTime Now();
    DateTime Today();
}

[ExcludeFromCodeCoverage]
public class TimeProvider : ITimeProvider
{
    public DateTime Now() =>
        DateTime.UtcNow;

    public DateTime Today() =>
        DateTime.Today;
}