using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
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
        private readonly Mock<IHttpEmailClient> _emailClient = new Mock<IHttpEmailClient>();
        private readonly Mock<IApplicationConfiguration> _applicationConfiguration = new Mock<IApplicationConfiguration>();

        public GroupsServiceTests()
        {
            _service = new GroupsService(_mockRepository.Object, _emailClient.Object, _applicationConfiguration.Object);;
            _applicationConfiguration.Setup(_ => _.GetGroupArchiveEmail("stockportgov")).Returns(() => AppSetting.GetAppSetting("test"));
        }

        [Fact]
        public async void HandleArchivedGroupsShouldCallContentApiRepository()
        {
            // Arrange
            _mockRepository.Setup(_ => _.GetResponseWithBusinessId<List<Group>>(It.IsAny<string>())).ReturnsAsync(new List<Group> {new GroupBuilder().Build()});
            _applicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
                new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { Template = "", NumOfDays = 180 }, new ArchiveEmailPeriod { Template = "", NumOfDays = 270 } });
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
        public async void HandleArchivedGroups_ShouldEmailStagedGroupsAdministrators()
        {
            // Arrange
            _mockRepository.Setup(_ => _.GetResponseWithBusinessId<List<Group>>(It.IsAny<string>())).ReturnsAsync(new List<Group>
            {
                new GroupBuilder().Slug("recently-updated").DateLastModified(DateTime.Now.AddDays(-10)).Build(),
                new GroupBuilder().Slug("still-recently-updated").DateLastModified(DateTime.Now.AddDays(-180)).Build(),
                new GroupBuilder().Slug("updated-more-than-six-months-ago").DateLastModified(DateTime.Now.AddDays(-270)).Build()
            });

            _applicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
                new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { Template = "", NumOfDays = 180 }, new ArchiveEmailPeriod { Template = "", NumOfDays = 270 } });

            // Act
            await _service.HandleArchivedGroups();

            // Assert
            _emailClient.Verify(_ => _.SendEmailToService(It.IsAny<EmailMessage>()), Times.Exactly(2));
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
                            new GroupAdministratorItems { Email = "correct-recipient@thing.com", Permission = "A"}
                        }
                    })
                 .DateLastModified(DateTime.Now.AddDays(-180)).Build()
            });

            _emailClient.Setup(_ =>
                    _.GenerateEmailBodyFromHtml(It.IsAny<GroupAdministratorItems>(), ""))
                .Returns("body");

            _applicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
                new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { Template = "", NumOfDays = 180 }, new ArchiveEmailPeriod { Template = "", NumOfDays = 270 } });

            // Act
            await _service.HandleArchivedGroups();

            // Assert
            _emailClient.Verify(_ => _.SendEmailToService(It.Is<EmailMessage>(entity => entity.ToEmail == "correct-recipient@thing.com" && entity.Body == "body")), Times.Once());
        }

        [Fact]
        public async void HandleArchivedGroups_ShouldGetEmailPeriodsFromConfig()
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
                                new GroupAdministratorItems { Email = "correct-recipient@thing.com", Permission = "A"}
                            }
                        })
                    .DateLastModified(DateTime.Now.AddDays(-180)).Build()
            });

            _applicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
                new List<ArchiveEmailPeriod> {new ArchiveEmailPeriod {Template = "", NumOfDays = 0}});

            // Act
            await _service.HandleArchivedGroups();

            // Assert
            _applicationConfiguration.Verify(_ => _.GetArchiveEmailPeriods(), Times.Once);
        }

        [Fact]
        public async void HandleArchivedGroups_ShouldThrowExceptionWhenNoPeriodsReturned()
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
                                new GroupAdministratorItems { Email = "correct-recipient@thing.com", Permission = "A"}
                            }
                        })
                    .DateLastModified(DateTime.Now.AddDays(-180)).Build()
            });


            // Act Assert
            await Assert.ThrowsAsync<GroupsServiceException>(() => _service.HandleArchivedGroups());
        }
    }
}
