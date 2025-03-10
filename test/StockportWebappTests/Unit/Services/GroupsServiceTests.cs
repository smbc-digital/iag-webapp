﻿namespace StockportWebappTests_Unit.Unit.Services;

public class GroupsServiceTests
{
    private readonly Mock<IContentApiRepository> _mockContentApiRepository = new Mock<IContentApiRepository>();
    private readonly Mock<IProcessedContentRepository> _mockProcessedContentRepository = new Mock<IProcessedContentRepository>();
    private readonly Mock<IStockportApiRepository> _mockStockportApiRepository = new Mock<IStockportApiRepository>();
    private readonly GroupsService _service;
    private readonly Mock<IHttpEmailClient> _mockEmailClient = new Mock<IHttpEmailClient>();
    private readonly Mock<IApplicationConfiguration> _mockApplicationConfiguration = new Mock<IApplicationConfiguration>();
    private readonly Mock<ILogger<GroupsService>> _mockLogger = new Mock<ILogger<GroupsService>>();
    private const int MaxNumberOfItemsPerPage = 9;
    private readonly BusinessId businessId = new BusinessId();

    public GroupsServiceTests()
    {
        _service = new GroupsService(_mockContentApiRepository.Object, _mockProcessedContentRepository.Object, _mockEmailClient.Object, _mockApplicationConfiguration.Object, _mockLogger.Object, _mockStockportApiRepository.Object, businessId);
        _mockApplicationConfiguration.Setup(_ => _.GetEmailEmailFrom("stockportgov")).Returns(() => AppSetting.GetAppSetting("test"));
    }

    [Fact]
    public async Task HandleStaleGroupsShouldCallContentApiRepository()
    {
        // Arrange
        _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>()).ReturnsAsync(new List<Group> { new GroupBuilder().Build() });
        _mockApplicationConfiguration.Setup(_ => _.GetArchiveEmailPeriods()).Returns(
            new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { Template = "", NumOfDays = 180 }, new ArchiveEmailPeriod { Template = "", NumOfDays = 270 } });
        // Act
        await _service.HandleStaleGroups();

        // Assert
        _mockStockportApiRepository.Verify(_ => _.GetResponse<List<Group>>(), Times.Once);
    }

    [Fact]
    public async Task IfNoGroupsReturned_HandleStaleGroups_ShouldThrowException()
    {
        // Arrange
        _mockStockportApiRepository.Setup(_ => _.GetResponse<List<Group>>(It.IsAny<string>()))
            .ReturnsAsync(new List<Group>());

        // Assert
        await Assert.ThrowsAsync<GroupsServiceException>(() => _service.HandleStaleGroups());
    }

    [Fact]
    public async Task HandleStaleGroups_ShouldEmailStagedGroupsAdministrators()
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
    public async Task HandleStaleGroups_ShouldBuildEmailEntities()
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
    public async Task HandleStaleGroups_ShouldGetEmailPeriodsFromConfig()
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
            new List<ArchiveEmailPeriod> { new ArchiveEmailPeriod { Template = "", NumOfDays = 0 } });

        // Act
        await _service.HandleStaleGroups();

        // Assert
        _mockApplicationConfiguration.Verify(_ => _.GetArchiveEmailPeriods(), Times.Once);
    }

    [Fact]
    public async Task HandleStaleGroups_ShouldThrowExceptionWhenNoPeriodsReturned()
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
    public async Task HandleStaleGroups_ShouldArchiveGroups_InLastPeriod()
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

    [Fact]
    public void ShouldReturnEmptyPaginationForNoGroups()
    {
        // Arrange
        var groupResults = new GroupResults()
        {
            Groups = new List<Group>()
        };

        // Act
        _service.DoPagination(groupResults, 0, 0);

        // Assert
        groupResults.Pagination.Should().BeEquivalentTo(new Pagination());
    }

    [Theory]
    [InlineData(1, 1, 1, 1)]
    [InlineData(2, 1, 2, 1)]
    [InlineData(MaxNumberOfItemsPerPage, 1, MaxNumberOfItemsPerPage, 1)]
    [InlineData(MaxNumberOfItemsPerPage * 3, 1, MaxNumberOfItemsPerPage, 3)]
    [InlineData(MaxNumberOfItemsPerPage + 1, 2, 1, 2)]
    public void PaginationShouldResultInCorrectNumItemsOnPageAndCorrectNumPages(
        int totalNumItems,
        int requestedPageNumber,
        int expectedNumItemsOnPage,
        int expectedNumPages)
    {
        var groupResults = new GroupResults();
        var groups = new List<Group>();

        for (var i = 0; i < totalNumItems; i++)
        {
            groups.Add(new GroupBuilder().Build());
        }

        groupResults.Groups = groups;

        _service.DoPagination(groupResults, requestedPageNumber, MaxNumberOfItemsPerPage);

        groupResults.Groups.Count.Should().Be(expectedNumItemsOnPage);
        groupResults.Pagination.TotalPages.Should().Be(expectedNumPages);
    }

    [Theory]
    [InlineData(0, 50, 1)]
    [InlineData(5, MaxNumberOfItemsPerPage * 3, 3)]
    public void IfSpecifiedPageNumIsImpossibleThenActualPageNumWillBeAdjustedAccordingly(
        int specifiedPageNumber,
        int numItems,
        int expectedPageNumber)
    {
        var groupResults = new GroupResults();
        var groups = new List<Group>();

        for (var i = 0; i < numItems; i++)
        {
            groups.Add(new GroupBuilder().Build());
        }

        groupResults.Groups = groups;

        _service.DoPagination(groupResults, specifiedPageNumber, MaxNumberOfItemsPerPage);

        groupResults.Pagination.CurrentPageNumber.Should().Be(expectedPageNumber);
    }
}
