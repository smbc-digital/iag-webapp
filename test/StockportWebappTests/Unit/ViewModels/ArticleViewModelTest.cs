using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.ViewModels;
using Xunit;
using Helper = StockportWebappTests.TestHelper;
using static StockportWebapp.Models.LiveChat;

namespace StockportWebappTests.Unit.ViewModels
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

            act.ShouldThrow<SectionDoesNotExistException>()
                .WithMessage($"Section with slug: {_sectionThree.Slug} does not exist");
        }

        [Fact]
        private void ShouldReturnTopicSubItemsListForSideBar()
        {
            var firstSubitem = new SubItem(Helper.AnyString, "first-subitem", Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString);
            var subItems = new List<SubItem> { firstSubitem };
            var firstSecondaryitem = new SubItem(Helper.AnyString, "first-secondaryitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);
            var secondaryItems = new List<SubItem> { firstSecondaryitem };

            var topic = new Topic(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, subItems, secondaryItems,new List<SubItem>(), new List<Crumb>(), new List<Alert>(), false, Helper.AnyString);
            var article = new ProcessedArticle(Helper.AnyString,Helper.AnyString, Helper.AnyString, Helper.AnyString,new List<ProcessedSection>(),
                Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>(), new List<Alert>(), topic, false, new NullLiveChat());

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
            var firstSubItem = new SubItem(Helper.AnyString, "first-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);
            var secondSubItem = new SubItem(Helper.AnyString, "second-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);
            var thirdSubItem = new SubItem(Helper.AnyString, "third-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);
            var fourthSubItem = new SubItem(Helper.AnyString, "fourth-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);
            var fifthSubItem = new SubItem(Helper.AnyString, "fifth-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);
            var sixthSubItem = new SubItem(Helper.AnyString, "sixth-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);
            var seventhSubItem = new SubItem(Helper.AnyString, "seventh-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);
            var eightSubItem = new SubItem(Helper.AnyString, "eigth-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString);

            var subItems = new List<SubItem> { firstSubItem, secondSubItem, thirdSubItem, fourthSubItem };
            var secondaryItems = new List<SubItem> { fifthSubItem, sixthSubItem, seventhSubItem, eightSubItem };

            var topic = new Topic(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                subItems, secondaryItems, new List<SubItem>(), new List<Crumb>(), new List<Alert>(), false, Helper.AnyString);
            var article = new ProcessedArticle(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<ProcessedSection>(), Helper.AnyString,
                Helper.AnyString, Helper.AnyString, new List<Crumb>(), new List<Alert>(), topic, false, new NullLiveChat());

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

        private ProcessedArticle BuildArticle(string slug, List<ProcessedSection> sections)
        {
            var parentTopic = new Topic("Name", "slug", "Summary", "Teaser", "Icon", "Image", "Image", null, null, null,
                new List<Crumb>(), null, true, "test-id");
            return new ProcessedArticle(Helper.AnyString, slug, Helper.AnyString, Helper.AnyString, sections,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>(), new List<Alert>(), parentTopic, false, new NullLiveChat());
        }


        private ProcessedSection BuildSection(string slug)
        {
            var profiles = new List<Profile>();
            var documents = new List<Document>();

            return new ProcessedSection(Helper.AnyString, slug, Helper.AnyString, profiles, documents);
        }
    }
}