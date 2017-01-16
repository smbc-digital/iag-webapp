var methods = {
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut)
            .expect.element('@pageTitle').text.to.equal(title);
    },

    assertNewsSideBarVisible: function (browser,title) {
        this.waitForElementVisible('@sideBar', this.api.globals.timeOut)
            .expect.element('@sideBarTitle').text.to.equal(title);
    },

    assertTimestampPresent: function (browser) {
        this.waitForElementVisible('@timeStamp', this.api.globals.timeOut)
            .expect.element('@timeStamp').text.to.contain("Last updated");
    },

    assertNewsSharePresent: function(browser) {
        this.waitForElementVisible('@shareIT', this.api.globals.timeOut)
            .expect.element('@shareThis')
            .text.to.equals("Share this");
        this.waitForElementVisible('@addThisIcons', this.api.globals.timeOut);
    },

    assertNewsTagIsVisible: function(tag) {
        this.waitForElementVisible('@newsTag', this.api.globals.timeOut)
            .expect.element('@newsTag').text.to.equal(tag);
    },

    assertDocumentIsVisible:function() {
        this.waitForElementVisible('@documentTag', this.api.globals.timeOut)
            .expect.element('@documentHeading')
            .text.to.equal("UITEST: Document");
    },

    assertAllCategoriesAreVisible: function (browser) {
        this.assertCategoryIsVisible(browser, "Benefits");
        this.assertCategoryIsVisible(browser, "Business");
        this.assertCategoryIsVisible(browser, "Children%20and%20families");
        this.assertCategoryIsVisible(browser, "Council%20leader");
        this.assertCategoryIsVisible(browser, "Crime%20prevention%20and%20safety");
        this.assertCategoryIsVisible(browser, "Elections");
        this.assertCategoryIsVisible(browser, "Environment");
        this.assertCategoryIsVisible(browser, "Health%20and%20social%20care");
        this.assertCategoryIsVisible(browser, "Housing");
        this.assertCategoryIsVisible(browser, "Jobs");
        this.assertCategoryIsVisible(browser, "Leisure%20and%20culture");
        this.assertCategoryIsVisible(browser, "Libraries");
        this.assertCategoryIsVisible(browser, "Licensing");
        this.assertCategoryIsVisible(browser, "Partner%20organisations");
        this.assertCategoryIsVisible(browser, "Planning%20and%20building");
        this.assertCategoryIsVisible(browser, "Roads%20and%20travel");
        this.assertCategoryIsVisible(browser, "Schools%20and%20education");
        this.assertCategoryIsVisible(browser, "Test%20Category");
        this.assertCategoryIsVisible(browser, "Waste%20and%20recycling");
    },

    assertCategoryIsVisible: function (browser, category) {
        browser.useXpath().assert.visible("//a[contains(@href,'/news?category=" + category + "')]");
    }
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri + "/news/";
  },
  elements: {
      pageTitle: "h1",
      sideBar: ".l-left-side-bar.grid-30.tablet-grid-100.mobile-grid-100.grid-parent",
      sideBarTitle:".l-left-side-bar-section>h3",
      timeStamp: ".news-date.news-article-date",
      shareIT:".share",
      shareThis:".share>h6",
      addThisIcons: ".addthis_toolbox",
      newsTag: "li.news-tag a",
      documentTag: '.test-documents',
      documentHeading: '.test-document-detail'
  }
};
