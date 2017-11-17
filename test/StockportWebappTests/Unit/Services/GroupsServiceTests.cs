using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using StockportWebapp.Entities;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.Repositories;
using StockportWebapp.Services;
using StockportWebapp.Utils;
using StockportWebappTests.Builders;
using Xunit;

namespace StockportWebappTests.Unit.Services
{
    public class GroupsServiceTests
    {
        private readonly Mock<IContentApiRepository> _mockRepository = new Mock<IContentApiRepository>();
        private readonly GroupsService _service;
        private readonly Mock<IEmailHandler> _emailHandler = new Mock<IEmailHandler>();

        public GroupsServiceTests()
        {
            _service = new GroupsService(_mockRepository.Object, _emailHandler.Object);;
        }

        [Fact]
        public async void HandleArchivedGroupsShouldCallContentApiRepository()
        {
            // Arrange

            // Act
            await _service.HandleArchivedGroups();

            // Assert
            _mockRepository.Verify(_ => _.GetResponseWithBusinessId<List<Group>>(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void IfNoGroupsReturned_HandleArchivedGroups_ShouldThrowException()
        {
            // Arrange
            _mockRepository.Setup(_ => _.GetResponseWithBusinessId<List<Group>>(It.IsAny<string>()))
                .ReturnsAsync(new List<Group>());

         // Assert
            await Assert.ThrowsAsync<GroupsServiceException>(() => _service.HandleArchivedGroups());
        }

        [Fact]
        public async void HandleArchivedGroups_ShouldFilterStageOneGroups()
        {
            // Arrange
            _mockRepository.Setup(_ => _.GetResponseWithBusinessId<List<Group>>(It.IsAny<string>())).ReturnsAsync(new List<Group>
            {
                new GroupBuilder().Slug("recently-updated").DateLastModified(DateTime.Now.AddDays(-10)).Build(),
                new GroupBuilder().Slug("still-recently-updated").DateLastModified(DateTime.Now.AddDays(-20)).Build(),
                new GroupBuilder().Slug("updated-more-than-six-months-ago").DateLastModified(DateTime.Now.AddDays(-180)).Build()
            });

            // Act
            var result = await _service.HandleArchivedGroups();

            // Assert
            result.ToList().Count.Should().Be(1);
        }

        [Fact]
        public async void HandleArchivedGroups_ShouldEmailStageOneGroupsAdministrators()
        {
            // Arrange
            _mockRepository.Setup(_ => _.GetResponseWithBusinessId<List<Group>>(It.IsAny<string>())).ReturnsAsync(new List<Group>
            {
                new GroupBuilder().Slug("recently-updated")..DateLastModified(DateTime.Now.AddDays(-10)).Build(),
                new GroupBuilder().Slug("still-recently-updated").DateLastModified(DateTime.Now.AddDays(-180)).Build(),
                new GroupBuilder().Slug("updated-more-than-six-months-ago").DateLastModified(DateTime.Now.AddDays(-180)).Build()
            });            


            
            // Act
            await _service.HandleArchivedGroups();

            // Assert
            _emailHandler.Verify(_ => _.SendEmail(It.IsAny<EmailEntity>()), Times.Exactly(2));
        }

        [Fact]
        public async void HandleArchivedGroups_ShouldBuildEmailEntities()
        {
            // Arrange
            _mockRepository.Setup(_ => _.GetResponseWithBusinessId<List<Group>>(It.IsAny<string>())).ReturnsAsync(new List<Group>
            {
                new GroupBuilder().Slug("recently-updated").DateLastModified(DateTime.Now.AddDays(-10)).Build(),
                new GroupBuilder().Slug("still-recently-updated").DateLastModified(DateTime.Now.AddDays(-20)).Build(),
                new GroupBuilder().Slug("updated-more-than-six-months-ago")
                .GroupAdministrators(
                    new GroupAdministrators
                    {
                        Items = new List<GroupAdministratorItems>
                        {
                            new GroupAdministratorItems { Email = "correct-recipient@thing.com" }
                        }
                    })
                 .DateLastModified(DateTime.Now.AddDays(-180)).Build()
            });

            _emailHandler.Setup(_ =>
                    _.GenerateEmailBodyFromHtml(It.IsAny<GroupAdministratorItems>(), "ArchiveGroupScheduler"))
                .Returns("body");

            // Act
            await _service.HandleArchivedGroups();

            // Assert
            _emailHandler.Verify(_ => _.SendEmail(It.Is<EmailEntity>(entity => entity.Recipient == "correct-recipient@thing.com" && entity.Body == "body")), Times.Once());
        }
    }
}
