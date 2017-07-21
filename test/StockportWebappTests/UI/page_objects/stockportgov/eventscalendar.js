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

    goToFirstEvent: function (browser) {
        this.waitForElementVisible('@eventsList', this.api.globals.timeOut);
        browser.useCss().assert.visible("h3")
            .click("h3");
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
      return this.api.globals.testUri + "/events";
  },
  elements: {
      pageTitle: "h1",
      eventsList: "#event-listing-container"
  }
};
