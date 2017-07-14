var methods = {
    // assertions go in here
    goToGroupsHomePage: function (browser) {
        this.waitForElementVisible('.button-groups', this.api.globals.timeOut);
        browser.useXpath().click("//a[contains(@href,'/groups') and contains(@class,'button-groups')]");
    },
    assertTitleIsVisible: function (title) {
        this.waitForElementVisible('@pageTitle', this.api.globals.timeOut)
            .expect.element('@pageTitle')
            .text.to.equal(title);
    },
    assertAddAGroupButtonIsVisible: function (browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='add-group-button']", this.api.globals.timeOut)
            .expect.element(".//*[@id='add-group-button']")
            .text.to.equal("Add your group");
    }
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
