using System;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using StockportWebapp.Wrappers;
using StockportWebappTests_Unit.Builders;
using StockportWebappTests_Unit.Helpers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Services
{
    public class DocumentsServiceTests
    {

        [Fact]
        public async void GetSecureDocument_ShouldCallDocumentsRepository()
        {
            // Arrange
            var mockDocumentsRepository = new Mock<IDocumentsRepository>();
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var logger = new Mock<ILogger<DocumentsService>>();
            var document = new DocumentBuilder().Build();
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 })
            };

            var documentsService = new DocumentsService(mockDocumentsRepository.Object, mockHttpClientWrapper.Object, logger.Object);

            // Mock
            mockDocumentsRepository.Setup(o => o.GetSecureDocument("asset id", "group-slug")).ReturnsAsync(document);
            mockHttpClientWrapper.Setup(o => o.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

            // Act
            var documentResponse = await documentsService.GetSecureDocument("asset id", "group-slug");

            // Assert
            mockDocumentsRepository.Verify(o => o.GetSecureDocument(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            documentResponse.Should().NotBeNull();
            documentResponse.MediaType.Should().Be(document.MediaType);
            documentResponse.FileData.Should().BeAssignableTo(typeof(byte[]));
        }

        [Fact]
        public async void GetSecureDocument_ShouldLogIfThrowsException()
        {
            var mockDocumentsRepository = new Mock<IDocumentsRepository>();
            var mockHttpClientWrapper = new Mock<IHttpClientWrapper>();
            var logger = new Mock<ILogger<DocumentsService>>();
            var document = new DocumentBuilder().Build();

            var documentsService = new DocumentsService(mockDocumentsRepository.Object, mockHttpClientWrapper.Object, logger.Object);

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
