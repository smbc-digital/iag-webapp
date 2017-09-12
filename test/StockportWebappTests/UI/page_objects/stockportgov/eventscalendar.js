var methods = {
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut)
            .expect.element('@pageTitle').text.to.equal(title);
    },

    goToEventsWithTitle: function (browser, title, link) {
        this.waitForElementVisible('@eventsList', this.api.globals.timeOut);
        browser.useXpath().assert.visible("//h3[@text()='" + title + "']")
            .click("//a[contains(@href,'" + link + "')]");
    },
    assertPrimaryFilter: function(browser){
      browser.useCss();
      this.waitForElementVisible('#events-filter-bar', this.api.globals.timeOut);
      this.waitForElementVisible('#DateFrom', this.api.globals.timeOut);
      this.waitForElementVisible('#DateTo', this.api.globals.timeOut);
      this.waitForElementVisible('#Category', this.api.globals.timeOut);

    },
    selectDropdownCategoryAndSearch: function(browser, category){
      browser.useCss().click("#Category");
      this.waitForElementVisible("#Category option[value='" + category +"']", this.api.globals.timeOut);
      browser.click("#Category option[value='" + category +"']");
      browser.click("button.button-default.button-outline-white-transparent");
      browser.assert.urlContains(category);
      // this.waitForElementVisible('#listing-refine-bar', this.api.globals.timeOut);
    },
    goToFirstEvent: function (browser) {
        this.waitForElementVisible('@eventsList', this.api.globals.timeOut);
        browser.useCss().assert.visible("h3.events-link")
            .click("h3.events-link");
    },

    removeFirstFilter: function (browser) {
        this.waitForElementVisible('@eventsList', this.api.globals.timeOut);
        //browser.useCss().click("li.filters-active li:first-child a");
    }
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri + "/events?FromSearch=true";
  },
  elements: {
      pageTitle: "h1",
      eventsList: "#event-listing-container"
  }
};
