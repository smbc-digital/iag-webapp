var methods = {
    // assertions go in here
    goToGroupsHomePage: function (browser) {
        this.waitForElementVisible('.button-groups', this.api.globals.timeOut);
        browser.useXpath().click("//a[contains(@href,'/groups')]");
    },
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut)
            .expect.element('@pageTitle')
            .text.to.equal(title);
    },
};

module.exports = {
    commands: [methods],
    url: function () {
        // where to start the tests from in this file
        return this.api.globals.testUri + "/";
    },
    elements: {
        pageTitle: "h1"
    }
};
