using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;

namespace StockportWebappTests_Integration
{
    public static class LogTesting
    {
        public static void Assert<T>(Mock<ILogger<T>> loggerMock, LogLevel logLevel, string logMessage)
        {
            loggerMock.Verify(
                x =>
                    x.Log<object>(logLevel, (EventId)0, It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(),
                        (Func<object, Exception, string>)(Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce);
        }

        public static void DoesNotContain<T>(Mock<ILogger<T>> loggerMock, LogLevel logLevel, string logMessage)
        {
            loggerMock.Verify(
                x =>
                    x.Log<object>(logLevel, (EventId)0, It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(),
                        (Func<object, Exception, string>)(Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.AtLeastOnce);
        }
    }
}