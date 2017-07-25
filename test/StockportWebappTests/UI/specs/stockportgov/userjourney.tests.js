module.exports = {
    before: function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        homepage.navigate();
        browser.maximizeWindow();
        browser.setCookie({
            name: "int_jwtCookie",
            value: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6InVpQHRlc3Rlc3Rlc3Rlc3QuY29tIiwiTmFtZSI6IlVJIFRlc3QifQ.ykkUVEm9qT0tGEali9dab6m_xwTztsQ5ztGpLlIhqT0"
        });
        browser.setCookie({
            name: "jwtCookie",
            value: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6InVpQHRlc3Rlc3Rlc3Rlc3QuY29tIiwiTmFtZSI6IlVJIFRlc3QifQ.ykkUVEm9qT0tGEali9dab6m_xwTztsQ5ztGpLlIhqT0"
        });
    },

    'Find article for About the Hat Works': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        // homepage.closeCookieBanner(browser);
        homepage.goToTopicListBlockPage(browser, "UITEST: Hat Works");

        var topicpage = browser.page.stockportgov.topicpage();
        topicpage.assertTitleIsVisible('UITEST: Hat Works');
        topicpage.assertSecondaryItemIsVisible(browser, "UITEST: Secondary Item");

        topicpage.goToTopicListBlockPage(browser, "UITEST: About the Hat Works", "uitest-about-the-hat-works");

        topicpage.assertTitleIsVisible("UITEST: About the Hat Works");

        var articlepage = browser.page.stockportgov.articlepage();
        articlepage.assertTitleIsVisible('UITEST: About the Hat Works');
        articlepage.assertLiveChatIsVisible(browser);

        articlepage.assertCarouselIsVisible();
        articlepage.assertCarouselImagesAreVisible();

        articlepage.goToNextSection(browser);
        articlepage.goToStartPage(browser, 'uitest-council-tax-reduction-form');

        var startpage = browser.page.stockportgov.startpage();
        startpage.assertStartButtonIsVisible();

        var breadcrumb = browser.page.stockportgov.breadcrumb();
        breadcrumb.goToHome(browser);

        homepage.searchForBins(browser);
    },

    'Find an article from the top tasks, which has a video, a table, and a profile and viewing the profile': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        homepage.navigate();
        homepage.goToTopTasksBlockPage(browser, "UITEST: Article with profile, video and table", "uitest-testarticle");

        var articlepage = browser.page.stockportgov.articlepage();
        articlepage.assertTitleIsVisible("UITEST: Article with profile, video and table");
        articlepage.assertButoVideoIsVisible();
        articlepage.assertTableIsVisible();
        articlepage.assertProfileIsVisible("Graduate Development Officer");
        articlepage.goToProfile(browser, "uitest-kirsten");

        var profilepage = browser.page.stockportgov.profilepage();
        profilepage.assertTitleIsVisible("UITEST: Kirsten");
    },

    'Find an article which has a document': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        homepage.navigate();
        homepage.goToTopTasksBlockPage(browser, "UITEST: Article with profile, video and table", "uitest-testarticle");

        var articlepage = browser.page.stockportgov.articlepage();
        articlepage.goToNextSection(browser);
        articlepage.assertDocumentIsVisible();
    },

    'Find all homepage elements': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        homepage.navigate();
        homepage.checkForWebCastImage(browser);
        homepage.assertNewsBannerIsVisible(browser);
        homepage.assertEventsBannerIsVisible(browser);
        homepage.assertEmailAlertsIsVisible(browser, "Subscribe");
        homepage.assertAtoZListIsVisible(browser);
    },

    'Search for news story and Find All NewsPage Elements': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        homepage.navigate();
        homepage.goToNewsroom(browser);

        var newsroom = browser.page.stockportgov.newsroom();
        newsroom.assertTitleIsVisible("News");
        newsroom.goToNewsWithTitle(browser, "UITEST: Steel frame starts to go up at Redrock", "/news/uitest-steel-frame-starts-to-go-up-at-redrock");

        var news = browser.page.stockportgov.news();
        news.assertTitleIsVisible("UITEST: Steel frame starts to go up at Redrock");

        news.assertNewsSideBarVisible(browser, "Latest News");
        news.assertTimestampPresent(browser);
        news.assertNewsSharePresent(browser);
        news.assertNewsTagIsVisible("UITEST");

        news.assertDocumentIsVisible();
    },

    'Visit a news article via category, then check category and date links': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        browser.maximizeWindow();
        homepage.navigate();
        homepage.goToNewsroom(browser);

        var newsroom = browser.page.stockportgov.newsroom();
        newsroom.assertTitleIsVisible("News");

        var activeCategory = "Elections";
        newsroom.goToCategory(browser, activeCategory);
        newsroom.assertAllCategoriesAreVisible(browser);
        newsroom.assertLinkIsActive(browser, activeCategory);
        newsroom.assertLinkHasCorrectUrl(browser, activeCategory, '/news?Category=' + activeCategory + "&Page=1");

        var allCategories = "All categories";
        newsroom.assertLinkIsNotActive(browser, allCategories);
        newsroom.assertLinkHasCorrectUrl(browser, allCategories, '/news');
    },

    'Search for Event story and Find All EventsPage Elements': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        homepage.navigate();
        homepage.goToEventsCalendar(browser);

        var eventcalendar = browser.page.stockportgov.eventscalendar();
        eventcalendar.assertTitleIsVisible("What's on in Stockport");
        eventcalendar.goToFirstEvent(browser);

        var events = browser.page.stockportgov.events();
        events.assertEventHasTitle(browser);

        events.assertHeadingStampPresent();
    },

    'Navigate through AtoZ list to check Article listed under Both Title starting letter, & synonym letter': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        homepage.navigate();
        homepage.goToAtoZList(browser, 'u');
        var atozpage = browser.page.stockportgov.atozpage();
        atozpage.assertTitle(browser, "U");
        atozpage.assertArticlelistedVisible(browser, "UITEST: About the Hat Works");
        atozpage.goToLetter(browser, "a");
        atozpage.assertTitle(browser, "A");
        atozpage.assertArticlelistedVisible(browser, "About the Hat Works");
    },

    'Navigate to a Contact Us form and enter invalid data into the form': function (browser) {
        var homepage = browser.page.stockportgov.homepage();
        homepage.navigate();
        homepage.goToTopTasksBlockPage(browser, "UITEST: Article with Section for Contact Us form", "uitest-article-with-section-for-contact-us-form");
        var articlepage = browser.page.stockportgov.articlepage();
        articlepage.assertContactFormIsVisible(browser);
        articlepage.assertTitleIsHiddenFieldOnContactForm(browser);
        articlepage.assertTitleOfArticleIsInTitleFieldOfContactFormForSection(browser);
        articlepage.submitContactUsForm(browser);
        articlepage.assertValidationMessageIsVisible(browser, "Email", "An email address is required");
        articlepage.assertValidationMessageIsVisible(browser, "Name", "Your name is required");
        articlepage.assertValidationMessageIsVisible(browser, "Subject", "A subject is required");
        articlepage.assertValidationMessageIsVisible(browser, "Message", "An enquiry message is required");

        articlepage.enterTextIntoFormField(browser, "Email", "invalid");
        articlepage.assertValidationMessageIsVisible(browser, "Email", "This is not a valid email address");

        articlepage.enterTextIntoFormField(browser, "Message", "This is a lot of text. Too much for this box. This is a lot of text." +
            " Too much for this box. This is a lot of text. Too much for this box. This is a lot of text. Too much for this box. " +
            "This is a lot of text. Too much for this box. This is a lot of text. Too much for this box. This is a lot of text. " +
            "Too much for this box. This is a lot of text. Too much for this box. This is a lot of text. Too much for this box. " +
            "This is a lot of text. Too much for this box. This is a lot of text. This is the end of the test.");
    },
    'Navigate to showcase and validate details  ': function (browser) {
        var showcasePage = browser.page.stockportgov.showcase();
        showcasePage.navigate();
        showcasePage.assertBreadcrumbIsVisible();
        showcasePage.assertHeroImageIsVisible();
        showcasePage.assertTitleIsVisible("UITEST: showcase page");
        showcasePage.assertTeaserIsVisible(browser, "this is a spotlight page for ui-test");
        showcasePage.assertCurrentConsulationsIsVisible(browser);
        showcasePage.assertSubheadingIsVisible(browser, "spotlight subheading");
        showcasePage.assertFeaturedTopicsAreVisible();
        showcasePage.assertNewsSectionVisible();
        showcasePage.assertEventsSectionVisible();

    },
    'Navigate to groups from homepage and validate details': function (browser) {
        browser.maximizeWindow();
        // get the groups page
        var groups = browser.page.stockportgov.groups();
        // navaigate to home page
        groups.navigate();
        // assertions
        groups.goToGroupsHomePage(browser);
        groups.assertTitleIsVisible("Find a local group");
        groups.assertAddAGroupButtonIsVisibleAndGotToPage(browser);
        groups.assertCanSubmitFormAndGetValidationErrors(browser);

    },
    'Navigate to a groups upcoming events page from the groups management page': function (browser) {
        // get the groups page
        var manageGroups = browser.page.stockportgov.manageGroups();
        // navaigate to home page
        manageGroups.navigate();
        // assertions
        manageGroups.assertTitleIsVisible("Manage your groups");
        manageGroups.assertGroupIsVisibleAndGoToManagePage(browser);
        manageGroups.assertManageGroupsTitleIsVisible("UITEST: A group for ui testing");
        manageGroups.assertViewEventsButtonIsVisibleAndClickIt(browser);
        manageGroups.assertManageGroupsEventsTitleIsVisible("Manage your groups");
        manageGroups.assertGroupsEventIsVisibleAndClickIt(browser);
        manageGroups.assertEventsTitleIsVisible("UITEST: Hats Amazing");
    },
    after: function (browser, done) {
        setTimeout(function () {
            done();
            browser.end();
        }, 200);
    }
};
