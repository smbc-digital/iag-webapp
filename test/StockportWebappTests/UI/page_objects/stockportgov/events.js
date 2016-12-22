var methods = {
    assertEventTitleIsVisible: function (title) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut)
            .expect.element('@pageTitle').text.to.equal(title);
    },

    assertHeadingStampPresent: function (browser) {
        this.waitForElementVisible('@headingStamp', this.api.globals.timeOut)
            .expect.element('@headingStamp').text.to.contain("Date and time");
    },

    assertDescriptionPresent: function (browser) {
        this.waitForElementVisible('@description', this.api.globals.timeOut)
            .expect.element('@description').text.to.contain("Wellington Mill");
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
