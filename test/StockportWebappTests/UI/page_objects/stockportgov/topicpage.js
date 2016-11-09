var methods = {
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@topicTitle', this.api.globals.timeOut)
            .expect.element('@topicTitle').text.to.equal(title);
    },

    goToTopicListBlockPage: function (browser, title, link) {
        this.waitForElementVisible('@topicList', this.api.globals.timeOut);
        browser.useXpath().assert.visible("//h2[text()='" + title + "']")
            .click("//a[contains(@href,'" + link + "')]");
    },

    assertSecondaryItemIsVisible: function(browser,title) {
        this.waitForElementVisible('@secondaryTopicList', this.api.globals.timeOut)
            .expect.element('#test-secondary-topic-title').text.to.equal(title);
    }

};

module.exports = {
    commands: [methods],
    url: function() {
        // This is giving us a page object for the index method
        return this.api.globals.testUri + 'topic';
    },
    elements: {
        topicTitle: "h1",
        topicList: "#test-topic-page-subitem-list",
        secondaryTopicList: "#test-topic-page-secondary-topics-list"
    }
};
