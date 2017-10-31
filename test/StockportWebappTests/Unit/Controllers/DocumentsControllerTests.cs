using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.Services;
using Xunit;
using System.Net.Http;
using StockportWebapp.Wrappers;
using StockportWebappTests.Builders;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Controllers
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
