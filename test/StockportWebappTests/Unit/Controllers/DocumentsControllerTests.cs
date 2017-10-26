using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using StockportWebapp.Controllers;
using StockportWebapp.Services;
using Xunit;

namespace StockportWebappTests.Unit.Controllers
{
    public class DocumentsControllerTests
    {
        [Fact]
        public void GetSecureDocumentsShouldCallDocumentsService()
        {
            // Arrange
            var mockDocumentsService = new Mock<IDocumentsService>();
            var documentsController = new DocumentsController(mockDocumentsService.Object);

            // Act
            documentsController.GetSecureDocument("stockportgov", "asset id", "group-slug");

            // Assert
            mockDocumentsService.Verify(o => o.GetSecureDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
