var methods = {
    goToTopicListBlockPage: function (browser, title) {
        this.waitForElementVisible('@topicList', this.api.globals.timeOut);
        browser.useXpath()
            .assert.visible("//div[@class='featured-topic-name' and text()='" + title + "']")
            .click("//div[@class='featured-topic-name' and text()='" + title + "']");
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

    // searchForPostCode: function (browser) {
    //     this.waitForElementVisible('@postCodeSearch', this.api.globals.timeOut);
    //     browser.setValue('input[type=text].light-on-dark-field', 'SK7 3EU').click("button[type=submit].button-default");
    // },

    checkForWebCastImage: function (browser) {
        this.waitForElementVisible('.webcast', this.api.globals.timeOut);
    },

    assertNewsBannerIsVisible: function (browser) {
            this.waitForElementVisible('@newsroomCallToAction', this.api.globals.timeOut);
            this.waitForElementVisible(".latest-nav-card-item", this.api.globals.timeOut)
            .expect.element(".news.title").text.to.equal("Latest news");
    },

    assertEventsBannerIsVisible: function (browser) {
        this.waitForElementVisible('@eventscalendarCallToAction', this.api.globals.timeOut)
        .expect.element(".event.title").text.to.equal("What's on in Stockport");
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
        this.waitForElementVisible('@newsroomCallToAction', this.api.globals.timeOut);
        browser.useXpath().click("//div[@class='full-width-white']//a[.='View more news']");
    },

    goToEventsCalendar: function (browser) {
        this.waitForElementVisible('@eventscalendarCallToAction', this.api.globals.timeOut);
        //.expect.element('@eventcalendarLink').text.to.equal("View Events");
        browser.useXpath().click("//div[@class='full-width-white']//a[.='View more events']");
    },

    goToAtoZList: function(browser, letter) {
        this.waitForElementVisible('@atozList', this.api.globals.timeOut);
        browser.useXpath()
            .click("//a[contains(@href,'/atoz/" + letter + "')]");
    },
  //    closeCookieBanner: function(browser) {
  //     this.waitForElementVisible('.cc_banner.cc_container.cc_container--open', this.api.globals.timeOut);
  //      browser.pause(700);
  //      browser.click(".cc_btn.cc_btn_accept_all");
  // }
};

module.exports = {
  commands: [methods],
  url: function () {
    // This is giving us a page object for the index method
      return this.api.globals.testUri;
  },
  elements: {
      topicList: '.primary-topics',
      taskList: '.uitest-task-block-list-loaded',
      searchBar: '.search-bar',
      postCodeSearch: 'input[type=text].light-on-dark-field',
      webCast: '.webcast',
      emailAlerts: '#test-email-alerts',
      subscribeButton: "#test-subscribe",
      newsroomLink: "#test-newsroom-link",
      newsroomCallToAction: ".featured-topic-list",
      eventcalendarLink: "#test-eventscalendar-link",
      eventscalendarCallToAction: ".featured-topic-list",
      atozList: '.atoz'
  }
};
