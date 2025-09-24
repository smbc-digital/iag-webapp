namespace StockportWebappTests_Unit.Helpers;

public static class LogTesting
{
    public static void Assert<T>(Mock<ILogger<T>> loggerMock, LogLevel logLevel, string logMessage)
    {
        loggerMock.Verify(
            loggerMock =>
                loggerMock.Log(logLevel, (EventId)0, It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(),
                    (Func<object, Exception, string>)(Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce);
    }

    public static void DoesNotContain<T>(Mock<ILogger<T>> loggerMock, LogLevel logLevel, string logMessage)
    {
        loggerMock.Verify(
            loggerMock =>
                loggerMock.Log(logLevel, (EventId)0, It.IsAny<It.IsAnyType>(), null,
                    (Func<object, Exception, string>)(Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Never);
    }
}