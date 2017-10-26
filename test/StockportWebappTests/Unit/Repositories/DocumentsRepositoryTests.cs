using System.Collections.Generic;
using System.Net;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebappTests.Builders;
using Xunit;
using StockportWebapp.Models;

namespace StockportWebappTests.Unit.Repositories
{
    public class DocumentsRepositoryTests
    {
        [Fact]
        public async void GetSecureDocument_ShouldCallHttpClient()
        {
            // Arrange
            var repository = new Mock<IRepository>();
            var documentsRepository = new DocumentsRepository(repository.Object);

            // Mock
            repository.Setup(o => o.Get<Document>(It.IsAny<string>(), It.IsAny< List<Query>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, new DocumentBuilder().Build(), ""));

            // Act
            await documentsRepository.GetSecureDocument("asset id", "group-slug");

            // Assert
            repository.Verify(o => o.Get<Document>(It.IsAny<string>(), It.IsAny<List<Query>>()), Times.Once());
        }
    }
}
