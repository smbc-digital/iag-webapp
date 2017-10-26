using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using Xunit;

namespace StockportWebappTests.Unit.Services
{
    public class DocumentsServiceTests
    {

        [Fact]
        public void GetSecureDocument_ShouldCallDocumentsRepository()
        {
            // Arrange
            var mockDocumentsRepository = new Mock<IDocumentsRepository>();
            var documentsService = new DocumentsService(mockDocumentsRepository.Object);

            // Act
            documentsService.GetSecureDocument("business id", "asset id", "group-slug");

            // Assert
            mockDocumentsRepository.Verify(o => o.GetSecureDocument(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }

        
    }
}
