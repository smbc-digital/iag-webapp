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
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace StockportWebappTests.Unit.Repositories
{
    public class DocumentsRepositoryTests
    {
        [Fact(Skip = "fix")]
        public async void GetSecureDocument_ShouldCallHttpClient()
        {
            // Arrange
            var httpClient = new Mock<IHttpClient>();
            var applicationConfiguraiton = new Mock<IApplicationConfiguration>();
            var simpleUrlGenerator = new Mock<IUrlGeneratorSimple<Document>>();
            var loggedInHelper = new Mock<ILoggedInHelper>();
            var logger = new Mock<ILogger<GenericRepository<Document>>>();
            var documentsRepository = new DocumentsRepository(httpClient.Object, applicationConfiguraiton.Object, simpleUrlGenerator.Object, loggedInHelper.Object, logger.Object);

            // Mock
            simpleUrlGenerator.Setup(o => o.BaseContentApiUrl()).Returns("url");
            loggedInHelper.Setup(o => o.GetLoggedInPerson()).Returns(new LoggedInPerson() { Email = "email" });

            // Act
            var document = await documentsRepository.GetSecureDocument("asset id", "group-slug");

            // Assert
            document.Should().NotBeNull();
        }
    }
}
