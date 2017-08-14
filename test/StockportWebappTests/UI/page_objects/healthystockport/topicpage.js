var methods = {
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@topicTitle', this.api.globals.timeOut)
            .expect.element('@topicTitle').text.to.equal(title);
    },

    goToArticlePage: function (browser, title, link) {
        this.waitForElementVisible('@topicList', this.api.globals.timeOut);
        browser.useXpath().assert.visible("//li[@class='article-list-item article-list-item-mobile grid-50 tablet-grid-50 mobile-grid-100 matchbox-item']")
            .click("//li/a[contains(@href,'" + link + "')]");
    }
};

module.exports = {
    commands: [methods],
    url: function() {
        // This is giving us a page object for the index method
        return this.api.globals.testUri + 'topic';
    },
    elements: {
        topicTitle: "h2",
        topicList: ".article-list"
    }
};
