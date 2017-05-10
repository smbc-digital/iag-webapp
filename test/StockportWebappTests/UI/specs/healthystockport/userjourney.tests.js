module.exports = {
    before: function (browser) {
        var homepage = browser.page.healthystockport.homepage();
        homepage.navigate();
        browser.useXpath().assert.visible("//div[@class='topic-block-content']/h4");
    },

    'Deliberately failing test': function (browser) {
        browser.useXpath().assert.visible("//div[@class='bibbidy-bobbidy-boo']/h4");
    },

    'Navigating to article "UITEST: A balanced diet", navigationg to the contact us page and then returning to the homepage': function (browser) {
        var homepage = browser.page.healthystockport.homepage();
        homepage.goToTopicListBlockPage(browser, "UITEST: Healthy weight", "/topic/uitest-healthy-weight");

        var topicpage = browser.page.healthystockport.topicpage();
        topicpage.assertTitleIsVisible('UITEST: Healthy weight');
        topicpage.goToArticlePage(browser, "UITEST: A balanced diet", "uitest-a-balanced-diet");


        var articlepage = browser.page.healthystockport.articlepage();
        articlepage.assertTitleIsVisible("UITEST: A balanced diet");
        articlepage.assertProfileIsVisible("Brinnington, Stockport");
        articlepage.goToNextSection(browser);
        articlepage.goToContactUsPage(browser);

        var contactuspage = browser.page.healthystockport.contactuspage();
        contactuspage.assertTitleIsVisible("Contact Us");

        contactuspage.goToHomePage(browser);

        homepage.assertTitleIsVisible("People of Stockport");
    },
   
    after: function (browser, done) {
        setTimeout(function () {
            done();
            browser.end();
        }, 200);
    }
};
