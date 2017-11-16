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
    assertAddAGroupButtonIsVisibleAndGotToPage: function (browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='add-group-button']", this.api.globals.timeOut)
            .expect.element(".//*[@id='add-group-button']")
            .text.to.equal("Add your group or service");

        browser.useXpath().click(".//*[@id='add-group-button']");
    },
    assertCanSubmitFormAndGetValidationErrors: function (browser) {
        browser.useCss();

        this.waitForElementVisible("a[href='#next']", this.api.globals.timeOut).click("a[href='#next']");
        
        this.expect.element(
                "@addGroupFormValidationDiv")
            .text.to.equal("The Enter the name of your group or service field is required.");

        // take back to the main /groups/ page for further testing using breadrumb
        browser.click(".breadcrumb>li>a[href='/groups/']");
    }
};

module.exports = {
    commands: [methods],
    url: function () {
        // where to start the tests from in this file
        return this.api.globals.testUri + "/";
    },
    elements: {
        pageTitle: "h1",
        addGroupFormValidationDiv: ".form-field-validation-error.grid-50.tablet-grid-50.suffix-50.tablet-suffix-50.mobile-grid-100.grid-parent.field-validation-error>span[for='Name']"
    }
};
