using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;

namespace StockportWebappTests_Integration
{
    public static class LogTesting
    {
        public static void Assert<T>(Mock<ILogger<T>> loggerMock, LogLevel logLevel, string logMessage)
        {
            loggerMock.Verify(
                x =>
                    x.Log<object>(logLevel, (EventId)0, new FormattedLogValues(logMessage), It.IsAny<Exception>(),
                        It.IsAny<Func<object, Exception, string>>()), Times.AtLeastOnce);
        }

        public static void DoesNotContain<T>(Mock<ILogger<T>> loggerMock, LogLevel logLevel, string logMessage)
        {
            loggerMock.Verify(
                x =>
                    x.Log<object>(logLevel, (EventId)0, new FormattedLogValues(logMessage), (Exception)null,
                        It.IsAny<Func<object, Exception, string>>()), Times.Never);
        }
    }
}