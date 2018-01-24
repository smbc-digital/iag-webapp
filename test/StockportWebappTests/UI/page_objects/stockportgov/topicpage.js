var methods = {
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@topicTitle', this.api.globals.timeOut)
            .expect.element('@topicTitle').text.to.equal(title);
	},

	//assertSubItemTitleIsVisible: function (title) {
	//	this.waitForElementVisible('@subItemTitle', this.api.globals.timeOut)
	//		.expect.element('@subItemTitle').text.to.equal(title);
	//},

    goToTopicListBlockPage: function (browser, title, link) {
        this.waitForElementVisible('@topicBody', this.api.globals.timeOut);
        browser.useXpath().assert.visible("//h3[@class='sub-item-title' and text()='" + title + "']")
			.click("//h3[@class='sub-item-title' and text()='" + title + "']");
    },

    assertSecondaryItemIsVisible: function(browser,title) {
        this.waitForElementVisible('@secondaryTopicList', this.api.globals.timeOut)
            .expect.element('#test-topic-page-secondary-topics-list .subitem-secondary').text.to.include(title);
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
		subItemTitle: "h3",
        topicBody: "body",
        secondaryTopicList: "#test-topic-page-secondary-topics-list"
    }
};
