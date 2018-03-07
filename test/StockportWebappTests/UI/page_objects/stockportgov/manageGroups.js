var methods = {
 
    assertTitleIsVisible: function (browser, title) {
        browser.useCss();
        this.waitForElementVisible('h1', this.api.globals.timeOut)
            .expect.element('h1').text.to.equal(title);
    },

    assertElementIsVisibleAndClickIt: function (browser, id) {
        browser.useCss();
        scrollToElement(browser, "#" + id);
        browser.useXpath().waitForElementVisible(".//*[@id='" + id + "']", this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='" + id + "']");
    },

    assertGroupIsVisibleAndGoToManagePage: function(browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='group-item-uitest-a-group-for-ui-testing']",
            this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='group-item-uitest-a-group-for-ui-testing']");
    },

    assertManageGroupsTitleIsVisible: function(title) {
        this.waitForElementVisible('@manageGroupPageTitle', this.api.globals.timeOut)
            .expect.element('@manageGroupPageTitle')
            .text.to.equal(title);
    },

    assertManageGroupsHeadingIsVisible: function(title) {
        this.waitForElementVisible('@manageGroupPageHeading', this.api.globals.timeOut)
            .expect.element('@manageGroupPageHeading')
            .text.to.equal(title);
    },

    assertViewEventsButtonIsVisibleAndClickIt: function(browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='view-group-events']", this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='view-group-events']");
    },

    assertManageGroupsEventsTitleIsVisible: function(title) {
        this.waitForElementVisible('@manageGroupPageTitle', this.api.globals.timeOut)
            .expect.element('@manageGroupPageTitle')
            .text.to.equal(title);
    },

    assertGroupsEventIsVisibleAndClickIt: function(browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='event-item-hats-amazing']", this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='event-item-hats-amazing']");
    },

    assertEventsTitleIsVisible: function(title) {
        this.waitForElementVisible('@eventPageTitle', this.api.globals.timeOut)
            .expect.element('@eventPageTitle')
            .text.to.equal(title);
    },

    assertMultistepFormHeadingIsVisible: function(title) {
        this.waitForElementVisible('@multistepFormHeading', this.api.globals.timeOut)
            .expect.element('@multistepFormHeading')
            .text.to.equal("1. About your group or service");
    },

    assertEditGroupIsVisibleAndGoToEditGroupPage: function(browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='edit-group']", this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='edit-group']");
    },

    assertAddOrRemoveUsersIsVisibleAndGoToAddOrRemoveUsers: function (browser) {
        browser.useXpath().waitForElementVisible(".//*[@id='add-or-remove-users']", this.api.globals.timeOut);
        browser.useXpath().click(".//*[@id='add-or-remove-users']");
    },

    assertUserIsVisibleOnManageUsersPage: function (browser) {
        browser.useCss();
        browser.waitForElementVisible("a[href='/groups/manage/uitest-a-group-for-ui-testing/edituser?email=ui@testestestest.com']", this.api.globals.timeOut);
        browser.click("a[href='/groups/manage/uitest-a-group-for-ui-testing/edituser?email=ui@testestestest.com']");
    },

    clearNameValueFromForm: function(browser) {
        browser.useCss();
        browser.clearValue('input[name="Name"]');   
    },

    enterValidName: function(browser) {
        browser.useCss();
        browser.setValue('input[name="Name"]', 'UITEST: A group for ui testing');
    },

    assertNextIsVisibleButtonAndClick: function (browser) {
        scrollToElement(browser, "a[href='#next']");
        this.waitForElementVisible("a[href='#next']", this.api.globals.timeOut).click("a[href='#next']");
    },

    assertValidationErrors: function(browser, validationDiv, validationText) {
        browser.useCss();
        this.waitForElementVisible(validationDiv, this.api.globals.timeOut);
        this.expect.element(validationDiv).text.to.equal(validationText);
    },

    assertEditGroupButtonAndClick: function(browser) {
        browser.useCss();
        this.waitForElementVisible("@uiTestContactFormSubmit", this.api.globals.timeOut);
        this.click("@uiTestContactFormSubmit");
    },
    assertEventIsVisibleAndClickIt: function (browser) {
        browser.useCss();
        browser.waitForElementVisible("a[href='/groups/manage/uitest-a-group-for-ui-testing/events/uitest-event']", this.api.globals.timeOut);
        browser.click("a[href='/groups/manage/uitest-a-group-for-ui-testing/events/uitest-event']");
    },
};

function scrollToElement(browser, css) {
    browser.getLocationInView(css, function (result) {
        browser.execute('scrollTo(0, ' + result.value.y + ')')
    });
}

module.exports = {
    commands: [methods],
    url: function () {
        return this.api.globals.testUri + "/groups/manage";
    },
    elements: {
        managePageTitle: "#manage-groups-heading",
        groupId: "#group-item-uitest-a-group-for-ui-testing",
        manageGroupPageTitle: "#manage-group-title",
        viewEventsForGroup: "#view-group-events",
        eventPageTitle: "#event-title",
        manageGroupPageHeading: "#manage-groups-heading",
        editGroupFormValidationDiv: "span[for='Name']",
        multistepFormHeading: "#multistep-form-sections-wrapper-t-0",
        uiTestContactFormSubmit: "#uitest-contact-form-submit",
        addEventFormValidationDiv: "span[data-valmsg-for='Title']"
    }
};
