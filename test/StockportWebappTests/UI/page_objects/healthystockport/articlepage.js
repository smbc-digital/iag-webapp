var methods = {
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@articleTitle', this.api.globals.timeOut)
            .expect.element('@articleTitle').text.to.equal(title);
    },

    assertProfileIsVisible: function (title) {
        this.waitForElementVisible('@profile', this.api.globals.timeOut)
            .expect.element('@profileSubtitle').text.to.equal(title);
    },

    assertButoVideoIsVisible: function() {
        this.waitForElementPresent('@butoDiv', 50000);
    },

    assertTableIsVisible: function() {
        this.waitForElementVisible('@tableTag', this.api.globals.timeOut)
            .expect.element('@tableHeader').text.to.equal("Header");
    },

    goToProfile: function(browser,link) {
        this.waitForElementVisible('@profile', this.api.globals.timeOut);
        browser.useXpath().click("//a[contains(@href,'/profile/" + link + "')]");
    },

    goToNextSection: function (browser) {
        this.waitForElementVisible('@paginationNext', this.api.globals.timeOut);
        browser.useXpath().click("//div[contains(@class,'pagination-next')]/a");
    },

    goToHomePage: function (browser) {
        this.waitForElementVisible('@header', this.api.globals.timeOut);
        browser.useXpath().click("//a[@href=='/']");
    },

    goToContactUsPage: function (browser) {
        this.waitForElementVisible('@sideBarBlock', this.api.globals.timeOut);
        browser.useXpath().click("//a[@href='/contact-us']");
    }
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri + '/article';
  },
  elements: {
      articleTitle: 'h1',
      articleContainer: '.l-article-container',
      paginationNext: '.pagination-next',
      profile: '.profile',
      profileSubtitle: '.profile-subtitle',
      butoDiv: '#buto_kQl5D',
      baseVideoTitle: '.base-video-title',
      tableTag: 'table',
      tableHeader: 'th',
      tableCell: 'td',
      sideBarBlock: '.sidebar-right',
      header: '#header'
  }
};
