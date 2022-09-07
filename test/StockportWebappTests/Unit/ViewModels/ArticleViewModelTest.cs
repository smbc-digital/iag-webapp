using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.ViewModels;
using StockportWebappTests_Unit.Helpers;
using Xunit;

namespace StockportWebappTests_Unit.Unit.ViewModels
{
    public class ArticleViewModelTest
    {
        private readonly ProcessedSection _sectionOne;
        private readonly ProcessedSection _sectionTwo;
        private readonly ProcessedSection _sectionThree;

        public ArticleViewModelTest()
        {
            _sectionOne = BuildSection("test-slug");
            _sectionTwo = BuildSection("test-slug-section-two");
            _sectionThree = BuildSection("test-slug-section-three");
        }

        [Fact]
        public void SetsTheFirstSectionAsTheDisplayedSectionIfNoSectionSlugIsGiven()
        {
            var article = BuildArticle("", new List<ProcessedSection> {_sectionOne, _sectionTwo});

            var viewModel = new ArticleViewModel(article);

            viewModel.DisplayedSection.Should().Be(_sectionOne);
        }

        [Fact]
        public void ShowsArticleSummaryIfDisplayedSectionIsFirstSection()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionOne.Slug);

            viewModel.ShouldShowArticleSummary.Should().BeTrue();
        }

        [Fact]
        public void DoesNotShowArticleSummaryIfDisplayedSectionIsNotTheFirstSection()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionTwo.Slug);

            viewModel.ShouldShowArticleSummary.Should().BeFalse();
        }

        [Fact]
        public void ReturnsTheDisplayedSectionIndex()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionTwo.Slug);

            viewModel.DisplayedSectionIndex.Should().Be(2);
        }

        [Fact]
        public void ReturnsTheNextSectionIfTheDisplayedSectionIsNotTheLast()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionTwo.Slug);

            viewModel.NextSection().Should().Be(_sectionThree);
        }

        [Fact]
        public void ReturnsNullForNextSectionIfDisplayedSectionIsTheLast()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionThree.Slug);

            viewModel.NextSection().Should().BeNull();
        }

        [Fact]
        public void ShowsNextSectionButtonIfDisplayedSectionIsNotTheLast()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });
   
            var viewModel = new ArticleViewModel(article, _sectionTwo.Slug);

            viewModel.ShouldShowNextSectionButton().Should().BeTrue();
        }

        [Fact]
        public void DoesNotShowNextSectionButtonIfDisplayedSectionIsTheLast()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionThree.Slug);

            viewModel.ShouldShowNextSectionButton().Should().BeFalse();
        }

        [Fact]
        public void ReturnsThePreviousSectionIfTheDisplayedSectionIsNotTheFirst()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionTwo.Slug);

            viewModel.PreviousSection().Should().Be(_sectionOne);
        }

        [Fact]
        public void ReturnsNullForPreviousSectionIfDisplayedSectionIsTheFirst()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionOne.Slug);

            viewModel.PreviousSection().Should().BeNull();
        }

        [Fact]
        public void ShowsPreviousSectionButtonIfDisplayedSectionIsNotTheFirst()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionTwo.Slug);

            viewModel.ShouldShowPreviousSectionButton().Should().BeTrue();
        }

        [Fact]
        public void DoesNotShowPreviousSectionButtonIfDisplayedSectionIsTheFirst()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionOne.Slug);

            viewModel.ShouldShowPreviousSectionButton().Should().BeFalse();
        }

        [Fact]
        public void OgTitleShouldBeTitleAndDisplayedSection()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo, _sectionThree });

            var viewModel = new ArticleViewModel(article, _sectionOne.Slug);

            viewModel.OgTitleMetaData.Should().Be(viewModel.Article.Title + " - " + viewModel.DisplayedSection.Title);
        }

        [Fact]
        public void ShowsArticleWhenThereAreNoSections()
        {
            var article = BuildArticle("article-slug", new List<ProcessedSection> { });

            var viewModel = new ArticleViewModel(article);
            viewModel.Article.Sections.Should().BeNullOrEmpty();
            viewModel.DisplayedSection.Should().BeNull();
            viewModel.ShouldShowArticleSummary.Should().BeTrue();
            viewModel.OgTitleMetaData.Should().Be(viewModel.Article.Title);
        }

        [Fact]
        public void RaisesSectionDoesNotExistExceptionIfSectionSlugNotFound()
        {
            var article = BuildArticle("", new List<ProcessedSection> { _sectionOne, _sectionTwo });

            Action act = () => new ArticleViewModel(article, _sectionThree.Slug);

            act.Should().Throw<SectionDoesNotExistException>()
                .WithMessage($"Section with slug: {_sectionThree.Slug} does not exist");
        }

        [Fact]
        private void ShouldReturnTopicSubItemsListForSideBar()
        {
            var firstSubitem = new SubItem(TextHelper.AnyString, "first-subitem", TextHelper.AnyString, TextHelper.AnyString,
                TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var subItems = new List<SubItem> { firstSubitem };
            var firstSecondaryitem = new SubItem(TextHelper.AnyString, "first-secondaryitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var secondaryItems = new List<SubItem> { firstSecondaryitem };

            var topic = new Topic(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, subItems, secondaryItems,new List<SubItem>(), new List<Crumb>(), new List<Alert>(), false, TextHelper.AnyString, null, String.Empty, new List<ExpandingLinkBox>(), String.Empty, string.Empty, true,
                 new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty);
            var article = new ProcessedArticle(TextHelper.AnyString,TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<ProcessedSection>(),
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(), new List<Alert>(), topic, new List<Alert>(), null, new DateTime(), new bool());

            var articleViewModel = new ArticleViewModel(article);

            bool showMoreButton;
            var sidebarSubItems = articleViewModel.SidebarSubItems(out showMoreButton);

            sidebarSubItems.Count().Should().Be(2);
            sidebarSubItems.ToList()[0].Should().Be(firstSubitem);
            sidebarSubItems.ToList()[1].Should().Be(firstSecondaryitem);
            showMoreButton.Should().Be(false);
        }

        [Fact]
        private void ShouldReturnSixTopicsSubItemsForSideBar()
        {
            var firstSubItem = new SubItem(TextHelper.AnyString, "first-subitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var secondSubItem = new SubItem(TextHelper.AnyString, "second-subitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var thirdSubItem = new SubItem(TextHelper.AnyString, "third-subitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var fourthSubItem = new SubItem(TextHelper.AnyString, "fourth-subitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var fifthSubItem = new SubItem(TextHelper.AnyString, "fifth-subitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var sixthSubItem = new SubItem(TextHelper.AnyString, "sixth-subitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var seventhSubItem = new SubItem(TextHelper.AnyString, "seventh-subitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());
            var eightSubItem = new SubItem(TextHelper.AnyString, "eigth-subitem", TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<SubItem>());

            var subItems = new List<SubItem> { firstSubItem, secondSubItem, thirdSubItem, fourthSubItem };
            var secondaryItems = new List<SubItem> { fifthSubItem, sixthSubItem, seventhSubItem, eightSubItem };

            var topic = new Topic(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString,
                subItems, secondaryItems, new List<SubItem>(), new List<Crumb>(), new List<Alert>(), false, TextHelper.AnyString, null, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true,
                 new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty);
            var article = new ProcessedArticle(TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<ProcessedSection>(), TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(), new List<Alert>(), topic, new List<Alert>(), null, new DateTime(), new bool());

            var articleViewModel = new ArticleViewModel(article);

            bool showMoreButton;
            var sidebarSubItems = articleViewModel.SidebarSubItems(out showMoreButton);

            sidebarSubItems.Count().Should().Be(6);
            sidebarSubItems.ToList()[0].Should().Be(firstSubItem);
            sidebarSubItems.ToList()[1].Should().Be(secondSubItem);
            sidebarSubItems.ToList()[2].Should().Be(thirdSubItem);
            sidebarSubItems.ToList()[3].Should().Be(fourthSubItem);
            sidebarSubItems.ToList()[4].Should().Be(fifthSubItem);
            sidebarSubItems.ToList()[5].Should().Be(sixthSubItem);
            showMoreButton.Should().Be(true);
        }

        [Theory]
        [InlineData("test section meta", "test article meta", "test section meta")]
        [InlineData("test section meta", null, "test section meta")]
        [InlineData("", "test article meta", "test article meta")]
        [InlineData(null, "test article meta", "test article meta")]
        private void ShouldSetMetaDescription(string sectionMeta, string articleMeta, string expectedMeta)
        {
            // Arrange
            var sectionSlug = "test-slug";
            var section = new ProcessedSection(
                string.Empty,
                sectionSlug,
                sectionMeta,
                string.Empty,
                null,
                null,
                null
            );
            var article = new ProcessedArticle(
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                articleMeta,
                new List<ProcessedSection> { section },
                string.Empty,
                string.Empty,
                null,
                null,
                null,
                null,
                null,
                null,
                new DateTime(), 
                new bool()
            );

            // Act
            var model = new ArticleViewModel(article, sectionSlug);

            // Assert
            model.MetaDescription.Should().Be(expectedMeta);
        }

        private ProcessedArticle BuildArticle(string slug, List<ProcessedSection> sections)
        {

            var parentTopic = new Topic("Name", "slug", "Summary", "Teaser", "metaDescription", "Icon", "Image", "Image", null, null, null,
                new List<Crumb>(), null, true, "test-id", null, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, true,
                 new CarouselContent(string.Empty, string.Empty, string.Empty, string.Empty), string.Empty);

            return new ProcessedArticle(TextHelper.AnyString, slug, TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, sections,
                TextHelper.AnyString, TextHelper.AnyString, TextHelper.AnyString, new List<Crumb>(), new List<Alert>(), parentTopic, new List<Alert>(), null, new DateTime(), new bool());
        }


        private ProcessedSection BuildSection(string slug)
        {
            var profiles = new List<Profile>();
            var documents = new List<Document>();
            var alertsInline = new List<Alert>();

            return new ProcessedSection(TextHelper.AnyString, slug, TextHelper.AnyString, TextHelper.AnyString, profiles, documents, alertsInline);
        }
    }
}