var methods = {
    goToTopicListBlockPage: function (browser, title, link) {
        this.waitForElementVisible('@topicList', this.api.globals.timeOut);
        browser.maximizeWindow();
        browser.useXpath()
            .assert.visible("//h4[@class='featured-topic-name' and text()='" + title + "']")
            .click("//a[contains(@href,'/topic/" + link + "')]");

        this.waitForElementVisible('@topicChildLibraries', this.api.globals.timeOut);
        this.waitForElementVisible('@topicChildLibrariesMore', this.api.globals.timeOut);
        browser.useXpath()
            .click("//div[contains(@class,'topic-child-more')]//a[contains(@href,'/topic/" + link + "')]");
    },

    goToTopTasksBlockPage: function(browser, title, link) {
        this.waitForElementVisible('@taskList', this.api.globals.timeOut);
        browser.useXpath().assert.visible("//div[contains(@class,'uitest-task-title') and text()='" + title + "']")
            .click("//a[contains(@href,'"+ link + "')]");
    },

    searchForBins: function(browser) {
        this.waitForElementVisible('@searchBar', this.api.globals.timeOut);
        browser.setValue('div.search-bar > form > input[name="query"]', 'bins')
            .click("div.search-bar > form > button.search-button");
    },

    searchForPostCode: function (browser) {
        this.waitForElementVisible('@postCodeSearch', this.api.globals.timeOut);
        browser.setValue('input[type=text].light-on-dark-field', 'SK7 3EU').click("button[type=submit].button-call-to-action");
    },

    checkForWebCastImage: function (browser) {
        this.waitForElementVisible('.webcast', this.api.globals.timeOut);        
    },

    assertNewsBannerIsVisible: function (browser) {
            this.waitForElementVisible('@newsroomCallToAction', this.api.globals.timeOut);
            this.waitForElementVisible(".grid-parent.grid-100.homepage-news-items", this.api.globals.timeOut);
            this.waitForElementVisible(".grid-50.mobile-grid-100.homepage-news-item", this.api.globals.timeOut)
            .expect.element("#test-newsroom-calltoaction>h2").text.to.equal("Latest News");
    },

    assertEmailAlertsIsVisible: function (browser, buttonText) {
        this.waitForElementVisible('@emailAlerts', this.api.globals.timeOut)
            .expect.element('@subscribeButton').text.to.equal(buttonText);
    },

    assertAtoZListIsVisible: function (browser) {
        this.waitForElementVisible('@atozList', this.api.globals.timeOut);
        this.waitForElementVisible('div.alphabet', this.api.globals.timeOut);
        browser.expect.element('.atoz>h2').text.to.equal("Find services A-Z");
    },

    goToNewsroom: function(browser) {
        this.waitForElementVisible('@newsroomCallToAction', this.api.globals.timeOut)
            .expect.element('@newsroomLink').text.to.equal("More News");
        browser.click("#test-newsroom-link");
    },

    goToAtoZList: function(browser, letter) {
        this.waitForElementVisible('@atozList', this.api.globals.timeOut);
        browser.useXpath()
            .click("//a[contains(@href,'/atoz/" + letter + "')]");
    }
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri;
  },
  elements: {
      topicList: '#test-featured-topic-list',
      topicChildLibraries: '#featured-topic-children-uitest-hat-works',
      topicChildLibrariesMore: '#featured-topic-children-uitest-hat-works .topic-child-more a',
      taskList: '.uitest-task-block-list-loaded',
      searchBar: '.search-bar',
      postCodeSearch: 'input[type=text].light-on-dark-field',
      webCast: '.webcast',
      emailAlerts: '#test-email-alerts',
      subscribeButton: "#test-subscribe",
      newsroomLink: "#test-newsroom-link",
      newsroomCallToAction: "#test-newsroom-calltoaction",
      atozList: '.atoz'
  }
};
