using FluentAssertions;
using Moq;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using Xunit;
using Microsoft.Extensions.Logging;

namespace StockportWebappTests.Unit.Services
{
    public class DocumentsServiceTests
    {

        [Fact(Skip = "fix")]
        public async void GetSecureDocument_ShouldCallDocumentsRepository()
        {
            // Arrange
            var mockDocumentsRepository = new Mock<IDocumentsRepository>();
            var logger = new Mock<ILogger<DocumentsService>>();
            
            var documentsService = new DocumentsService(mockDocumentsRepository.Object, logger.Object);

            // Act
            var document = await documentsService.GetSecureDocument("asset id", "group-slug");

            // Assert
            mockDocumentsRepository.Verify(o => o.GetSecureDocument(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            document.Should().NotBeNull();
        }
    }
}
