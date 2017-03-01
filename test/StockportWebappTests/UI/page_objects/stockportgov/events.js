var methods = {
    assertEventHasTitle: function (browser) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut);
        browser.useCss().assert.visible("h1.events-article-title");
    },

    assertHeadingStampPresent: function (browser) {
        this.waitForElementVisible('@headingStamp', this.api.globals.timeOut)
            .expect.element('@headingStamp').text.to.contain("Date and time");
    }
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri + "/events/";
  },
  elements: {
      pageTitle: "h1",
      headingStamp: ".events-details-heading",
      description: "article"
  }
};
