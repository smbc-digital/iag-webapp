using FluentAssertions;
using Moq;
using StockportWebapp.ContentFactory;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StockportWebappTests.Unit.ContentFactory
{
    public class PrivacyNoticeFactoryTest
    {
        private readonly PrivacyNoticeFactory _factory;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;
        private readonly PrivacyNotice _privacyNotice;

        public PrivacyNoticeFactoryTest()
        {
            _markdownWrapper = new Mock<MarkdownWrapper>();
            _factory = new PrivacyNoticeFactory(_markdownWrapper.Object);
        }

        [Fact]
        public void Build_ShouldReturnAPrivacyNotice()
        {
            // Arrange
            var privacyNotice = new PrivacyNotice();
            // Act
            var processedPrivacyNotice = _factory.Build(privacyNotice);
            // Assert
            processedPrivacyNotice.Should().BeOfType<ProcessedPrivacyNotice>();
        }

        [Fact]
        public void Build_ShouldConvertPrivacyNoticeToProcessedPrivacyNotice()
        {
            // Arrange
            var typeOfData = "test-type-of-data";
            _markdownWrapper.Setup(_ => _.ConvertToHtml(typeOfData)).Returns("test-type-of-data-html");

            var legislation = "test-legislation";
            _markdownWrapper.Setup(_ => _.ConvertToHtml(legislation)).Returns("test-legislation-html");

            var externallyShared = "test-externally-shared";
            _markdownWrapper.Setup(_ => _.ConvertToHtml(externallyShared)).Returns("test-externally-shared-html");

            var privacyNotice = new PrivacyNotice()
            {
                Slug = "test-slug",
                Title = "test-title",
                Directorate = "test-directorate",
                ActivitiesAsset = "test-activities-asset",
                TransactionsActivity = "test-transactions-activity",
                Purpose = "test-purpose",
                TypeOfData = typeOfData,
                Legislation = legislation,
                Obtained = "test-obtained",
                ExternallyShared = externallyShared,
                RetentionPeriod = "test-retention-period",
                Conditions = "test-conditions",
                ConditionsSpecial = "test-conditions-special",
                UrlOne = "test-url-1",
                UrlTwo = "test-url-2",
                UrlThree = "test-url-3"
            };

            //Act
            var processedPrivacyNotice = _factory.Build(privacyNotice);

            //Assert
            processedPrivacyNotice.Slug.Should().Be("test-slug");
            processedPrivacyNotice.Title.Should().Be("test-title");
            processedPrivacyNotice.Directorate.Should().Be("test-directorate");
            processedPrivacyNotice.ActivitiesAsset.Should().Be("test-activities-asset");
            processedPrivacyNotice.TransactionsActivity.Should().Be("test-transactions-activity");
            processedPrivacyNotice.Purpose.Should().Be("test-purpose");
            processedPrivacyNotice.TypeOfData.Should().Be("test-type-of-data-html");
            processedPrivacyNotice.Legislation.Should().Be("test-legislation-html");
            processedPrivacyNotice.Obtained.Should().Be("test-obtained");
            processedPrivacyNotice.ExternallyShared.Should().Be("test-externally-shared-html");
            processedPrivacyNotice.RetentionPeriod.Should().Be("test-retention-period");
            processedPrivacyNotice.Conditions.Should().Be("test-conditions");
            processedPrivacyNotice.ConditionsSpecial.Should().Be("test-conditions-special");
            processedPrivacyNotice.UrlOne.Should().Be("test-url-1");
            processedPrivacyNotice.UrlTwo.Should().Be("test-url-2");
            processedPrivacyNotice.UrlThree.Should().Be("test-url-3");
        }
    }
}
