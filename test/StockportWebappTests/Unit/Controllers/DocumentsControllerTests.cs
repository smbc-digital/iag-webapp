using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.Models;
using StockportWebapp.Services;
using StockportWebappTests_Unit.Builders;
using Xunit;

namespace StockportWebappTests_Unit.Unit.Controllers
{
    public class DocumentsControllerTests
    {
        [Fact]
        public async void GetSecureDocumentsShouldReturnDocument()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var documentToDownload = new DocumentToDownload() { MediaType = document.MediaType, FileData = new byte[] { } };
            var mockDocumentsService = new Mock<IDocumentsService>();
            var assetId = "asset-id";
            var slug = "slug";

            // Mock
            mockDocumentsService.Setup(o => o.GetSecureDocument(assetId, slug)).ReturnsAsync(documentToDownload);

            var documentsController = new DocumentsController(mockDocumentsService.Object);

            // Act
            var result = await documentsController.GetSecureDocument(slug, assetId) as FileContentResult;

            // Assert
            mockDocumentsService.Verify(o => o.GetSecureDocument(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            result.Should().NotBeNull();
            result.ContentType.Should().Be(document.MediaType);
        }
    }
}
