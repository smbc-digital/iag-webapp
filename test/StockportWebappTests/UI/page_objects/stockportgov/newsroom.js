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
        this.expandCategories(browser);
        browser.useXpath().click("//a[contains(@href,'/news?category=" + category + "')]");
        this.expandCategories(browser);
    },

    expandCategories: function (browser) {
        browser.useXpath().click("//p[contains(@class, 'mobile-filter-heading') and text()='Filter news']");
        browser.useXpath().click("//p[@class='filter-title' and text()='Category']");
        this.waitForElementVisible('@newsList', this.api.globals.timeOut);
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
      categoryList: "#category-list"
  }
};
