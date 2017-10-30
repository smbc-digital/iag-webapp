using FluentAssertions;
using Moq;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using Xunit;
using Microsoft.Extensions.Logging;
using StockportWebappTests.Builders;
using System;

namespace StockportWebappTests.Unit.Services
{
    public class DocumentsServiceTests
    {

        [Fact]
        public async void GetSecureDocument_ShouldCallDocumentsRepository()
        {
            // Arrange
            var mockDocumentsRepository = new Mock<IDocumentsRepository>();
            var logger = new Mock<ILogger<DocumentsService>>();
            var document = new DocumentBuilder().Build();
            
            var documentsService = new DocumentsService(mockDocumentsRepository.Object, logger.Object);

            // Mock
            mockDocumentsRepository.Setup(o => o.GetSecureDocument("asset id", "group-slug")).ReturnsAsync(document);

            // Act
            var documentResponse = await documentsService.GetSecureDocument("asset id", "group-slug");

            // Assert
            mockDocumentsRepository.Verify(o => o.GetSecureDocument(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            documentResponse.Should().NotBeNull();
            documentResponse.ShouldBeEquivalentTo(document);
        }

        [Fact]
        public async void GetSecureDocument_ShouldLogIfThrowsException()
        {
            var mockDocumentsRepository = new Mock<IDocumentsRepository>();
            var logger = new Mock<ILogger<DocumentsService>>();
            var document = new DocumentBuilder().Build();

            var documentsService = new DocumentsService(mockDocumentsRepository.Object, logger.Object);

            // Mock
            mockDocumentsRepository.Setup(o => o.GetSecureDocument("asset id", "group-slug")).ThrowsAsync(new Exception("Error"));

            // Act
            var documentResponse = await documentsService.GetSecureDocument("asset id", "group-slug");

            // Assert
            LogTesting.Assert(logger, LogLevel.Error, "There was a problem getting document with assetId: asset id for group group-slug");
            documentResponse.Should().BeNull();
        }
    }
}
