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

namespace StockportWebappTests.Unit.Controllers
{
    public class DocumentsControllerTests
    {
        [Fact]
        public async void GetSecureDocumentsShouldReturnDocument()
        {
            // Arrange
            var document = new DocumentBuilder().Build();
            var mockDocumentsService = new Mock<IDocumentsService>();
            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new ByteArrayContent(new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 });

            // Mock
            mockDocumentsService.Setup(o => o.GetSecureDocument("asset id", "group-slug")).ReturnsAsync(document);
            httpClientWrapper.Setup(o => o.GetAsync(It.IsAny<string>())).ReturnsAsync(response);

            var documentsController = new DocumentsController(mockDocumentsService.Object, httpClientWrapper.Object);

            // Act
            var result = await documentsController.GetSecureDocument("asset id", "group-slug") as FileContentResult;

            // Assert
            mockDocumentsService.Verify(o => o.GetSecureDocument(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            httpClientWrapper.Verify(o => o.GetAsync($"https:{document.Url}"), Times.Once);
            result.Should().NotBeNull();
            result.ContentType.Should().Be(document.MediaType);
        }
    }
}
