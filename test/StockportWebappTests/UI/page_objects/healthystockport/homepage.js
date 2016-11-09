var methods = {
    goToTopicListBlockPage: function (browser, title, link) {
        this.waitForElementVisible('@topicList', this.api.globals.timeOut);     
        browser.useXpath().assert.visible("//div[@class='topic-block-content']/h4")
            .click("//a[contains(@href,'" + link + "')]/div[@class='topic-block-content']");
    },

    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@homeTitle', this.api.globals.timeOut)
            .expect.element('@homeTitle').text.to.equal(title);
    },
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri;
  },
  elements: {
      topicList: '.topic-block-list',
      topicHeader: '.topic-block-content > h4',
      homeTitle:'h1'

  }
};
