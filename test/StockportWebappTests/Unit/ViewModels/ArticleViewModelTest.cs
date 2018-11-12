using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using StockportWebapp.Exceptions;
using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
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

            act.Should().Throw<SectionDoesNotExistException>()
                .WithMessage($"Section with slug: {_sectionThree.Slug} does not exist");
        }

        [Fact]
        private void ShouldReturnTopicSubItemsListForSideBar()
        {
            var firstSubitem = new SubItem(Helper.AnyString, "first-subitem", Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var subItems = new List<SubItem> { firstSubitem };
            var firstSecondaryitem = new SubItem(Helper.AnyString, "first-secondaryitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var secondaryItems = new List<SubItem> { firstSecondaryitem };

            var advertisement = new Advertisement(string.Empty, string.Empty, string.Empty, DateTime.MinValue,
                DateTime.MinValue, true, "image-url", string.Empty);

            var topic = new Topic(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, subItems, secondaryItems,new List<SubItem>(), new List<Crumb>(), new List<Alert>(), false, Helper.AnyString, null, String.Empty, new List<ExpandingLinkBox>(), String.Empty, string.Empty, advertisement);
            var article = new ProcessedArticle(Helper.AnyString,Helper.AnyString, Helper.AnyString, Helper.AnyString,new List<ProcessedSection>(),
                Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>(), new List<Alert>(), topic, false, new NullLiveChat(), new List<Alert>(), advertisement, null);

            var articleViewModel = new ArticleViewModel(article);

            bool showMoreButton;
            var sidebarSubItems = articleViewModel.SidebarSubItems(out showMoreButton);

            sidebarSubItems.Count().Should().Be(2);
            sidebarSubItems.ToList()[0].Should().Be(firstSubitem);
            sidebarSubItems.ToList()[1].Should().Be(firstSecondaryitem);
            showMoreButton.Should().Be(false);
        }

        [Fact]
        private void ShouldReturnTopicWithAdvertisement()
        {
            var firstSubitem = new SubItem(Helper.AnyString, "first-subitem", Helper.AnyString, Helper.AnyString,
                Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var subItems = new List<SubItem> { firstSubitem };
            var firstSecondaryitem = new SubItem(Helper.AnyString, "first-secondaryitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var secondaryItems = new List<SubItem> { firstSecondaryitem };

            var advertisement = new Advertisement("ad-title", "ad-slug", "ad-teaser", DateTime.MinValue,
                DateTime.MinValue, true, "image-url", string.Empty);

            var topic = new Topic(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, subItems, secondaryItems, new List<SubItem>(), new List<Crumb>(), new List<Alert>(), false, Helper.AnyString, null, String.Empty, new List<ExpandingLinkBox>(), String.Empty, string.Empty, advertisement);
            var article = new ProcessedArticle(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<ProcessedSection>(),
                Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>(), new List<Alert>(), topic, false, new NullLiveChat(), new List<Alert>(), advertisement, null);

            var articleViewModel = new ArticleViewModel(article);

            articleViewModel.Article.Advertisement.Isadvertisement.Should().Be(true);
            articleViewModel.Article.Advertisement.NavigationUrl.Should().Be("image-url");
            articleViewModel.Article.Advertisement.Title.Should().Be("ad-title");
            articleViewModel.Article.Advertisement.Slug.Should().Be("ad-slug");
            articleViewModel.Article.Advertisement.Teaser.Should().Be("ad-teaser");
        }

        [Fact]
        private void ShouldReturnSixTopicsSubItemsForSideBar()
        {
            var firstSubItem = new SubItem(Helper.AnyString, "first-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var secondSubItem = new SubItem(Helper.AnyString, "second-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var thirdSubItem = new SubItem(Helper.AnyString, "third-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var fourthSubItem = new SubItem(Helper.AnyString, "fourth-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var fifthSubItem = new SubItem(Helper.AnyString, "fifth-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var sixthSubItem = new SubItem(Helper.AnyString, "sixth-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var seventhSubItem = new SubItem(Helper.AnyString, "seventh-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());
            var eightSubItem = new SubItem(Helper.AnyString, "eigth-subitem", Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<SubItem>());

            var subItems = new List<SubItem> { firstSubItem, secondSubItem, thirdSubItem, fourthSubItem };
            var secondaryItems = new List<SubItem> { fifthSubItem, sixthSubItem, seventhSubItem, eightSubItem };

            var advertisement = new Advertisement(string.Empty, string.Empty, string.Empty, DateTime.MinValue,
                DateTime.MinValue, false, string.Empty, string.Empty);

            var topic = new Topic(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString,
                subItems, secondaryItems, new List<SubItem>(), new List<Crumb>(), new List<Alert>(), false, Helper.AnyString, null, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, advertisement);
            var article = new ProcessedArticle(Helper.AnyString, Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<ProcessedSection>(), Helper.AnyString,
                Helper.AnyString, Helper.AnyString, new List<Crumb>(), new List<Alert>(), topic, false, new NullLiveChat(), new List<Alert>(), new NullAdvertisement(), null);

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

            var advertisement = new Advertisement(string.Empty, string.Empty, string.Empty, DateTime.MinValue,
                DateTime.MinValue, false, string.Empty, string.Empty);

            var parentTopic = new Topic("Name", "slug", "Summary", "Teaser", "Icon", "Image", "Image", null, null, null,
                new List<Crumb>(), null, true, "test-id", null, "expandingLinkText", new List<ExpandingLinkBox>(), string.Empty, string.Empty, advertisement);
            return new ProcessedArticle(Helper.AnyString, slug, Helper.AnyString, Helper.AnyString, sections,
                Helper.AnyString, Helper.AnyString, Helper.AnyString, new List<Crumb>(), new List<Alert>(), parentTopic, false, new NullLiveChat(), new List<Alert>(), new NullAdvertisement(), null);
        }


        private ProcessedSection BuildSection(string slug)
        {
            var profiles = new List<Profile>();
            var documents = new List<Document>();
            var alertsInline = new List<Alert>();

            return new ProcessedSection(Helper.AnyString, slug, Helper.AnyString, profiles, documents, alertsInline);
        }
    }
}