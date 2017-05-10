module.exports = {
    before: function (browser) {
        var homepage = browser.page.healthystockport.homepage();
        homepage.navigate();
    },

    'Navigating to article "UITEST: A balanced diet", navigationg to the contact us page and then returning to the homepage': function (browser) {
        var homepage = browser.page.healthystockport.homepage();
        homepage.goToTopicListBlockPage(browser, "UITEST: A balanced diet", "/topic/uitest-healthy-weight");

        var topicpage = browser.page.healthystockport.topicpage();
        topicpage.assertTitleIsVisible('UITEST: Healthy weight');
        topicpage.goToArticlePage(browser, "UITEST: A balanced diet", "uitest-a-balanced-diet");


        var articlepage = browser.page.healthystockport.articlepage();
        articlepage.assertTitleIsVisible("UITEST: Getting started");
        articlepage.assertProfileIsVisible("Brinnington, Stockport");
        articlepage.goToNextSection(browser);

        //articlepage.goToContactUsPage(browser);

        //var contactuspage = browser.page.healthystockport.contactuspage();
        //contactuspage.assertTitleIsVisible("Contact Us");

        //contactuspage.goToHomePage(browser);

        //homepage.assertTitleIsVisible("People of Stockport");
    },
   
    after: function (browser, done) {
        setTimeout(function () {
            done();
            browser.end();
        }, 200);
    }
};
