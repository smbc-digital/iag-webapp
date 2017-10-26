using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.Http;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Utils;
using StockportWebappTests.Builders;
using Xunit;

namespace StockportWebappTests.Unit.Repositories
{
    public class DocumentsRepositoryTests
    {
        [Fact]
        public async void GetSecureDocument_ShouldCallHttpClient()
        {
            // Arrange
            var mockHttpClient = new Mock<IHttpClient>();
            var mockConfig = new Mock<IApplicationConfiguration>();
            var documentsRepository = new DocumentsRepository(mockHttpClient.Object, mockConfig.Object, new UrlGenerator(mockConfig.Object, new BusinessId("test")));

            // Mock
            mockHttpClient.Setup(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync(new HttpResponse((int)HttpStatusCode.OK, new DocumentBuilder().Build(), ""));

            // Act
            await documentsRepository.GetSecureDocument("business id", "asset id", "group-slug");

            // Assert
            mockHttpClient.Verify(o => o.Get(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Once());
        }
    }
}
