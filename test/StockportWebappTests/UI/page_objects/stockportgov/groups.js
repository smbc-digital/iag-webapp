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
            .text.to.equal("Add your group");

        browser.useXpath().click(".//*[@id='add-group-button']");
    },
    assertCanSubmitFormAndGetValidationErrors: function (browser) {
        browser.useCss();

        this.waitForElementVisible("#uitest-contact-form-submit", this.api.globals.timeOut).click("#uitest-contact-form-submit");

        this.waitForElementVisible(
            "@addGroupFormValidationDiv",
            this.api.globals.timeOut);

        this.expect.element(
                "@addGroupFormValidationDiv")
            .text.to.equal("The Group name field is required.");

        this.setValue("#Name", "a name").click('#uitest-contact-form-submit');

        this.waitForElementNotPresent(
            "@addGroupFormValidationDiv", this.api.globals.timeOut);

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
