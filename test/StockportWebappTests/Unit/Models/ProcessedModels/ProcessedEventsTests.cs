namespace StockportWebappTests_Unit.Unit.Models.ProcessedModels;

public class ProcessedEventsTests
{
    private readonly ProcessedEvents processedEvent = new("title",
                                                        "slug",
                                                        "teaser",
                                                        "image.png",
                                                        "image.png",
                                                        "description",
                                                        "fee",
                                                        false,
                                                        "location",
                                                        "submittedBy",
                                                        DateTime.Now.AddDays(1),
                                                        "startTime",
                                                        "endTime",
                                                        new List<Crumb>(),
                                                        new MapDetails(),
                                                        "booking information",
                                                        null,
                                                        null,
                                                        string.Empty,
                                                        new(),
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        string.Empty,
                                                        null,
                                                        string.Empty,
                                                        string.Empty,
                                                        new(),
                                                        new List<CallToActionBanner>());

    [Fact]
    public void ShouldBeTrueIsAlertSunsetDateIsNotPassed()
    {
        // Act
        bool isAlertDisplayed = processedEvent.IsAlertDisplayed(new Alert("title",
                                                                        "body",
                                                                        "severity",
                                                                        DateTime.Now.AddDays(-5),
                                                                        DateTime.Now.AddDays(5),
                                                                        string.Empty,
                                                                        false,
                                                                        string.Empty));

        // Assert
        Assert.True(isAlertDisplayed);
    }

    [Fact]
    public void ShouldBeFalseIsAlertSunsetDateIsPassed()
    {
        // Act
        bool isAlertDisplayed = processedEvent.IsAlertDisplayed(new Alert("title",
                                                                        "body",
                                                                        "severity",
                                                                        DateTime.Now.AddDays(-5),
                                                                        DateTime.Now.AddDays(-1),
                                                                        string.Empty,
                                                                        false,
                                                                        string.Empty));

        // Assert
        Assert.False(isAlertDisplayed);                                                            
    }
}