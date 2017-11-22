using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StockportWebapp.Config;
using StockportWebapp.AmazonSES;
using StockportWebapp.Config;
using StockportWebapp.Entities;
using StockportWebapp.Exceptions;
using StockportWebapp.Http;
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
        private readonly Mock<IContentApiRepository> _mockContentApiRepository = new Mock<IContentApiRepository>();
        private readonly Mock<IStockportApiRepository> _mockStockportApiRepository = new Mock<IStockportApiRepository>();
        private readonly GroupsService _service;
        private readonly Mock<IHttpEmailClient> _mockEmailClient = new Mock<IHttpEmailClient>();
        private readonly Mock<IApplicationConfiguration> _mockApplicationConfiguration = new Mock<IApplicationConfiguration>();
        private readonly Mock<ILogger<GroupsService>> _mockLogger = new Mock<ILogger<GroupsService>>();

        public GroupsServiceTests()
        {
            _service = new GroupsService(_mockContentApiRepository.Object, _mockEmailClient.Object, _mockApplicationConfiguration.Object, _mockLogger.Object, _mockStockportApiRepository.Object);
            _mockApplicationConfiguration.Setup(_ => _.GetGroupArchiveEmail("stockportgov")).Returns(() => AppSetting.GetAppSetting("test"));
        }

        [Fact]
        public async void HandleStaleGroupsShouldCallContentApiRepository()
        {
            // Arrange
            _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>()).ReturnsAsync(new List<Group> {new GroupBuilder().Build()});
            _mockApplicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
                new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { Template = "", NumOfDays = 180 }, new ArchiveEmailPeriod { Template = "", NumOfDays = 270 } });
            // Act
            await _service.HandleStaleGroups();

            // Assert
            _mockStockportApiRepository.Verify(_ => _.GetResponse<List<Group>>(), Times.Once);
        }

        [Fact]
        public async void IfNoGroupsReturned_HandleStaleGroups_ShouldThrowException()
        {
            // Arrange
            _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>(It.IsAny<string>()))
                .ReturnsAsync(new List<Group>());

            // Assert
            await Assert.ThrowsAsync<GroupsServiceException>(() => _service.HandleStaleGroups());
        }

        [Fact]
        public async void HandleStaleGroups_ShouldEmailStagedGroupsAdministrators()
        {
            // Arrange
            _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>()).ReturnsAsync(new List<Group>
            {
                new GroupBuilder().Slug("recently-updated").DateLastModified(DateTime.Now.AddDays(-10)).Build(),
                new GroupBuilder().Slug("still-recently-updated").DateLastModified(DateTime.Now.AddDays(-180)).Build(),
                new GroupBuilder().Slug("updated-more-than-six-months-ago").DateLastModified(DateTime.Now.AddDays(-170)).Build()
            });

            _mockApplicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
                new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { Template = "", NumOfDays = 180 }, new ArchiveEmailPeriod { Template = "", NumOfDays = 270 } });

            // Act
            await _service.HandleStaleGroups();

            // Assert
            _mockEmailClient.Verify(_ => _.SendEmailToService(It.IsAny<EmailMessage>()), Times.Exactly(1));
        }

        [Fact]
        public async void HandleStaleGroups_ShouldBuildEmailEntities()
        {
            // Arrange
            _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>()).ReturnsAsync(new List<Group>
            {
                new GroupBuilder().Slug("recently-updated").DateLastModified(DateTime.Now.AddDays(-10)).Build(),
                new GroupBuilder().Slug("still-recently-updated").DateLastModified(DateTime.Now.AddDays(-20)).Build(),
                new GroupBuilder().Slug("updated-more-than-six-months-ago").GroupAdministrators(
                    new GroupAdministrators
                    {
                        Items = new List<GroupAdministratorItems>
                        {
                            new GroupAdministratorItems { Email = "correct-recipient@thing.com", Permission = "A"}
                        }
                    })
                 .DateLastModified(DateTime.Now.AddDays(-180)).Build()
            });

            _mockEmailClient.Setup(_ =>
                    _.GenerateEmailBodyFromHtml(It.IsAny<GroupAdministratorItems>(), It.IsAny<string>()))
                .Returns("body");

            _mockApplicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
                new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { Template = "", NumOfDays = 180 }, new ArchiveEmailPeriod { Template = "", NumOfDays = 270 } });

            // Act
            await _service.HandleStaleGroups();

            // Assert
            _mockEmailClient.Verify(_ => _.SendEmailToService(It.Is<EmailMessage>(entity => entity.ToEmail == "correct-recipient@thing.com")), Times.Once());
        }

        [Fact]
        public async void HandleStaleGroups_ShouldGetEmailPeriodsFromConfig()
        {
            // Arrange
            _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>()).ReturnsAsync(new List<Group>
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

            _mockApplicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
                new List<ArchiveEmailPeriod> {new ArchiveEmailPeriod {Template = "", NumOfDays = 0}});

            // Act
            await _service.HandleStaleGroups();

            // Assert
            _mockApplicationConfiguration.Verify(_ => _.GetArchiveEmailPeriods(), Times.Once);
        }

        [Fact]
        public async void HandleStaleGroups_ShouldThrowExceptionWhenNoPeriodsReturned()
        {
            // Arrange
            _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>()).ReturnsAsync(new List<Group>
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
            await Assert.ThrowsAsync<GroupsServiceException>(() => _service.HandleStaleGroups());
        }

        [Fact]
        public async void HandleStaleGroups_ShouldArchiveGroups_InLastPeriod()
        {
            // Arrange
            _mockApplicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(new List<ArchiveEmailPeriod>()
            {
                new ArchiveEmailPeriod()
                {
                    NumOfDays = 10,
                    Subject = "subject",
                    Template = "template"
                }
            });

            _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>()).ReturnsAsync(new List<Group>
            {
                new GroupBuilder().Slug("recently-updated").DateLastModified(DateTime.Now.AddDays(-1)).Build(),
                new GroupBuilder().Slug("still-recently-updated").DateLastModified(DateTime.Now.AddDays(-2)).Build(),
                new GroupBuilder().Slug("should-be-archived")
                    .GroupAdministrators(
                        new GroupAdministrators
                        {
                            Items = new List<GroupAdministratorItems>
                            {
                                new GroupAdministratorItems { Email = "correct-recipient@thing.com", Permission = "A"}
                            }
                        })
                    .DateLastModified(DateTime.Now.AddDays(-10)).Build()
            });

            _mockStockportApiRepository.Setup(_ => _.PutResponse<Group>(It.IsAny<HttpContent>(), It.IsAny<string>())).ReturnsAsync(HttpStatusCode.OK);

            _mockEmailClient.Setup(_ =>
                    _.GenerateEmailBodyFromHtml(It.IsAny<GroupAdministratorItems>(), It.IsAny<string>()))
                .Returns("body");

            // Act
            await _service.HandleStaleGroups();

            // Assert
            _mockStockportApiRepository.Verify(_ => _.PutResponse<Group>(It.IsAny<HttpContent>(), "should-be-archived"), Times.Once);

        }
    }
}
