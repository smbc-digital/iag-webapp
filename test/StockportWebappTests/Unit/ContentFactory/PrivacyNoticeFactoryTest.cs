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

namespace StockportWebappTests_Unit.Unit.ContentFactory
{
    public class PrivacyNoticeFactoryTest
    {
        private readonly PrivacyNoticeFactory _factory;
        private readonly Mock<MarkdownWrapper> _markdownWrapper;

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

            var purpose = "test-purpose";
            _markdownWrapper.Setup(_ => _.ConvertToHtml(purpose)).Returns("test-purpose-html");

            var externallyShared = "test-externally-shared";
            _markdownWrapper.Setup(_ => _.ConvertToHtml(externallyShared)).Returns("test-externally-shared-html");

            var obtained = "test-obtained";
            _markdownWrapper.Setup(_ => _.ConvertToHtml(obtained)).Returns("test-obtained-html");

            var retentionPeriod = "test-retention";
            _markdownWrapper.Setup(_ => _.ConvertToHtml(retentionPeriod)).Returns("test-retention-html");

            var privacyNotice = new PrivacyNotice()
            {
                Slug = "test-slug",
                Title = "test-title",
                Category = "test-categories",
                Purpose = "test-purpose",
                TypeOfData = "test-type-of-data",
                Legislation = "test-legislation",
                Obtained = "test-obtained",
                ExternallyShared = "test-externally-shared",
                RetentionPeriod = "test-retention",
                OutsideEu = false,
                AutomatedDecision = false,
                UrlOne = "test-url-1",
                UrlTwo = "test-url-2",
                UrlThree = "test-url-3",
                Breadcrumbs = new List<Crumb>()
            };

            //Act
            var processedPrivacyNotice = _factory.Build(privacyNotice);

            //Assert
            processedPrivacyNotice.Slug.Should().Be("test-slug");
            processedPrivacyNotice.Title.Should().Be("test-title");
            processedPrivacyNotice.Category.Should().Be("test-categories");
            processedPrivacyNotice.Purpose.Should().Be("test-purpose-html");
            processedPrivacyNotice.TypeOfData.Should().Be("test-type-of-data-html");
            processedPrivacyNotice.Legislation.Should().Be("test-legislation");
            processedPrivacyNotice.Obtained.Should().Be("test-obtained-html");
            processedPrivacyNotice.ExternallyShared.Should().Be("test-externally-shared-html");
            processedPrivacyNotice.RetentionPeriod.Should().Be("test-retention-html");
            processedPrivacyNotice.OutsideEu.Should().Be(false);
            processedPrivacyNotice.AutomatedDecision.Should().Be(false);
            processedPrivacyNotice.UrlOne.Should().Be("test-url-1");
            processedPrivacyNotice.UrlTwo.Should().Be("test-url-2");
            processedPrivacyNotice.UrlThree.Should().Be("test-url-3");
        }
    }
}
