var methods = {
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut)
            .expect.element('@pageTitle').text.to.equal(title);
    },

    goToNewsWithTitle: function (browser, title, link) {
        this.waitForElementVisible('@newsList', this.api.globals.timeOut);
        browser.useXpath().assert.visible("//h2[@class='nav-card-news-title' and text()='" + title + "']")
            .click("//a[contains(@href,'" + link + "')]");
    },

    goToCategory: function (browser, category) {
        this.waitForElementVisible('@categoryList', this.api.globals.timeOut);
        browser.useXpath().click("//a[contains(@href,'/news?category=" + category + "')]");
        this.waitForElementVisible('@newsList', this.api.globals.timeOut);
    },

    expandCategoriesFromScratch: function (browser) {
        this.expandNewsFilter(browser);
        this.expandCategories(browser);
    },

    expandNewsFilter: function (browser) {
        browser.useXpath().click("//p[contains(@class, 'mobile-filter-heading') and text()='Filter news']");
    },

    expandCategories: function (browser) {
        browser.useXpath().click("//p[@class='filter-title' and text()='Category']");
        this.waitForElementVisible('@newsList', this.api.globals.timeOut);
    },

    expandNews: function (browser) {
        browser.useXpath().click("//p[@class='filter-title' and text()='News archive']");
        this.waitForElementVisible('@newsArchive', this.api.globals.timeOut);
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
    },

    assertLinkIsActive: function (browser, category) {
        var textIsCategory = "//a[text()='" + category + "']";
        var containsActiveClass = "contains(@class, 'active')";

        browser.useXpath().assert.visible(textIsCategory + "/parent::li[" + containsActiveClass + "]");
    },

    assertLinkIsNotActive: function (browser, category) {
        var hrefContainsUrl = "//a[contains(@href,'/news?category=" + category + "')]";
        var textIsCategory = "//a[text()='" + category + "']";
        var doesNotcontainActiveClass = "not(contains(@class, 'active'))";

        browser.useXpath().assert.visible(textIsCategory + "/parent::li[" + doesNotcontainActiveClass + "]");
    },

    assertLinkHasCorrectUrl: function (browser, category, url) {
        browser.useXpath().assert.visible("//a[@href='" + url + "' and text()='" + category + "']");
    }
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri + "/news";
  },
  elements: {
      pageTitle: "h1",
      newsList: ".nav-card-news-list",
      newsArchive: "#uitest-news-archive",
      categoryList: "#category-filter"
  }
};
