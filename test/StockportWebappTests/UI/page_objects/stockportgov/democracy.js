var methods = {
    assertTitleIsVisible: function(browser) {
      browser.assert.title("{pagetitle} - Stockport Council");
    },

    assertBreadcrumbVisible: function(browser) {
      browser.useCss();
        this.waitForElementVisible('.breadcrumb', this.api.globals.timeOut)
          .expect.element('.breadcrumb').text.to.contain("Home");
      //" › About your council › Councillors › {breadcrumb}");
    },

    asserSideMenuVisible: function (browser) {
      browser.useCss();
      this.waitForElementVisible('.l-left-side-bar-section', this.api.globals.timeOut)
      .expect.element('.l-left-side-bar-section').text.to.contain("Constitution");
    }
};

module.exports = {
    commands: [methods],
    url: function() {
        // This is giving us a page object for the index method
        return this.api.globals.testUri + '/externaltemplates/democracy';
    },
    elements: {
        articleTitle: 'h1',

  }
};
